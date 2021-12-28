using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioSource collectItemSource;
    public AudioSource levelUpSource;
    public CharacterInfo info;
    public Transform experienceBar;
    public Text txtLevel;
    public GameObject levelUp;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<CharacterInfo>();
        txtLevel.text = "LV " + info.level;
        info.MAX_EXP = GameConstant.MAX_EXP_LEVEL[info.level];
        info.characterName = GameInformation.Instance.PlayerName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("Heart") || collider.CompareTag("Diamond"))
            CollectItem(collider.tag, collider.gameObject);
    }

    void CollectItem(string itemName, GameObject obj){
        switch(itemName){
            case "Heart":
                collectItemSource.Play();
                GameInformation.Instance.ItemCount--;
                info.GainHealth(obj.GetComponent<ItemInfo>().Health);
                Destroy(obj);
                break;
            case "Diamond":
                collectItemSource.Play();
                GainEXP(obj.GetComponent<ItemInfo>().EXP);
                Destroy(obj);
                GameInformation.Instance.ItemCount--;
                break;
        }
    }

    public void GainEXP(int EXP){
        info.EXP += EXP;
        RectTransform rect = experienceBar.Find("ProgressBar").GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(experienceBar.Find("Background").GetComponent<RectTransform>().sizeDelta.x * info.EXP / info.MAX_EXP, rect.sizeDelta.y);
        if (info.EXP >= info.MAX_EXP){
            info.level++;
            txtLevel.text = "LV " + info.level;
            GameObject.Find("GameController").GetComponent<UIController>().AddQueueSkill();
            info.MAX_EXP = GameConstant.MAX_EXP_LEVEL[info.level]; 
            int exp = info.EXP;
            info.EXP = 0;
            levelUp.GetComponent<Animator>().SetTrigger("LevelUp");
            levelUpSource.Play();
            GainEXP(exp - GameConstant.MAX_EXP_LEVEL[info.level - 1]);     
        }
    }
}
