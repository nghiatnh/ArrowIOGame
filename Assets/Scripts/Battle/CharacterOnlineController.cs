using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterOnlineController : MonoBehaviour
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
    private PhotonView view;

    void Start(){
        GameInformation.Instance.characters.Add(GetComponent<CharacterInfo>());
        if (Gates == null) Gates = GameObject.Find("Map").transform.Find("Gate").gameObject;
        view = gameObject.GetPhotonView();
        GetComponent<CharacterInfo>().onCreatePet = new CharacterInfo.OnCreatePet(CreatePet);
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine || GetComponent<CharacterInfo>().health <= 0) return;

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
            GameObject attacker = collider.GetComponent<BulletOnlineController>().parent;
            if (attacker == gameObject || attacker == null || (attacker.GetComponent<PetOnlineController>() != null && attacker.GetComponent<PetOnlineController>().parent.gameObject.GetPhotonView().ViewID == view.ViewID)) return; 
            float ATK = collider.GetComponent<BulletOnlineController>().ATK;
            if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.CRIT))
                ATK = CritDamge(ATK, attacker.GetComponent<CharacterInfo>().status.FindAll(x => x == STATUS.CRIT).Count);
            view.RPC("GetDamage", RpcTarget.All, ATK, attacker.GetPhotonView().ViewID);
            hitSource.Play();
            if (collider.gameObject.GetPhotonView().IsMine){
                collider.GetComponent<BulletOnlineController>().initSpeed = 0f;
                PhotonNetwork.Destroy(collider.gameObject);
            }
        }

        if (collider.CompareTag("Spike")){
            GameObject attacker = collider.GetComponent<BulletOnlineController>().parent;
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
        GameObject obj = PhotonNetwork.Instantiate(bullet.name, position - new Vector3(0,-0.5f,0), rotation);
        BulletOnlineController controller = obj.GetComponent<BulletOnlineController>();
        controller.initSpeed = GetComponent<CharacterInfo>().range;
        controller.ATK = GetComponent<CharacterInfo>().ATK;
        controller.isThroughWall = gameObject.GetComponent<CharacterInfo>().status.Contains(STATUS.BULLET_THROUGH_WALL);
        controller.parent = gameObject;
        attackSource.Play();
    }

    public void CreateSpike(){
        if (Time.realtimeSinceStartup - lastTimeSpike >= 0.3f) {
            GameObject obj = PhotonNetwork.Instantiate(spike.name, new Vector3(transform.position.x, 0, transform.position.z), new Quaternion(0, 0, 0, 1));
            obj.GetComponent<SpikeOnlineController>().SetParent(view.ViewID);
            lastTimeSpike = Time.realtimeSinceStartup;
        }
    }

    [PunRPC]
    public void GetDamage(float ATK, int attackerID){
        GameObject attacker = PhotonNetwork.GetPhotonView(attackerID).gameObject;

        // Descrease Health
        GetComponent<CharacterInfo>().health = Mathf.Max(0, GetComponent<CharacterInfo>().health - ATK * GameConstant.INIT_ATK / (GameConstant.INIT_ATK + GetComponent<CharacterInfo>().DEF));
        
        // When die
        if (GetComponent<CharacterInfo>().health <= 0){
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().isTrigger = true;
            transform.Find("Health").gameObject.SetActive(false);
            if (attacker.CompareTag("Pet")) {attacker = attacker.GetComponent<PetOnlineController>().parent.gameObject;}
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
        if (!view.IsMine) return;
        GameObject obj = PhotonNetwork.Instantiate(pet.name, transform.position - new Vector3(-1, 0, 0), transform.rotation);
        obj.GetComponent<PetOnlineController>().SetParent(view.ViewID);
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

    private float CritDamge(float ATK, int countCrit){
        int r = Random.Range(0, 100);
        if (r < GameConstant.CritAverage(countCrit)){
            ATK *= 2;
            crit.GetComponent<Animator>().SetTrigger("Crit");
        }
        return ATK;        
    }
}
