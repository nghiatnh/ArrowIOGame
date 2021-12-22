using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet;
    
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CharacterInfo>().health <= 0) return;
        if (Input.GetMouseButtonUp(0)){
            GameObject obj = Instantiate(bullet, transform.position - new Vector3(0,-0.5f,0), transform.rotation);
            obj.tag = "BulletPlayer";
            BulletController controller = obj.GetComponent<BulletController>();
            controller.initSpeed = GetComponent<CharacterInfo>().range;
            controller.ATK = GetComponent<CharacterInfo>().ATK;
            controller.parent = gameObject;
        }
    }

    void OnTriggerEnter(Collider collider){
        if (collider.transform.CompareTag("BulletEnemy")){
            GetDamage(collider.GetComponent<BulletController>().ATK);
            GameObject attacker = collider.GetComponent<BulletController>().parent;
            if (attacker != null){
            if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.BLOOD_SUCKING))
                attacker.GetComponent<CharacterInfo>().GainHealth(4);
            if (GetComponent<CharacterInfo>().health <= 0){
                if (attacker.GetComponent<CharacterInfo>().status.Contains(STATUS.KILL_HP_UP))
                    attacker.GetComponent<CharacterInfo>().GainHealth(20);
            }
            }
            Destroy(collider.gameObject);
        }
    }

    public void GetDamage(float ATK){
        GetComponent<CharacterInfo>().health = Mathf.Max(0, GetComponent<CharacterInfo>().health - ATK * GameConstant.INIT_ATK / (GameConstant.INIT_ATK + GetComponent<CharacterInfo>().DEF));
        if (GetComponent<CharacterInfo>().health <= 0){
            GetComponent<Rigidbody>().isKinematic = true;
            transform.Find("Health").gameObject.active = false;
        }
    }
}
