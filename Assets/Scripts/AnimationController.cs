using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimationController : MonoBehaviour
{
    void EndCrit(){
        GetComponent<Animator>().ResetTrigger("Crit");
    }

    void EndLevelUp(){
        GetComponent<Animator>().ResetTrigger("LevelUp");
    }

    TouchScreenKeyboard tc;
    public void StartGame(InputField text){
        if (text.text == string.Empty) return;
        SceneManager.LoadScene("MainScene");
    }

    public void OnPlayerNameChange(InputField text){
        GameInformation.Instance.PlayerName = text.text;
    }

    public void BackToStart(){
        SceneManager.LoadScene("StartScene");
    }

    public void KillPlayer(CharacterController info){
        info.GetDamage(10000, info.gameObject);
    }

    public void UpdateText(InputField text){
        text.text = GameInformation.Instance.PlayerName;
    }

    public void ShowInventory(Transform pnInventory){
        pnInventory.GetComponent<Animator>().SetBool("IsShow", !pnInventory.GetComponent<Animator>().GetBool("IsShow"));
    }
}
