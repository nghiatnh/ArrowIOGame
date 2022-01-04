using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private bool isJoinedRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update(){
        if (isJoinedRoom && PhotonNetwork.PlayerList.Length >= 2){
            GameInformation.Instance.gameMode = GAME_MODE.BATTLE;
            PhotonNetwork.LoadLevel("BattleScene");
            isJoinedRoom = false;
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(System.Guid.NewGuid().ToString());
    }

    public override void OnJoinedRoom()
    {
        isJoinedRoom = true;
    }
}
