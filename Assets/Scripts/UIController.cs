using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Sprite[] Skills;
    public Transform pnSkill;
    public bool isRandom = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRandom)
            RandomSkill(isRandom);
    }

    void RandomSkill(bool isRandom){
        List<int> ListSkill = new List<int>();
        for (int i = 0; i < Enum.GetValues(typeof(SKILLS)).Length; i++){
            ListSkill.Add(i);
        }
        if (isRandom)
            for (int i = 0; i < pnSkill.childCount; i++){
                Transform skill = pnSkill.GetChild(i);
                int index = UnityEngine.Random.Range(0, ListSkill.Count);
                skill.GetComponent<Image>().sprite = Skills[ListSkill[index]];
                ListSkill.RemoveAt(index);
            }
    }
}
