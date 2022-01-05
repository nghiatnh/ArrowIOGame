using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public int MAX_PLAYER = 1;
    private bool isJoinedRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update(){
        if (isJoinedRoom && PhotonNetwork.PlayerList.Length >= MAX_PLAYER){
            GameInformation.Instance.gameMode = GAME_MODE.BATTLE;
            PhotonNetwork.LoadLevel("BattleScene");
            isJoinedRoom = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
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
        PhotonNetwork.NickName = GameInformation.Instance.PlayerName;
    }
}
