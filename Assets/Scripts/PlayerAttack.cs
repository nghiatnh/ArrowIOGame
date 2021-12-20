using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)){
            GameObject obj = Instantiate(bullet, transform.position - new Vector3(0,-0.5f,0), transform.rotation);
            obj.GetComponent<BulletController>().initSpeed = GetComponent<CharacterInfo>().range;
            obj.GetComponent<BulletController>().ATK = GetComponent<CharacterInfo>().ATK;
            obj.tag = "BulletPlayer";
        }
    }

    void OnTriggerEnter(Collider collider){
        if (collider.transform.CompareTag("BulletEnemy")){
            GetComponent<CharacterInfo>().health -= collider.GetComponent<BulletController>().ATK * GameInformation.INIT_ATK / (GameInformation.INIT_ATK + GetComponent<CharacterInfo>().DEF);
            Destroy(collider.gameObject);
        }
    }
}
