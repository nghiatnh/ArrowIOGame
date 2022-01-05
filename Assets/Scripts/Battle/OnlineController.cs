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
        Player = PhotonNetwork.Instantiate(Player.name, new Vector3(Random.Range(-5,5), 1, Random.Range(-5,5)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - lastGenerate >= 10f){
            GenerateItem();
            lastGenerate = Time.realtimeSinceStartup;
        }
    }

    void OnDestroy(){
        PhotonNetwork.Disconnect();
    }
    
    public void GenerateItem(){
        GameInformation.Instance.ItemCount = Items.childCount;
        while (GameInformation.Instance.ItemCount < 150){
            int itemType = Random.Range(0,101);
            float angle = Random.Range(0, 10) * 36f;
            Vector3 pos = GenerateAt(new int[4]{-300, 300, -300, 300}, 1f);
            try{
            if (itemType < 15){
                // Heart
                GameObject obj = PhotonNetwork.InstantiateRoomObject(Heart.name, new Vector3(pos.x, 0.1f, pos.z), new Quaternion(0, angle, 0, 1));
                obj.GetComponent<ItemInfo>().Health = (int)(obj.GetComponent<ItemInfo>().Health * Random.Range(1f, 1.5f));
            }
            else{
                // Diamond
                GameObject obj = PhotonNetwork.InstantiateRoomObject(Diamond.name, new Vector3(pos.x, 0.1f, pos.z), new Quaternion(0, angle, 0, 1));
                obj.GetComponent<ItemInfo>().EXP = (int)(obj.GetComponent<ItemInfo>().EXP * Random.Range(1f, 2.5f));
            }
            }
            catch {

            }
            GameInformation.Instance.ItemCount++;
        }
    }

    private Vector3 GenerateAt(int[] positions, float distance){
            float x = (float)Random.Range(positions[0], positions[1]) / 10;
            float z = (float)Random.Range(positions[2], positions[3]) / 10;
            while (Physics.BoxCast(new Vector3(x, 0.1f, z), Vector3.one, Vector3.up, Quaternion.identity, distance)){
                x = (float)Random.Range(positions[0], positions[1]) / 10;
                z = (float)Random.Range(positions[2], positions[3]) / 10;
            }
            return new Vector3(x, 0, z);
    }
}
