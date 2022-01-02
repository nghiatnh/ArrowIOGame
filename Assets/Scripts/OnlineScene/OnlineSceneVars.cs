using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineSceneVars : MonoBehaviour
{
    public Text[] txtNames;
    public Dictionary<string, KeyValuePair<string, string>> playerSkinNName = new Dictionary<string, KeyValuePair<string, string>>();
    public Transform pnWait;
    public OnlineSceneController controller;
    public Transform player;

    public void SetTxtName(int index, string playerName){
        txtNames[index].text = playerName;
    }

    public void SetPnWait(bool active){
        pnWait.gameObject.SetActive(active);
    }

    public void StartController(){
        GameInformation.Instance.Reload();
        controller.GenerateItem();
        controller.GeneratePlayer(this);
        //controller.IsStart = true;
    }

    public void SetPlayerSkinNName(string ConnectionID, string Skin, string Name){
        playerSkinNName.Add(ConnectionID, new KeyValuePair<string, string>(Name, Skin));
    }
}
