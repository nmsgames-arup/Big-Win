using UnityEngine;
using SocketIO;
using Updown7.Gameplay;
using UpDown7.Utility;
using Updown7.UI;

namespace Updown7.ServerStuff
{
    public class LuckyDice_ServerResponse : SocketHandler
    {
        public ServerRequest serverRequest;
        private void Start()
        {
            socket = GameObject.Find("SocketIOComponents").GetComponent<SocketIOComponent>();
            socket.On("open", OnConnected);
            socket.On("disconnected", OnDisconnected);
            socket.On(Events.OnChipMove, OnChipMove);
            socket.On(Events.OnGameStart, OnGameStart);
            socket.On(Events.OnAddNewPlayer, OnAddNewPlayer);
            socket.On(Events.OnPlayerExit, OnPlayerExit);
            socket.On(Events.OnTimerStart, OnTimerStart);
            socket.On(Events.OnWait, OnWait);
            socket.On(Events.OnTimeUp, OnTimerUp);
            socket.On(Events.OnCurrentTimer, OnCurrentTimer);
            socket.On(Events.OnWinNo, OnWinNo);
            socket.On(Events.OnBotsData, OnBotsData);
            socket.On(Events.OnPlayerWin, OnPlayerWin);
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
            // ChipController.Instance.OnOtherPlayerMove((object)e.data);
            // IChipMovement.
        }

        void OnBotsData(SocketIOEvent e)
        {
            // BetsHandler.Instance.AddBotsData(e.data);
            _7updown_BetsHandler.Instance.AddBotsData(e.data);
        }

        void OnWinNo(SocketIOEvent e)
        {
            // RoundWinningHandler.Instance.OnWin(e.data);
            _7updown_RoundWinningHandler.Instance.OnWin(e.data);
        }

        void OnGameStart(SocketIOEvent e)
        {
            Debug.Log("OnGameStart " + e.data);
            // ChipController.Instance.OnOtherPlayerMove((object)e.data);
        }
        void OnAddNewPlayer(SocketIOEvent e)
        {
            Debug.Log("OnAddNewPlayer " + e.data);
            // ChipController.Instance.OnOtherPlayerMove((object)e.data);
        }
        void OnPlayerExit(SocketIOEvent e)
        {
            Debug.Log("OnPlayerExit " + e.data);
            // ChipController.Instance.OnOtherPlayerMove((object)e.data);
        }


        void OnTimerStart(SocketIOEvent e)
        {
            Debug.Log("on timer start " + e.data);
            // Timer.Instance.OnTimerStart((object)e.data);
            _7updown_Timer.Instance.OnTimerStart((object)e.data);
            // int ind = Random.Range(0, 10);
            // if (ind % 2 == 0)
            // {
            //     UiHandler.Instance.PredictionTiger.SetActive(false);
            //     UiHandler.Instance.PredictionDragon.SetActive(true);
            // }
            // else
            // {
            //     UiHandler.Instance.PredictionDragon.SetActive(false);
            //     UiHandler.Instance.PredictionTiger.SetActive(true);
            // }
        }

        void OnTimerUp(SocketIOEvent e)
        {
            Debug.Log("on timeUp " + e.data);
            // Timer.Instance.OnTimeUp((object)e.data);
            _7updown_Timer.Instance.OnTimeUp((object)e.data);
        }
        void OnWait(SocketIOEvent e)
        {
            Debug.Log("on wait " + e.data);
            // Timer.Instance.OnWait((object)e.data);
            _7updown_Timer.Instance.OnWait((object)e.data);
        }
        void OnCurrentTimer(SocketIOEvent e)
        {
            Debug.Log("currunt data " + e.data);
            _7updown_BotsManager.Instance.UpdateBotData(e.data);
            _7updown_RoundWinningHandler.Instance.SetWinNumbers(e.data);
            _7updown_Timer.Instance.OnCurrentTime((object)e.data);
        }
        void OnPlayerWin(SocketIOEvent e)
        {
            Debug.Log("win something " + e.data);
            _7updown_UiHandler.Instance.OnPlayerWin(e.data);
        }
        // void OnHistoryRecord(SocketIOEvent e)
        // {
        //     Debug.Log("OnHistoryRecord " + e.data);
        //     UiHandler.Instance.ShowHistoryGame(e.data);
        // }
    }
}
