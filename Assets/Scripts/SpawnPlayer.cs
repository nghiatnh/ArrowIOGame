using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start(){
        Vector3 pos = new Vector3(Random.Range(-5,5), 1, Random.Range(-5,5));
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
    }
}
