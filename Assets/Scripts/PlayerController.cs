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
    public Transform Skin;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<CharacterInfo>();
        txtLevel.text = "LV " + info.level;
        info.MAX_EXP = GameConstant.MAX_EXP_LEVEL[info.level];
        info.characterName = GameInformation.Instance.PlayerName;
        if (Skin == null)
            Skin = transform.Find("Skin");
        ChangeSkin();
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
        if (info.EXP >= info.MAX_EXP && info.level < GameConstant.MAX_EXP_LEVEL.Length - 1){
            info.level++;
            txtLevel.text = "LV " + info.level;
            GameObject.Find("Canvas").GetComponent<UIController>().AddQueueSkill();
            info.MAX_EXP = GameConstant.MAX_EXP_LEVEL[info.level];
            int exp = info.EXP;
            info.EXP = 0;
            info.speed = Mathf.Max(2.5f, info.speed - 0.1f);
            levelUp.GetComponent<Animator>().SetTrigger("LevelUp");
            levelUpSource.Play();

            GainEXP(exp - GameConstant.MAX_EXP_LEVEL[info.level - 1]);     
        }
        else if (info.level >= GameConstant.MAX_EXP_LEVEL.Length - 1){
            txtLevel.text = "LV Max";
            rect.sizeDelta = new Vector2(experienceBar.Find("Background").GetComponent<RectTransform>().sizeDelta.x, rect.sizeDelta.y);
        }
    }

    public void ChangeSkin(){
        if (GameInformation.Instance.PlayerSkin == null) return;
        Destroy(Skin.GetChild(0).gameObject);
        GameObject skin = Instantiate(GameInformation.Instance.PlayerSkin, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), Skin);
        skin.transform.localPosition = new Vector3(0, 0, 0);
    }
}
