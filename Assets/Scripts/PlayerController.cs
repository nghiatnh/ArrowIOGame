using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterInfo info;
    public Transform experienceBar;
    public Text txtLevel;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<CharacterInfo>();
        info.speed = GetComponent<PlayerMovement>().moveSpeed;
        txtLevel.text = "LV " + info.level;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CharacterInfo>().health <= 0) Die();
    }

    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("Heart") || collider.CompareTag("Diamond"))
            CollectItem(collider.tag, collider.gameObject);
    }

    void CollectItem(string itemName, GameObject obj){
        switch(itemName){
            case "Heart":
                GameInformation.Instance.ItemCount--;
                info.health = Mathf.Min(info.maxHealth, info.health + obj.GetComponent<ItemInfo>().Health);
                Destroy(obj);
                break;
            case "Diamond":
                GainEXP(obj.GetComponent<ItemInfo>().EXP);
                Destroy(obj);
                GameInformation.Instance.ItemCount--;
                RectTransform rect = experienceBar.Find("ProgressBar").GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(experienceBar.Find("Background").GetComponent<RectTransform>().sizeDelta.x * info.EXP / info.MAX_EXP, rect.sizeDelta.y);
                break;
        }
    }

    public void GainEXP(int EXP){
        info.EXP += EXP;
        if (info.EXP >= info.MAX_EXP){
            info.level++;
            info.EXP -= info.MAX_EXP;            
            txtLevel.text = "LV " + info.level;
            GameObject.Find("GameController").GetComponent<UIController>().AddQueueSkill();
        }
    }
    public void Die(){
        if (transform.eulerAngles.x > 270 || transform.eulerAngles.x == 0f)
            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x - 5, transform.eulerAngles.y, 0));
    }
}
