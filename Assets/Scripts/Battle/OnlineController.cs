using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineController : MonoBehaviour
{
    public GameObject Heart;
    public GameObject Diamond;
    public GameObject Player;
    public Transform Items;

    private float lastGenerate = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (Items == null) Items = GameObject.Find("Items").transform;
        GameInformation.Instance.Reload();
        Player = PhotonNetwork.Instantiate(Player.name, new Vector3(0, 1, 0), Quaternion.identity);
        //GetComponent<PhotonView>().RPC("GenerateItem", RpcTarget.All);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - lastGenerate >= 10f){
            //GetComponent<PhotonView>().RPC("GenerateItem", RpcTarget.All);
            lastGenerate = Time.realtimeSinceStartup;
        }
    }

    [PunRPC]
    public void GenerateItem(){
        GameInformation.Instance.ItemCount = Items.childCount;
        while (GameInformation.Instance.ItemCount < 300){
            int itemType = Random.Range(0,101);
            float angle = Random.Range(0, 10) * 36f;
            Vector3 pos = GenerateAt(new int[4]{-300, 300, -300, 300}, 1f);
            if (itemType < 15){
                // Heart
                GameObject obj = GameObject.Instantiate(Heart, new Vector3(pos.x, 0.1f, pos.z), new Quaternion(0, angle, 0, 1), Items);
                obj.GetComponent<ItemInfo>().Health = (int)(obj.GetComponent<ItemInfo>().Health * Random.Range(1f, 1.5f));
            }
            else{
                // Diamond
                GameObject obj = GameObject.Instantiate(Diamond, new Vector3(pos.x, 0.1f, pos.z), new Quaternion(0, angle, 0, 1), Items);
                obj.GetComponent<ItemInfo>().EXP = (int)(obj.GetComponent<ItemInfo>().EXP * Random.Range(1f, 2.5f));
            }
            GameInformation.Instance.ItemCount++;
        }
    }

    private Vector3 GenerateAt(int[] positions, float distance){
            float x = (float)Random.Range(positions[0], positions[1]) / 10;
            float z = (float)Random.Range(positions[2], positions[3]) / 10;
            while (Physics.Raycast(new Vector3(x, 0.1f, z), Vector3.left, distance) || Physics.Raycast(new Vector3(x, 0.1f, z), Vector3.right, distance) || 
            Physics.Raycast(new Vector3(x, 0.1f, z), Vector3.forward, distance) || Physics.Raycast(new Vector3(x, 0.1f, z), Vector3.back, distance)){
                x = (float)Random.Range(positions[0], positions[1]) / 10;
                z = (float)Random.Range(positions[2], positions[3]) / 10;
            }
            return new Vector3(x, 0, z);
    }
}
