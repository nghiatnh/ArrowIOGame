using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController
{
    private static SkillController instance;
    public static SkillController Instance{
        get {
            if (instance == null) instance = new SkillController();
            return instance;
        }
    }

    #region public Properties
    public int[] currentSkills = new int[3];
    public bool isRandom = false;
    public bool isDescribe = false;
    #endregion

    #region private Properties
    private float startRandomTime = 0f;
    private float startDescribeTime = 0f;
    #endregion

    #region Methods 
    public void StartRandom(){
        isRandom = true;
        startRandomTime = Time.realtimeSinceStartup;
    }

    public void StartDescribe(){
        isDescribe = true;
        startDescribeTime = Time.realtimeSinceStartup;
    }

    public void DescribeSkill(){
        if (Time.realtimeSinceStartup - startDescribeTime >= 1.5f){
            isDescribe = false;
        }
    }

    public void RandomSkill(){
        if (Time.realtimeSinceStartup - startRandomTime >= 1.5f) {
            isRandom = false;
            return;
        }
        List<int> ListSkill = new List<int>();
        for (int i = 0; i < Enum.GetValues(typeof(SKILLS)).Length; i++){
            ListSkill.Add(i);
        }
        for (int i = 0; i < 3; i++){
            int index = UnityEngine.Random.Range(0, ListSkill.Count);
            currentSkills[i] = ListSkill[index];
            ListSkill.RemoveAt(index);
        }
    }

    public static void ChooseSkill(SKILLS skill, Transform player){
        switch (skill){
            case SKILLS.ALL_STATUS_UP:
                break;
            case SKILLS.HP_UP:
                player.GetComponent<CharacterInfo>().maxHealth += 15;
                player.Find("Health").GetComponent<HealthController>().updateHealth();
                break;
        }
    }
    #endregion

    #region Constants
    public readonly Array LIST_SKILL = Enum.GetValues(typeof(SKILLS));
    public readonly Dictionary<SKILLS, string> SKILL_DESCRIPTIONS = new Dictionary<SKILLS, string>(){
        {SKILLS.MAGNET, "You will get more diamond around you!"},
        {SKILLS.RANGE_UP, "You will get more diamond around you!"},
        {SKILLS.CRIT, "You will get more diamond around you!"},
        {SKILLS.BLOOD_SUCKING, "You will get more diamond around you!"},
        {SKILLS.ALL_STATUS_UP, "You will get more diamond around you!"},
        {SKILLS.THROUGH_WALL, "You will get more diamond around you!"},
        {SKILLS.MORE_EXP, "You will get more diamond around you!"},
        {SKILLS.ATK_UP, "You will get more diamond around you!"},
        {SKILLS.ATK_UP_HP_DOWN, "You will get more diamond around you!"},
        {SKILLS.DEF_UP, ""},
        {SKILLS.HP_UP, ""},
        {SKILLS.SPEED_UP, ""},
        {SKILLS.BULLET_UP, ""},
        {SKILLS.KILL_HP_UP, ""},
        {SKILLS.BULLET_THROUGH_WALL, ""},
        {SKILLS.MAKE_SPIKE, ""},
    };
    #endregion
}

public enum SKILLS{
    MAGNET,
    RANGE_UP,
    CRIT,
    BLOOD_SUCKING,
    ALL_STATUS_UP,
    THROUGH_WALL,
    MORE_EXP,
    ATK_UP,
    ATK_UP_HP_DOWN,
    DEF_UP,
    HP_UP,
    SPEED_UP,
    BULLET_UP,
    KILL_HP_UP,
    BULLET_THROUGH_WALL,
    MAKE_SPIKE,
}
