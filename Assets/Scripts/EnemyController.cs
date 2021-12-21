using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;
    public LayerMask playerLayerMask;

    private float lastTimeAttack = 0f;
    private float lastTimeMove = 0f;
    private float moveSpeed = 10f;
    private Vector3 moveDir;
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
                obj.tag = "BulletEnemy";
            }
            lastTimeAttack = Time.realtimeSinceStartup;
        }
        if (Time.realtimeSinceStartup - lastTimeMove >= 1f){
            Move();
            lastTimeMove = Time.realtimeSinceStartup;
        }
        GetComponent<Rigidbody>().velocity = moveDir * moveSpeed;
        OnDie();
    }

    void OnTriggerEnter(Collider collider){
        if (collider.transform.CompareTag("BulletPlayer")){
            GetComponent<CharacterInfo>().health -= collider.GetComponent<BulletController>().ATK * GameInformation.INIT_ATK / (GameInformation.INIT_ATK + GetComponent<CharacterInfo>().DEF);;
            player.GetComponent<PlayerController>().GainEXP(10 + 5 * GetComponent<CharacterInfo>().level); 
            Destroy(collider.gameObject);
        }
        else if (collider.CompareTag("Heart"))
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
        if (GetComponent<CharacterInfo>().health <= 0)
            Destroy(gameObject); 
    }
}
