using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public GameObject offlineController;
    public GameObject battleController;
    // Start is called before the first frame update
    void Start()
    {
        switch (GameInformation.Instance.gameMode){
            case GAME_MODE.OFFLINE:
                Debug.Log("Offline");
                Instantiate(offlineController);
                break;
            case GAME_MODE.BATTLE:
                Instantiate(battleController);
                break;
            case GAME_MODE.RANK:
                break;
            case GAME_MODE.TEAMFIGHT:
                break;
            case GAME_MODE.VS5:
                break;
        }
    }
}
