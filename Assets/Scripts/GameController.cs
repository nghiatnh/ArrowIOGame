using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Heart;
    public GameObject Diamond;
    public GameObject Enemy;
    public Transform Items;
    public Transform Enemies;

    private float lastGenerate = 0f;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GenerateItem();
        GenerateEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - lastGenerate >= 10f){
            GenerateItem();
            GenerateEnemy();
            lastGenerate = Time.realtimeSinceStartup;
        }
    }

    public void GenerateItem(){
        while (GameInformation.Instance.ItemCount < 300){
            int itemType = Random.Range(0,101);
            float angle = Random.Range(0, 10) * 36f;
            Vector3 pos = GenerateAt(new int[4]{-300, 300, -300, 300}, 1f);
            if (itemType < 15){
                GameObject obj = GameObject.Instantiate(Heart, new Vector3(pos.x, 0.1f, pos.z), new Quaternion(0, angle, 0, 1), Items);
            }
            else{
                GameObject obj = GameObject.Instantiate(Diamond, new Vector3(pos.x, 0.1f, pos.z), new Quaternion(0, angle, 0, 1), Items);
            }
            GameInformation.Instance.ItemCount++;
        }
    }

    public void GenerateEnemy(){
        while (GameInformation.Instance.EnemyCount < 15){
            Vector3 pos = GenerateAt(new int[4]{-300, 300, -300, 300}, 0.7f);
            GameObject.Instantiate(Enemy, new Vector3(pos.x, 0.1f, pos.z), new Quaternion(0, 0, 0, 1), Enemies);
            GameInformation.Instance.EnemyCount++;
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
