using UnityEngine;
using System.Collections.Generic;

public class CharacterInfo: MonoBehaviour
{
    public int level = 1;
    public float health = 100f;
    public float maxHealth = 100f;
    public int ATK = 18;
    public int DEF = 7;
    public int EXP = 0;
    public int MAX_EXP = 100;
    public float speed = 5f;
    public float range = 15f; // init Speed
    public bool isCrit = false;
    public List<GameObject> pets;
    public GameObject weapon;

    public void AddPet(GameObject pet){

    }
}
