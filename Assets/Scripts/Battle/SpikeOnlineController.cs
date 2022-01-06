using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpikeOnlineController : MonoBehaviour
{
    public GameObject parent;
    private float startTime = 0f;
    private bool IsMine = false;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.realtimeSinceStartup;
        IsMine = gameObject.GetPhotonView().IsMine;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMine && Time.realtimeSinceStartup - startTime >= 1.5f)
            PhotonNetwork.Destroy(gameObject);
    }

    public void SetParent(int parentID){
        gameObject.GetPhotonView().RPC("SetParentRPC", RpcTarget.All, parentID);
    }

    [PunRPC]
    private void SetParentRPC(int parentID){
        parent = PhotonNetwork.GetPhotonView(parentID).gameObject;
    }
}
