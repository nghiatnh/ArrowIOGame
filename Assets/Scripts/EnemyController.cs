using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;
    public GameObject crit;
    public LayerMask playerLayerMask;

    private float lastTimeAttack = 0f;
    private float lastTimeMove = 0f;
    private float moveSpeed = 10f;
    private Vector3 moveDir;
    private float lastTimeCrit = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.Find("Player");
        moveSpeed = GetComponent<CharacterInfo>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - lastTimeAttack >= 0.3f){
            transform.LookAt(player.transform);
            if (Physics.Raycast(transform.position, transform.forward, 5f, playerLayerMask)){
                GameObject obj = Instantiate(bullet, transform.position - new Vector3(0,-0.5f,0), transform.rotation);
                obj.GetComponent<BulletController>().initSpeed = GetComponent<CharacterInfo>().range;
                obj.GetComponent<BulletController>().ATK = GetComponent<CharacterInfo>().ATK;
                obj.GetComponent<BulletController>().parent = gameObject;
                obj.tag = "BulletEnemy";
            }
            lastTimeAttack = Time.realtimeSinceStartup;
        }
        if (Time.realtimeSinceStartup - lastTimeMove >= 1f){
            Move();
            lastTimeMove = Time.realtimeSinceStartup;
        }
        GetComponent<Rigidbody>().velocity = moveDir * moveSpeed;

        if (crit.active == true && Time.realtimeSinceStartup - lastTimeCrit >= 0.5f) crit.SetActive(false);
        crit.transform.LookAt(crit.transform.position + GameObject.Find("Main Camera").transform.forward);
    }

    void OnTriggerEnter(Collider collider){
        if (collider.transform.CompareTag("BulletPlayer")){
            GameObject attacker = collider.GetComponent<BulletController>().parent;
            float ATK = collider.GetComponent<BulletController>().ATK;
            if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.CRIT)){
                int r = Random.Range(0, 100);
                if (r < 25) {
                    ATK *= 2;
                    crit.SetActive(true);
                    lastTimeCrit = Time.realtimeSinceStartup;
                }
            }
            GetComponent<CharacterInfo>().health -=  ATK * GameConstant.INIT_ATK / (GameConstant.INIT_ATK + GetComponent<CharacterInfo>().DEF);;
            if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.BLOOD_SUCKING))
                attacker.GetComponent<CharacterInfo>().GainHealth(4);
            if (GetComponent<CharacterInfo>().health <= 0){
                if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.KILL_HP_UP))
                    attacker.GetComponent<CharacterInfo>().GainHealth(20);
                OnDie();
            }
            Destroy(collider.gameObject);
        }
        if (collider.CompareTag("Heart"))
            CollectItem(collider.tag, collider.gameObject);
    }

    void Move(){
        Vector3 curPos = transform.position;
        int dirX = Random.Range(-1, 2);
        int dirZ = Random.Range(-1, 2);
        while (Physics.Raycast(curPos, new Vector3(dirX, 0, dirZ), 0.7f, 3 /*Wall*/)){
            dirX = Random.Range(-1, 2);
            dirZ = Random.Range(-1, 2);
        }
        moveDir = new Vector3(dirX, 0, dirZ);
    }

    void CollectItem(string itemName, GameObject obj){
        switch(itemName){
            case "Heart":
                GameInformation.Instance.ItemCount--;
                GetComponent<CharacterInfo>().health = Mathf.Min(GetComponent<CharacterInfo>().maxHealth,  GetComponent<CharacterInfo>().health + obj.GetComponent<ItemInfo>().Health);
                Destroy(obj);
                break;
        }
    }

    void OnDie(){
        if (GetComponent<CharacterInfo>().health <= 0){
            player.GetComponent<PlayerController>().GainEXP(10 + (int)(5 * GetComponent<CharacterInfo>().level * Mathf.Pow(1.5f, player.GetComponent<CharacterInfo>().status.FindAll(x => x == STATUS.MORE_EXP).Count + 1))); 
            Destroy(gameObject);
            GameInformation.Instance.EnemyCount--;
        }
    }
}
