using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Sprite[] Skills;
    public Transform pnSkill;
    public Transform player;
    public Transform txtDescription;
    public GameObject pnRestart;
    private int queueSkillCount = 0;
    private bool isShowSkill = false;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (queueSkillCount > 0 && isShowSkill == false){
            queueSkillCount--;
            isShowSkill = true;
            SkillController.Instance.StartRandom();
            pnSkill.gameObject.active = true;
        }
        if (SkillController.Instance.isRandom)
        {    
            SkillController.Instance.RandomSkill();
            for (int i = 0; i < pnSkill.childCount; i++){
                pnSkill.GetChild(i).GetComponent<Image>().sprite = Skills[SkillController.Instance.currentSkills[i]];
            }
        }

        if (SkillController.Instance.isDescribe){
            SkillController.Instance.DescribeSkill();
        }
        else{
            txtDescription.gameObject.active = false;
        }

        if (player.eulerAngles.x == 270)
            pnRestart.active = true;
    }

    public void AddQueueSkill(){
        queueSkillCount++;
    }

    public void ChooseSkill(int index){
        if (SkillController.Instance.isRandom) return;
        isShowSkill = false;
        pnSkill.gameObject.active = false;
        SKILLS selected = (SKILLS)GameConstant.LIST_SKILL.GetValue(SkillController.Instance.currentSkills[index]);
        SkillController.ChooseSkill(selected, player);
        txtDescription.gameObject.active = true;
        txtDescription.GetComponent<Text>().text = GameConstant.SKILL_DESCRIPTIONS[selected];
        SkillController.Instance.StartDescribe();
    }

    public void Restart(){
        pnRestart.active = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
