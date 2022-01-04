using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;
    public LayerMask playerLayerMask;
    public LayerMask wallLayerMask;

    private float lastTimeAttack = 0f;
    private float lastTimeMove = 0f;
    private float moveSpeed = 10f;
    private GameObject target;
    private Vector3 moveDir;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        moveSpeed = GetComponent<CharacterInfo>().speed;
        GetComponent<CharacterOfflineController>().afterDie = new CharacterOfflineController.AfterDie(AfterDie);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CharacterInfo>().health <= 0) return;
        CollideTarget();
        if (Time.realtimeSinceStartup - lastTimeAttack >= 0.5f){
            if (target != null){
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                GetComponent<CharacterOfflineController>().Attack(bullet, transform.position, transform.rotation);
            }
            else
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            
            lastTimeAttack = Time.realtimeSinceStartup;
        }
        if (Time.realtimeSinceStartup - lastTimeMove >= 1f){
            Move();
            lastTimeMove = Time.realtimeSinceStartup;
        }
        GetComponent<Rigidbody>().velocity = moveDir * moveSpeed;
    }

    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("Heart"))
            CollectItem(collider.tag, collider.gameObject);
    }

    void Move(){
        Vector3 curPos = transform.position;
        int dirX = Random.Range(-1, 2);
        int dirZ = Random.Range(-1, 2);
        while (!GetComponent<CharacterInfo>().status.Contains(STATUS.THROUGH_WALL) && Physics.Raycast(curPos, new Vector3(dirX, 0, dirZ), 0.7f, wallLayerMask)){
            dirX = Random.Range(-1, 2);
            dirZ = Random.Range(-1, 2);
        }
        moveDir = new Vector3(dirX, GetComponent<Rigidbody>().velocity.y / moveSpeed, dirZ);
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

    private void CollideTarget(){
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, Vector3.one, transform.forward, out hit, Quaternion.Euler(0, 0, 0), 4f, playerLayerMask))
            target = hit.transform.gameObject;
        else
            target = null;
    }

    void AfterDie(GameObject killer){
        if (killer.CompareTag("Player")){
            killer.GetComponent<PlayerOfflineController>().GainEXP((int)((5 + 10 * GetComponent<CharacterInfo>().level) * Mathf.Pow(1.5f, player.GetComponent<CharacterInfo>().status.FindAll(x => x == STATUS.MORE_EXP).Count + 1))); 
        }
        Destroy(gameObject);
        GameInformation.Instance.EnemyCount--;
    }
}
