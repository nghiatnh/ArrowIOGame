using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public void UpdateInventory(SkilInfo skill){
        int count = GameInformation.Instance.Skills.FindAll(x => x == skill.SkillTag).Count;
        for (int i = 0; i < transform.childCount; i++){
            if (transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == null){
                transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = skill.SkillImage;
                transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = count.ToString();
                break;
            }
            else if (transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == skill.SkillImage){
                transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = count.ToString();
                break;
            }
        }
    }
}
