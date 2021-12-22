using System;
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
        CharacterInfo info = player.GetComponent<CharacterInfo>();
        switch (skill){
            case SKILLS.ALL_STATUS_UP: //Done
                info.DEF += 2;
                info.ATK += 3;
                info.speed += 1f;
                if (player.gameObject.layer == 6)
                    player.GetComponent<PlayerMovement>().UpdateSpeed();
                break;
            case SKILLS.ATK_UP: //Done
                info.ATK += 6;
                break;
            case SKILLS.ATK_UP_HP_DOWN: //Done
                info.ATK += 4;
                info.maxHealth -= 10;
                info.health = Mathf.Min(info.health, info.maxHealth);
                player.Find("Health").GetComponent<HealthController>().updateHealth();
                break;
            case SKILLS.BLOOD_SUCKING: //Done
                info.status.Add(STATUS.BLOOD_SUCKING);
                break;
            case SKILLS.BULLET_THROUGH_WALL: //Done
                info.status.Add(STATUS.BULLET_THROUGH_WALL);
                break;
            case SKILLS.BULLET_UP:
                info.status.Add(STATUS.BULLET_UP);
                break;
            case SKILLS.CRIT: //Done
                info.status.Add(STATUS.CRIT);
                break;
            case SKILLS.DEF_UP: //Done
                info.DEF += 4;
                break;
            case SKILLS.HP_UP: //Done
                info.maxHealth += 20;
                info.health += 20;
                player.Find("Health").GetComponent<HealthController>().updateHealth();
                break;
            case SKILLS.KILL_HP_UP: //Done
                info.status.Add(STATUS.KILL_HP_UP);
                break;
            case SKILLS.MAGNET: //Done
                BoxCollider collider = player.GetComponent<BoxCollider>();
                collider.size = new Vector3(collider.size.x * 2f, collider.size.y, collider.size.z * 2f);
                break;
            case SKILLS.MAKE_SPIKE:
                info.status.Add(STATUS.MAKE_SPIKE);
                break;
            case SKILLS.MORE_EXP: //Done
                info.status.Add(STATUS.MORE_EXP); 
                break;
            case SKILLS.RANGE_UP: //Done
                info.range += 5f; 
                break;
            case SKILLS.SPEED_UP: //Done
                info.speed += 3f;
                if (player.gameObject.layer == 6)
                    player.GetComponent<PlayerMovement>().UpdateSpeed();
                break;
            case SKILLS.THROUGH_WALL:
                info.status.Add(STATUS.THROUGH_WALL);
                break;
        }
    }
    #endregion
}
