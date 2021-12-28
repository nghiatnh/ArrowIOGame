using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Sprite[] Skills;
    public Transform pnTop;
    public GameObject txtPlayerTop;
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
            pnSkill.gameObject.SetActive(true);
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
        else {
            txtDescription.gameObject.SetActive(false);
        }
        UpdateTop();
    }

    public void AddQueueSkill(){
        queueSkillCount++;
    }

    public void ChooseSkill(int index){
        if (SkillController.Instance.isRandom) return;
        isShowSkill = false;
        pnSkill.gameObject.SetActive(false);
        SKILLS selected = (SKILLS)GameConstant.LIST_SKILL.GetValue(SkillController.Instance.currentSkills[index]);
        SkillController.ChooseSkill(selected, player);
        txtDescription.gameObject.SetActive(true);
        txtDescription.GetComponent<Text>().text = GameConstant.SKILL_DESCRIPTIONS[selected];
        SkillController.Instance.StartDescribe();
    }

    public void Restart(){
        pnRestart.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameInformation.Instance.Reload();
    }

    public void UpdateTop(){
        GameInformation.Instance.characters.Sort(GameConstant.CompareCharacterPower);
        Debug.Log(GameInformation.Instance.characters.Count);
        try{
            for (int i = 0; i < pnTop.childCount; i++){
                pnTop.GetChild(i).GetComponent<Text>().text = "#" + (i+1).ToString() + " " + GameInformation.Instance.characters[i].characterName + "(" + GameInformation.Instance.characters[i].powerPoint.ToString() +")";
                if (GameInformation.Instance.characters[i].gameObject == player.gameObject)
                    pnTop.GetChild(i).GetComponent<Text>().color = Color.blue;
                else
                    pnTop.GetChild(i).GetComponent<Text>().color = Color.black;
            }
            int playerIndex = GameInformation.Instance.characters.IndexOf(player.GetComponent<CharacterInfo>());
            txtPlayerTop.GetComponent<Text>().text = "#" + (playerIndex+1).ToString() + " " + player.GetComponent<CharacterInfo>().characterName + "(" + player.GetComponent<CharacterInfo>().powerPoint.ToString() + ")";
        }
        catch {}
    }
}
