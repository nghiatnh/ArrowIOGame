using UnityEngine;
using System.Collections.Generic;

public class CharacterInfo: MonoBehaviour
{
    public string characterName = "character";
    public int level = 1;
    public float health = 100f;
    public float maxHealth = 100f;
    public int ATK = 18;
    public int DEF = 7;
    public int EXP = 0;
    public int MAX_EXP = 100;
    public float speed = 5f;
    public float range = 15f; // init Speed
    public long powerPoint = 0;
    public List<STATUS> status = new List<STATUS>();
    public delegate void OnUpdateInfo(SKILLS skill);
    public OnUpdateInfo onUpdateInfo;
    public delegate void OnCreatePet();
    public OnCreatePet onCreatePet;

    public void GainHealth(float _health){
        health = Mathf.Min(maxHealth, health + _health);
    }

    public void ChangeInfo(SKILLS skill){
        if (onUpdateInfo != null)
            onUpdateInfo.Invoke(skill);
    }

    public void CreatePet(){
        if (onCreatePet != null)
            onCreatePet.Invoke();
    }
}