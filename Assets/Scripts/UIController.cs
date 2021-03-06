using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Transform Inventory;
    public SkilInfo[] Skills;
    public Transform pnTop;
    public GameObject txtPlayerTop;
    public Transform pnSkill;
    public Transform player;
    public Transform txtDescription;
    private int queueSkillCount = 0;
    private bool isShowSkill = false;
    private float lastUpdateTop = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        if (queueSkillCount > 0 && isShowSkill == false){
            queueSkillCount--;
            isShowSkill = true;
            SkillController.Instance.StartRandom();
            pnSkill.gameObject.SetActive(true);
        }
        if (SkillController.Instance.isRandom)
        {    
            SkillController.Instance.RandomSkill(Skills);
            for (int i = 0; i < pnSkill.childCount; i++){
                pnSkill.GetChild(i).GetComponent<Image>().sprite = SkillController.Instance.currentSkills[i].SkillImage;
            }
        }

        if (SkillController.Instance.isDescribe){
            SkillController.Instance.DescribeSkill();
        }
        else {
            txtDescription.gameObject.SetActive(false);
        }
        if (Time.realtimeSinceStartup - lastUpdateTop >= 1f){
            //UpdateTop();
            lastUpdateTop = Time.realtimeSinceStartup;
        }
    }

    public void AddQueueSkill(){
        queueSkillCount++;
    }

    public void ChooseSkill(int index){
        if (SkillController.Instance.isRandom) return;
        isShowSkill = false;
        pnSkill.gameObject.SetActive(false);
        SkilInfo selected = SkillController.Instance.currentSkills[index];
        SkillController.ChooseSkill(selected.SkillTag, player);
        txtDescription.gameObject.SetActive(true);
        txtDescription.GetComponent<Text>().text = selected.Description;
        SkillController.Instance.StartDescribe();
        GameInformation.Instance.Skills.Add(selected.SkillTag);
        Inventory.GetComponent<InventoryController>().UpdateInventory(selected);
    }

    public void UpdateTop(){
        GameInformation.Instance.characters.RemoveAll(x => x == null);
        if (GameInformation.Instance.characters.Count < pnTop.childCount) return;
        GameInformation.Instance.characters.Sort(GameConstant.CompareCharacterPower);
        for (int i = 0; i < pnTop.childCount - 1; i++){
            pnTop.GetChild(i).GetComponent<Text>().text = "#" + (i+1).ToString() + " " + GameInformation.Instance.characters[i].characterName + "(" + GameInformation.Instance.characters[i].powerPoint.ToString() +")";
            if (GameInformation.Instance.characters[i].gameObject == player.gameObject)
                pnTop.GetChild(i).GetComponent<Text>().color = Color.blue;
            else
                pnTop.GetChild(i).GetComponent<Text>().color = Color.black;
            }
        int playerIndex = GameInformation.Instance.characters.IndexOf(player.GetComponent<CharacterInfo>());
        txtPlayerTop.GetComponent<Text>().text = "#" + (playerIndex+1).ToString() + " " + player.GetComponent<CharacterInfo>().characterName + "(" + player.GetComponent<CharacterInfo>().powerPoint.ToString() + ")";
    }
}
