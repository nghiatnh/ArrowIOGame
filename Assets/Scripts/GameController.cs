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
        GameInformation.Instance.Reload();
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

    public void GenerateEnemy(){
        GameObject player = GameObject.Find("Player");
        GameInformation.Instance.EnemyCount = Enemies.childCount;
        List<string> names = new List<string>();
        for (int i = 0; i < GameConstant.ENEMY_NAME.Length; i++)
            names.Add(GameConstant.ENEMY_NAME[i]);
        while (GameInformation.Instance.EnemyCount < 15){
            Vector3 pos = GenerateAt(new int[4]{-300, 300, -300, 300}, 0.7f);
            GameObject enemy = GameObject.Instantiate(Enemy, new Vector3(pos.x, 0.1f, pos.z), new Quaternion(0, 0, 0, 1), Enemies);
            enemy.GetComponent<CharacterInfo>().level = player.GetComponent<CharacterInfo>().level;
            enemy.GetComponent<CharacterInfo>().speed = Mathf.Max(2.5f, enemy.GetComponent<CharacterInfo>().speed - enemy.GetComponent<CharacterInfo>().level * 0.1f); 
            int indexName = Random.Range(0, names.Count);
            string name = names[indexName];
            names.RemoveAt(indexName);
            enemy.GetComponent<CharacterInfo>().characterName = name;
            enemy.GetComponent<CharacterInfo>().powerPoint = player.GetComponent<CharacterInfo>().powerPoint + Random.Range(10, 20) * enemy.GetComponent<CharacterInfo>().level;
            enemy.transform.Find("Health").Find("Name").GetComponent<TextMesh>().text = name;
            for (int i = 1; i < enemy.GetComponent<CharacterInfo>().level; i++){
                int skill = Random.Range(0, GameConstant.LIST_SKILL.Length);
                SkillController.ChooseSkill((SKILLS)GameConstant.LIST_SKILL.GetValue(skill), enemy.transform);
            }
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
