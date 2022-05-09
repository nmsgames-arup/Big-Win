using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using LuckyBall.UI;
using LuckyBall.Utility;
using KhushbuPlugin;
using Newtonsoft.Json;

namespace LuckyBall.Gameplay
{
    public class LuckyBall_Timer : MonoBehaviour
    {
        public static LuckyBall_Timer Instance;
        int bettingTime = 15;
        int timeUpTimer = 10;
        int waitTimer = 3;
        public Action onTimeUp;
        public Action onCountDownStart;
        public Action startCountDown;
        public static gameState gamestate;
        [SerializeField] Text countdownTxt;
        [SerializeField] Text messageTxt;

        IEnumerator countDown;
        IEnumerator onTimeUpcountDown;
        IEnumerator onWaitcountDown;
        public void StartCoundown() => StartCoroutine(countDown);
        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            gamestate = gameState.cannotBet;
            countDown = Countdown();
            onTimeUpcountDown = TimpUpCountdown();
            onWaitcountDown = WaitCountdown();
            onTimeUp?.Invoke();
            onTimeUp();
            if(is_a_FirstRound)
            {
                LuckyBall_UiHandler.Instance.ShowMessage("please wait for next round...");
            }
        }

        //this will run once it connected to the server
        //it will carry the time and state of server
        IEnumerator Countdown(int time = -1)
        {

            onCountDownStart?.Invoke();
            gamestate = gameState.canBet;
            for (int i = time != -1 ? time : bettingTime; i >= 0; i--)
            {
                if (i == 1)
                {
                    startCountDown?.Invoke();
                }
                messageTxt.text = "Start Time";
                countdownTxt.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }
            onTimeUp?.Invoke();

        }
        IEnumerator TimpUpCountdown(int time = -1)
        {
            gamestate = gameState.cannotBet;
            onTimeUp?.Invoke();

            for (int i = time != -1 ? time : timeUpTimer; i >= 0; i--)
            {
                messageTxt.text = "Time Up";
                countdownTxt.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }

        }
        IEnumerator WaitCountdown(int time = -1)
        {
            gamestate = gameState.wait;
            for (int i = time != -1 ? time : waitTimer; i >= 0; i--)
            {
                messageTxt.text = "Wait Time";
                countdownTxt.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);
            }

        }


        public void OnTimerStart(object data)
        {
            if (is_a_FirstRound)
            {
                LuckyBall_UiHandler.Instance.HideMessage();
            }
            is_a_FirstRound = false;

            StopCoroutines();
            StartCoroutine(Countdown());
        }

        public void OnTimeUp(object data)
        {
            if (is_a_FirstRound) return;
            StopCoroutines();
            StartCoroutine(TimpUpCountdown());
        }

        public void OnWait(object data)
        {            
            StopCoroutines();
            // StartCoroutine(StartDragonAnim());           
            if (is_a_FirstRound) return;
            // StartCoroutine(WOF_UiHandler.Instance.StartImageAnimation());
            StopCoroutines();
            StartCoroutine(WaitCountdown());
        }
        public bool is_a_FirstRound = true;
        public void OnCurrentTime(object data = null)
        {
            is_a_FirstRound = true;
            onTimeUp();
            LuckyBall_UiHandler.Instance.ShowMessage("please wait for next round...");
            try
            {
                InitialData cr = JsonConvert.DeserializeObject<InitialData>(data.ToString());
                LuckyBall_UiHandler.Instance.UpDateBalance(float.Parse(cr.balance));
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void StopCoroutines()
        {
            StopCoroutine(Countdown());
            StopCoroutine(TimpUpCountdown());
            StopCoroutine(WaitCountdown());
        }
    }

    [Serializable]
    public class CurrentTimer
    {
        public gameState gameState;
        public int timer;
        public List<int> lastWins;
        public int LeftBets;
        public int MiddleBets;
        public int RightBets;
    }
    public enum gameState
    {
        canBet = 0,
        cannotBet = 1,
        wait = 2,
    }

    public class InitialData
    {
        public List<int> previousWins;
        public List<BotsBetsDetail> botsBetsDetails;
        public string balance;
    }
}
