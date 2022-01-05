using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ItemOnlineController : MonoBehaviour
{
    private Transform Items;
    // Start is called before the first frame update
    void Start()
    {
        if (Items == null) Items = GameObject.Find("Items").transform;
        transform.SetParent(Items);
    }

    public void Collected(){
        gameObject.GetPhotonView().RPC("CollectItem", RpcTarget.All);
    }

    [PunRPC]
    private void CollectItem(){
        Destroy(gameObject);
    }
}
