using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineUIController : MonoBehaviour
{
    public GameObject moveJoystick;
    public GameObject rotateJoystick;
    
    [SerializeField] private GameObject btnExit;
    [SerializeField] private GameObject pnRestart;
    [SerializeField] private Transform experienceBar;
    [SerializeField] private Text txtLevel;

    private void Start(){
        pnRestart = GameObject.Find("pnRestart");
        moveJoystick = GameObject.Find("MoveJoystick");
        rotateJoystick = GameObject.Find("RotateJoystick");
        experienceBar = GameObject.Find("ExperienceBar").transform;
        txtLevel = GameObject.Find("txtLevel").GetComponent<Text>();
        btnExit = GameObject.Find("btnExit");
        btnExit.GetComponent<Button>().onClick.AddListener(KillPlayer);
        pnRestart.SetActive(false);
    }

    public void UpdateExperience(float averageHP, string level){
        RectTransform rect = experienceBar.Find("ProgressBar").GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(experienceBar.Find("Background").GetComponent<RectTransform>().sizeDelta.x * averageHP, rect.sizeDelta.y);
        txtLevel.text = level;
    }

    public void ShowLevelUp(GameObject levelUp){
        levelUp.GetComponent<Animator>().SetTrigger("LevelUp");
    }

    public void ShowPanelRestart(){
        pnRestart.SetActive(true);
    }

    public void KillPlayer(){
        GetComponent<OfflineController>().Player.GetComponent<CharacterOfflineController>().GetDamage(1000, GetComponent<OfflineController>().Player);
    }
}
