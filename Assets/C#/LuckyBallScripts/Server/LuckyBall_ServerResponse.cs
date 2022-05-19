using UnityEngine;
using SocketIO;
using LuckyBall.Utility;
using LuckyBall.UI;
using LuckyBall.Gameplay;

namespace LuckyBall.ServerStuff
{
    public class LuckyBall_ServerResponse : LuckyBall_SocketHandler
    {
        public LuckyBall_ServerRequest serverRequest;
        private void Start()
        {
            socket = GameObject.Find("SocketIOComponents").GetComponent<SocketIOComponent>();
            socket.On("open", OnConnected);
            socket.On("disconnected", OnDisconnected);
            socket.On(Events.OnChipMove, OnChipMove);   //NU
            socket.On(Events.OnGameStart, OnGameStart); //NU
            socket.On(Events.OnAddNewPlayer, OnAddNewPlayer);   //NU
            socket.On(Events.OnPlayerExit, OnPlayerExit);       //NU
            socket.On(Events.OnTimerStart, OnTimerStart);
            socket.On(Events.OnWait, OnWait);
            socket.On(Events.OnTimeUp, OnTimerUp);
            socket.On(Events.OnCurrentTimer, OnCurrentTimer);
            socket.On(Events.OnWinNo, OnWinNo);
            socket.On(Events.OnBotsData, OnBotsData);
            socket.On(Events.OnPlayerWin, OnPlayerWin);
            socket.On(Events.OnHistoryRecord, OnHistoryRecord);
            serverRequest.JoinGame();
        }
        void OnConnected(SocketIOEvent e)
        {
            print("connected");
            isConnected = true;
            serverRequest.JoinGame();
        }
        void OnDisconnected(SocketIOEvent e)
        {
            print("disconnected");
            isConnected = false;
        }
        void OnChipMove(SocketIOEvent e)
        {
            // LuckyBall_ChipController.Instance.OnOtherPlayerMove((object)e.data);
        }

        void OnBotsData(SocketIOEvent e)
        {
            LuckyBall_BetsHandler.Instance.AddBotsData(e.data);
        }

        void OnWinNo(SocketIOEvent e)
        {
            // WOF_RoundWinningHandler.Instance.OnWin(e.data);         //call this function when api is integrated
            LuckyBall_RoundWinningHandler.Instance.OnWin(e.data);
        }

        void OnGameStart(SocketIOEvent e)
        {
            Debug.Log("OnGameStart " + e.data);
            // LuckyBall_ChipController.Instance.OnOtherPlayerMove((object)e.data);
        }
        void OnAddNewPlayer(SocketIOEvent e)
        {
            Debug.Log("OnAddNewPlayer " + e.data);
            // LuckyBall_ChipController.Instance.OnOtherPlayerMove((object)e.data);
        }
        void OnPlayerExit(SocketIOEvent e)
        {
            Debug.Log("OnPlayerExit " + e.data);
            // LuckyBall_ChipController.Instance.OnOtherPlayerMove((object)e.data);
        }


        void OnTimerStart(SocketIOEvent e)
        {
            Debug.Log("on timer start " + e.data);
            LuckyBall_Timer.Instance.OnTimerStart((object)e.data);
            int ind = Random.Range(0, 10);
            if (ind % 2 == 0)
            {
                LuckyBall_UiHandler.Instance.PredictionTiger.SetActive(false);
                LuckyBall_UiHandler.Instance.PredictionDragon.SetActive(true);
            }
            else
            {
                LuckyBall_UiHandler.Instance.PredictionDragon.SetActive(false);
                LuckyBall_UiHandler.Instance.PredictionTiger.SetActive(true);
            }
        }

        void OnTimerUp(SocketIOEvent e)
        {
            Debug.Log("on timeUp " + e.data);
            LuckyBall_Timer.Instance.OnTimeUp((object)e.data);
        }
        void OnWait(SocketIOEvent e)
        {
            Debug.Log("on wait " + e.data);
            LuckyBall_Timer.Instance.OnWait((object)e.data);
        }
        void OnCurrentTimer(SocketIOEvent e)
        {
            Debug.Log("currunt data " + e.data);
            LuckyBall_BotsManager.Instance.UpdateBotData(e.data);
            LuckyBall_RoundWinningHandler.Instance.SetWinNumbers(e.data);
            LuckyBall_Timer.Instance.OnCurrentTime((object)e.data);
        }
        void OnPlayerWin(SocketIOEvent e)
        {
            Debug.Log("win something " + e.data);
            LuckyBall_UiHandler.Instance.OnPlayerWin(e.data);
        }
        void OnHistoryRecord(SocketIOEvent e)
        {
            Debug.Log("OnHistoryRecord " + e.data);
            // LuckyBall_UiHandler.Instance.ShowHistoryGame(e.data);
        }
    }
}
