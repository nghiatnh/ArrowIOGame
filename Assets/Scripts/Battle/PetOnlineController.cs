using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PetOnlineController : MonoBehaviour
{
    public Transform parent;
    public GameObject bullet;
    public LayerMask playerLayerMask;
    private GameObject target;
    private float lastTimeAttack = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetPhotonView().IsMine) return;
        if (parent == null) {
            PhotonNetwork.Destroy(gameObject);
            return;
        }
        if (Vector3.Distance(transform.position, parent.position) > 4f)
            transform.position = new Vector3(parent.position.x - parent.transform.forward.x, transform.position.y, parent.position.z - parent.transform.forward.z) ;
        else if (Vector3.Distance(transform.position, parent.position) > 2f)
            GetComponent<Rigidbody>().velocity = new Vector3(parent.position.x - transform.position.x, 0, parent.position.z - transform.position.z) * 2f;
        else
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        CollideTarget();

        if (target == null)
            transform.LookAt(new Vector3(parent.position.x, transform.position.y, parent.position.z));
        else
            transform.LookAt(new Vector3(target.transform.position.x, transform.transform.position.y, target.transform.position.z));

        if (Time.realtimeSinceStartup - lastTimeAttack >= 0.5f && target != null){
            Attack();
            lastTimeAttack = Time.realtimeSinceStartup;
        }
    }

    void CollideTarget(){
        RaycastHit[] rh = Physics.BoxCastAll(transform.position, Vector3.one, transform.forward, Quaternion.Euler(0, 0, 0), 4f, playerLayerMask);
        foreach(var item in rh){
            if (item.transform != parent.transform){
                target = item.transform.gameObject;
                return;
            }
        }
        target = null;
    }

    void Attack(){
        GameObject obj = PhotonNetwork.Instantiate(bullet.name, transform.position - new Vector3(0,-0.5f,0), transform.rotation);
        BulletOnlineController controller = obj.GetComponent<BulletOnlineController>();
        controller.initSpeed = GetComponent<CharacterInfo>().range;
        controller.ATK = GetComponent<CharacterInfo>().ATK;
        controller.isThroughWall = parent.GetComponent<CharacterInfo>().status.Contains(STATUS.BULLET_THROUGH_WALL);
        controller.parent = gameObject;
    }

    public void SetParent(int parentID){
        gameObject.GetPhotonView().RPC("SetParentRPC", RpcTarget.All, parentID);
    }

    [PunRPC]
    private void SetParentRPC(int parentID){
        parent = PhotonNetwork.GetPhotonView(parentID).transform;
        GetComponent<CharacterInfo>().ATK = parent.GetComponent<CharacterInfo>().ATK / 9;
    }
}
