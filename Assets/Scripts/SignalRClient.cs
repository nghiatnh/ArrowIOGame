using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

public class SignalRClient : MonoBehaviour
{
    private static Uri uri = new Uri("https://localhost:44355/signalr");
    private static GameHub gameHub;
    private static Connection signalRConnection;

    private SignalRClient signalRClient;
    public OnlineSceneVars OnlineVars;
    private float searchTimeOut = -1;

    public void Connect(){
        signalRClient = this;

        gameHub = new GameHub(ref signalRClient, ref OnlineVars);
        signalRConnection = new Connection(uri, gameHub);
        signalRConnection.Open();

        signalRConnection.OnConnected += (conn) => {
            signalRConnection[gameHub.Name].Call("PlayerJoin", conn.NegotiationResult.ConnectionId, GameInformation.Instance.PlayerName, GameInformation.Instance.PlayerSkin.Name);
        };

        signalRConnection.OnError += (conn, err) => {
            Debug.Log(err);
        };
    }

    void Start(){
        Connect();
    }

    void OnApplicationQuit(){
        if (signalRConnection.State != ConnectionStates.Closed){
            signalRConnection.Close();
        }
    }


    public class GameHub : Hub {
        private SignalRClient signalRClient;
        private OnlineSceneVars OnlineVars;
        private int playerCount = 0;
        public GameHub(ref SignalRClient client, ref OnlineSceneVars vars) : base("GameHub")
        {
            this.signalRClient = client;
            this.OnlineVars = vars;
            base.On("PlayerJoined", PlayerJoined);
            base.On("PlayerJoin", PlayerJoin);
            base.On("StartGame", StartGame);
        }

        private void PlayerJoined(Hub hub, MethodCallMessage msgs){
            OnlineVars.SetTxtName(playerCount, msgs.Arguments[1].ToString());
            OnlineVars.SetPlayerSkinNName(msgs.Arguments[0].ToString(), msgs.Arguments[2].ToString(), msgs.Arguments[1].ToString());
            playerCount++;
            if (playerCount >= 2){
                signalRConnection[this.Name].Call("StartGame", msgs.Arguments[3].ToString());
            }
        }
        
        private void PlayerJoin(Hub hub, MethodCallMessage msgs){
            OnlineVars.SetTxtName(playerCount, msgs.Arguments[1].ToString());
            playerCount++;
            if (msgs.Arguments[0].ToString() != signalRConnection.NegotiationResult.ConnectionId){
                signalRConnection[this.Name].Call("PlayerJoined", msgs.Arguments[0].ToString(), GameInformation.Instance.PlayerName, GameInformation.Instance.PlayerSkin.Name);
                OnlineVars.SetPlayerSkinNName(msgs.Arguments[0].ToString(), msgs.Arguments[2].ToString(), msgs.Arguments[1].ToString());
            }
        }

        private void StartGame(Hub hub, MethodCallMessage msgs){
            OnlineVars.SetPnWait(false);
            OnlineVars.StartController();
        }
    }
}
