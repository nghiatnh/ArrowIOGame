using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOfflineController : MonoBehaviour
{
    public AudioSource attackSource;
    public AudioSource hitSource;
    public GameObject spike;
    public GameObject crit;
    public GameObject pet;
    public GameObject Gates;
    public LayerMask gateLayerMask;
    public LayerMask playerLayerMask;
    public LayerMask wallLayerMask;
    public BoxCollider getItemCollider;
    public delegate void AfterDie(GameObject killer);
    public AfterDie afterDie;
    private float lastTimeAttack = 0f;
    private float lastTimeSpike = 0f;
    private float lastTimeOnGate = 0f;
    private float startDeadTime = 0f;
    private bool IsOnGate = false;
    private GameObject killer;

    void Start(){
        GameInformation.Instance.characters.Add(GetComponent<CharacterInfo>());
        if (Gates == null) Gates = GameObject.Find("Map").transform.Find("Gate").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CharacterInfo>().health <= 0) {
            return;
        }
        // Check on gate
        bool isCollide = Physics.Raycast(transform.position, Vector3.down, 1f, gateLayerMask);
        if (isCollide && !IsOnGate){
            lastTimeOnGate = Time.realtimeSinceStartup;
            IsOnGate = true;
        }
        else if (IsOnGate && !isCollide) {
            IsOnGate = false;
        }
        else if (isCollide && IsOnGate && Time.realtimeSinceStartup - lastTimeOnGate >= 2f){
            if (OnGate())
                lastTimeOnGate = Time.realtimeSinceStartup;
        }
        // Check through wall
        if (GetComponent<CharacterInfo>().status.Contains(STATUS.THROUGH_WALL))
            ThroughWall(GetComponent<Rigidbody>().velocity / GetComponent<CharacterInfo>().speed);
    }

    void OnTriggerEnter(Collider collider){
        if (GetComponent<CharacterInfo>().health <= 0) return;
        // When collide a bullet
        if (collider.CompareTag("Bullet")){
            GameObject attacker = collider.GetComponent<BulletOfflineController>().parent;
            if (attacker == gameObject || attacker == null || (attacker.GetComponent<PetController>() != null && attacker.GetComponent<PetController>().parent == transform)) return; 
            float ATK = collider.GetComponent<BulletOfflineController>().ATK;
            GetDamage(ATK, attacker);
            //GetDamage(ATK, attacker);
            hitSource.Play();
            Destroy(collider.gameObject);
        }

        if (collider.CompareTag("Spike")){
            GameObject attacker = collider.GetComponent<BulletOfflineController>().parent;
            if (attacker == gameObject || attacker == null) return;
            //GetDamage(5, attacker);
            hitSource.Play();
            Destroy(collider.gameObject);
        }
    }
    
    // Call every frame when die
    public void Die(){
        afterDie.Invoke(killer);
    }

    // Create a bullet
    public void Attack(GameObject bullet, Vector3 position, Quaternion rotation){
        GameObject obj = Instantiate(bullet, position - new Vector3(0,-0.5f,0), rotation);
        BulletOfflineController controller = obj.GetComponent<BulletOfflineController>();
        controller.initSpeed = GetComponent<CharacterInfo>().range;
        controller.ATK = GetComponent<CharacterInfo>().ATK;
        controller.isThroughWall = gameObject.GetComponent<CharacterInfo>().status.Contains(STATUS.BULLET_THROUGH_WALL);
        controller.parent = gameObject;
        attackSource.Play();
    }

    public void CreateSpike(){
        if (Time.realtimeSinceStartup - lastTimeSpike >= 0.3f) {
            GameObject obj = Instantiate(spike, new Vector3(transform.position.x, 0, transform.position.z), new Quaternion(0, 0, 0, 1));
            obj.GetComponent<SpikeController>().parent = gameObject;
            lastTimeSpike = Time.realtimeSinceStartup;
        }
    }

    public void GetDamage(float ATK, GameObject attacker){
        // If CRIT
        if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.CRIT)){
            int r = Random.Range(0, 100);
            if (r < GameConstant.CritAverage(attacker.GetComponent<CharacterInfo>().status.FindAll(x => x == STATUS.CRIT).Count)) {
                ATK *= 2;
                crit.GetComponent<Animator>().SetTrigger("Crit");
            }
        }

        // Descrease Health
        GetComponent<CharacterInfo>().health = Mathf.Max(0, GetComponent<CharacterInfo>().health - ATK * GameConstant.INIT_ATK / (GameConstant.INIT_ATK + GetComponent<CharacterInfo>().DEF));
        
        // When die
        if (GetComponent<CharacterInfo>().health <= 0){
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().isTrigger = true;
            transform.Find("Health").gameObject.SetActive(false);
            if (attacker.CompareTag("Pet")) {attacker = attacker.GetComponent<PetController>().parent.gameObject;}
            // Attacker gain power
            attacker.GetComponent<CharacterInfo>().powerPoint = System.Math.Max(attacker.GetComponent<CharacterInfo>().powerPoint, GetComponent<CharacterInfo>().powerPoint) + Random.Range(10, 20) * GetComponent<CharacterInfo>().level;
            // If kill gain health
            if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.KILL_HP_UP))
                attacker.GetComponent<CharacterInfo>().GainHealth(20 * attacker.GetComponent<CharacterInfo>().status.FindAll(x => x == STATUS.KILL_HP_UP).Count);
            
            killer = attacker;
            startDeadTime = Time.realtimeSinceStartup;
            GetComponent<Animator>().SetTrigger("Die");
        }

        // Blood Sucking
        if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.BLOOD_SUCKING))
            attacker.GetComponent<CharacterInfo>().GainHealth((ATK / 5) * attacker.GetComponent<CharacterInfo>().status.FindAll(x => x == STATUS.BLOOD_SUCKING).Count);
    }

    public void CreatePet(){
        GameObject obj = Instantiate(pet, transform.position - new Vector3(-1, 0, 0), transform.rotation);
        obj.GetComponent<PetController>().parent = transform;
        obj.GetComponent<CharacterInfo>().ATK = GetComponent<CharacterInfo>().ATK / 9;
    }

    public bool OnGate(){
        int rd = Random.Range(0, Gates.transform.childCount);
        Transform gate = Gates.transform.GetChild(rd);
        if (Physics.BoxCast(gate.position, Vector3.one * 3f, Vector3.up, Quaternion.Euler(0,0,0), 2f, playerLayerMask)) return false;
        gate.GetComponent<AudioSource>().Play();
        transform.position = gate.position;
        return true;
    }

    private void ThroughWall(Vector3 moveDir){
        if (Physics.Raycast(gameObject.transform.position, moveDir, 0.55f, wallLayerMask) && !Physics.Raycast(transform.position + moveDir * 2f, moveDir, 1f, wallLayerMask))
            transform.position += moveDir * 2f;
    }
}
