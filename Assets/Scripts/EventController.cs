using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
    void EndCrit(){
        GetComponent<Animator>().ResetTrigger("Crit");
    }

    void EndLevelUp(){
        GetComponent<Animator>().ResetTrigger("LevelUp");
    }

    TouchScreenKeyboard tc;
    public void StartOffline(){
        if (GameInformation.Instance.PlayerName == string.Empty) return;
        GameInformation.Instance.gameMode = GAME_MODE.OFFLINE;
        SceneManager.LoadScene("BattleScene");
    }

    public void StartOnline(){
        if (GameInformation.Instance.PlayerName == string.Empty) return;
        SceneManager.LoadScene("Loading");
    }

    public void Restart(GameObject pnRestart){
        pnRestart.SetActive(false);
        GameInformation.Instance.Reload();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPlayerNameChange(InputField text){
        GameInformation.Instance.PlayerName = text.text;
    }

    public void BackToStart(){
        SceneManager.LoadScene("StartScene");
    }

    public void KillPlayer(CharacterController info){
        //info.GetDamage(10000, info.gameObject);
    }

    public void UpdateText(InputField text){
        text.text = GameInformation.Instance.PlayerName;
    }

    public void ShowInventory(Transform pnInventory){
        pnInventory.GetComponent<Animator>().SetBool("IsShow", !pnInventory.GetComponent<Animator>().GetBool("IsShow"));
    }
}
