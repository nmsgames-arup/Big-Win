using UpDown7.Utility;
using Updown7.ServerStuff;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Shared;
using UnityEngine;
using UnityEngine.UI;
using Updown7.ServerStuff;
using Updown7.Gameplay;
using System;
using UnityEngine.SceneManagement;

namespace Updown7.UI
{

    public class _7updown_UiHandler : MonoBehaviour
    {
        public static _7updown_UiHandler Instance;

        [SerializeField] Toggle chit2Btn;
        [SerializeField] Toggle chit10Btn;
        [SerializeField] Toggle chit50Btn;
        [SerializeField] Toggle chit100Btn;
        [SerializeField] Toggle chit500Btn;
        [SerializeField] Toggle chit1000Btn;
        [SerializeField] Toggle chit5000Btn;

        [SerializeField] Text leftBets;
        [SerializeField] Text middleBets;
        [SerializeField] Text rightBets;
        [SerializeField] TMP_Text usernameTxt;
        [SerializeField] TMP_Text balanceTxt;

        [SerializeField] Button lobby;

        Dictionary<Spot, Text> betUiRefrence = new Dictionary<Spot, Text>();
        public TMP_Text PlayerBet;
        float playerbetvalue = 0;
         public Chip currentChip;
        _7updown_Timer timer;
        public GameObject mainUnite;
        float balance;
        public Sprite[] frams;
        public Sprite stopBetting;
        public Sprite startBetting;
        public Image countdownPanel;
        public Image placeBets;
        public Sprite[] placeBets_Frames;
        public Sprite[] stopBets_Frames;
        public float countdownSpeed=.25f;
        public Button test;
        public Sprite[] ServerImage_Frames;
        public Image ServerImg;
        public Sprite[] WaitTxt_Frames;
        public Image WaitTxt_Img;
        public Sprite[] WaitStar_Frames;
        public Image WaitStar_Img;
        public GameObject[] chipimg;
        public Sprite[] characterFrames;
        public Image characterImg;
        int leftBets_value;
        int MiddleBets_value;
        int RightBets_value;
        public Text LeftBetsTxt, MiddleBetsTxt, RightBetsTxt;
        public GameObject QuitPanel;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            playerbetvalue = 0;
            PlayerBet.text = "0";
            chipimg[0].SetActive(false);
            currentChip = Chip.Chip2;
            ChipImgSelect(0);
            betUiRefrence.Add(Spot.left, leftBets);
            betUiRefrence.Add(Spot.middle, middleBets);
            betUiRefrence.Add(Spot.right, rightBets);
            countdownPanel.gameObject.SetActive(false);
            placeBets.gameObject.SetActive(false);
            timer = mainUnite.GetComponent<_7updown_Timer>();
            //test.onClick.AddListener(() => StartCoroutine(StartCountDown()));
            // SoundManager.instance.PlayBackgroundMusic();
            AddListeners();
            ResetUi();
            balance = 10000f;
            UpdateUi();
            StartCoroutine(StartCharacterAnimation());
            //StartCoroutine(Loading());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitPanel.SetActive(true);
            }
        }

        public IEnumerator StartCharacterAnimation()
        {
            characterImg.gameObject.SetActive(true);
            foreach (var item in characterFrames)
            {
                characterImg.sprite = item;
                yield return new WaitForSeconds(0.08f);
            }
            StartCoroutine(StartCharacterAnimation());
        }

        public IEnumerator StartplaceBets_Animation()
        {
            placeBets.gameObject.SetActive(true);
            foreach (var item in placeBets_Frames)
            {
                placeBets.sprite = item;
                yield return new WaitForSeconds(0.05f);
            }
            StopplaceBets_Animation();
        }
        public void StopplaceBets_Animation()
        {
            StopCoroutine(StartplaceBets_Animation());
            placeBets.gameObject.SetActive(false);
        }

        public IEnumerator Start_StopBets_Animation()
        {
            placeBets.gameObject.SetActive(true);
            foreach (var item in stopBets_Frames)
            {
                placeBets.sprite = item;
                yield return new WaitForSeconds(0.05f);
            }
            Stop_StopBets_Animation();
        }
        public void Stop_StopBets_Animation()
        {
            StopCoroutine(Start_StopBets_Animation());
            placeBets.gameObject.SetActive(false);
        }

        public IEnumerator StartServer_Animation()
        {
            ServerImg.gameObject.SetActive(true);
            foreach (var item in ServerImage_Frames)
            {
                ServerImg.sprite = item;
                yield return new WaitForSeconds(0.02f);
            }
            StopServer_Animation();
        }
        public void StopServer_Animation()
        {
            StopCoroutine(StartServer_Animation());
            ServerImg.gameObject.SetActive(false);
        }

        public IEnumerator StartWaitStar_Animation()
        {
            WaitStar_Img.gameObject.SetActive(true);
            foreach (var item in WaitStar_Frames)
            {
                WaitStar_Img.sprite = item;
                yield return new WaitForSeconds(0.08f);
            }
            StopWaitStar_Animation();
        }
        public void StopWaitStar_Animation()
        {
            StopCoroutine(StartWaitStar_Animation());
            WaitStar_Img.gameObject.SetActive(false);
            StartCoroutine(StartWaitTxt_Animation());
        }

        public IEnumerator StartWaitTxt_Animation()
        {
            WaitTxt_Img.gameObject.SetActive(true);
            foreach (var item in WaitTxt_Frames)
            {
                WaitTxt_Img.sprite = item;
                yield return new WaitForSeconds(0.08f);
            }
            StartCoroutine(StartWaitTxt_Animation());
        }
        public void StopWaitTxt_Animation()
        {
            StopCoroutine(StartWaitTxt_Animation());
            WaitTxt_Img.gameObject.SetActive(false);
        }

        IEnumerator StartBetting()
        {
            placeBets.gameObject.SetActive(true);
            placeBets.sprite = startBetting;
            StartCoroutine(StartplaceBets_Animation());
            yield return new WaitForSeconds(.5f);
            // placeBets.gameObject.SetActive(false);
        }

        int leftTotalBets;
        int middleTotalBets;
        int rightTotalBets;
        public void AddBotsBets(Spot spot, Chip chip)
        {
            string betValue = string.Empty;
            switch (spot)
            {
                case Spot.left:
                    leftTotalBets += (int)chip;
                    betValue = leftTotalBets.ToString();
                    break;
                case Spot.middle:
                    middleTotalBets += (int)chip;
                    betValue = middleTotalBets.ToString();
                    break;
                case Spot.right:
                    rightTotalBets += (int)chip;
                    betValue = rightTotalBets.ToString();
                    break;
                default:
                    break;
            }
            UpdateUi();
        }
        public void AddPlayerBets(Spot spot)
        {
            balance -= (float)currentChip;
            switch (spot)
            {
                case Spot.left:leftBets_value += (int)currentChip;
                    break;
                case Spot.middle:MiddleBets_value += (int)currentChip;
                    break;
                case Spot.right:RightBets_value += (int)currentChip;
                    break;
                default:
                    break;
            }
            // Debug.LogError("balance  " + balance);
            playerbetvalue += (float)currentChip;
            PlayerBet.text = playerbetvalue.ToString();
            UpdateUi();
        }
        public bool IsEnoughBalancePresent()
        {
            return balance - (float)currentChip > 0;
        }
        public void UpDateBalance(float amount)
        {
            StopLoading();
            isLoading = false;
            // balance = amount;
            balance = 10000f;
            UpdateUi();
        }
        public _7updown_LeftSideMenu sideMenu;
        private void AddListeners()
        {
            chit10Btn.onValueChanged.AddListener((isOn) =>
            {
                currentChip = Chip.Chip10;
                ChipImgSelect(1);
                // Debug.LogError("10 pressed  " + (float)currentChip + "  chip value   " + (float)Chip.Chip10);
            });
            // chit50Btn.onValueChanged.AddListener((isOn) =>
            // {
            //     // Debug.LogError("50 pressed  ");
            //     currentChip = Chip.Chip50;
            // });
            chit2Btn.onValueChanged.AddListener((isOn) =>
            {
                // Debug.LogError("50 pressed  ");
                currentChip = Chip.Chip2;
                ChipImgSelect(0);
            });
            chit100Btn.onValueChanged.AddListener((isOn) =>
            {
                // Debug.LogError("100 pressed  ");
                currentChip = Chip.Chip100;
                ChipImgSelect(2);
            });
            chit500Btn.onValueChanged.AddListener((isOn) =>
            {
                // Debug.LogError("500 pressed  ");
                currentChip = Chip.Chip500;
                ChipImgSelect(3);
            });
            chit1000Btn.onValueChanged.AddListener((isOn) =>
            {
                // Debug.LogError("1000 pressed  ");
                currentChip = Chip.Chip1000;
                ChipImgSelect(4);
            });
            chit5000Btn.onValueChanged.AddListener((isOn) =>
            {
                // Debug.LogError("5000 pressed  ");
                currentChip = Chip.Chip5000;
                ChipImgSelect(5);
            });
            timer.onCountDownStart += () =>
            {
                if (timer.is_a_FirstRound) return;
                StartCoroutine(StartBetting());
            };
            timer.startCountDown += () =>
            {
                // SoundManager.instance.PlayCountdown();
                StartCoroutine(StartCountDown());
            };
            // lobby.onClick.AddListener(() => sideMenu.ShowPopup());
        }
        public void ChipImgSelect(int ind)
        {
            for (int i = 0; i < chipimg.Length; i++)
            {
                chipimg[i].SetActive(false);
            }
            chipimg[ind].SetActive(true);
        }
       public void ResetUi()
        {
            playerbetvalue = 0;
            PlayerBet.text = "0";
            leftTotalBets = 0;
            middleTotalBets = 0;
            rightTotalBets = 0;
            leftBets_value = 0;
            MiddleBets_value = 0;
            RightBets_value = 0;
            leftBets.text = string.Empty;
            middleBets.text = string.Empty;
            rightBets.text = string.Empty;
        }
        public void SetBets(int left,int middle,int right)
        {
            leftTotalBets = left;
            middleTotalBets = middle;
            rightTotalBets = right;
            UpdateUi();
        }
       public void UpdateUi()
        {
            leftBets.text = leftTotalBets.ToString();
            middleBets.text = middleTotalBets.ToString();
            rightBets.text = rightTotalBets.ToString();
            balanceTxt.text = balance.ToString();
            usernameTxt.text ="000"+ LocalPlayer.deviceId;
            if(leftBets_value == 0)
            {
                LeftBetsTxt.text = "Click to play";
            }
            else
            {
                LeftBetsTxt.text = leftBets_value.ToString();
            }
            if(RightBets_value == 0)
            {
                RightBetsTxt.text = "Click to play";
            }
            else
            {
                RightBetsTxt.text = RightBets_value.ToString();
            }
            if(MiddleBets_value == 0)
            {
                MiddleBetsTxt.text = "Click to play";
            }
            else
            {
                MiddleBetsTxt.text = MiddleBets_value.ToString();
            }
        }

        public void ExitLobby()
        {
            ServerRequest.instance.socket.Emit(Events.onleaveRoom);
            SceneManager.LoadScene("MainScene");
        }


        [SerializeField] GameObject messagePopUP;
        [SerializeField] TMP_Text msgTxt;
        public void ShowMessage(string msg)
        {
            messagePopUP.SetActive(true);
            msgTxt.text = msg;
            StartCoroutine(StartWaitStar_Animation());
        }
        public void HideMessage()
        {
            messagePopUP.SetActive(false);
            msgTxt.text = string.Empty;
            StopWaitTxt_Animation();
        }

        private void OnApplicationQuit()
        {
            LocalPlayer.balance = balance.ToString();
            LocalPlayer.SaveGame();
        }
        public void AutoHidehowMessage(string msg, int time)
        {
            StartCoroutine(HidePanel(time));
            messagePopUP.SetActive(true);
            msgTxt.text = msg;
        }
        IEnumerator HidePanel(int time)
        {
            yield return new WaitForSeconds(time);
            messagePopUP.SetActive(false);
        }
        public void OnPlayerWin(object o)
        {
            Win win = UpDown7.Utility.Utility.GetObjectOfType<Win>(o);
            balance += win.winAmount;
            UpdateUi();
        }

        IEnumerator StartCountDown()
        {
            countdownPanel.gameObject.SetActive(true);
            foreach (var item in frams)
            {
                countdownPanel.sprite = item;
                yield return new WaitForSeconds(countdownSpeed);
            }
            countdownPanel.gameObject.SetActive(false);
            placeBets.gameObject.SetActive(true);
            placeBets.sprite = stopBetting;
            StartCoroutine(Start_StopBets_Animation());
            yield return new WaitForSeconds(0.5f);
            // placeBets.gameObject.SetActive(false);
        }

        public Image loadingpnel;
        public Image loadingImag;
        public Sprite[] loadingFrames;
        bool isLoading = true;
        IEnumerator Loading()
        {
            loadingpnel.gameObject.SetActive(true);
            foreach (var item in loadingFrames)
            {
                if (!isLoading) yield break;
                loadingImag.sprite = item;
                yield return new WaitForEndOfFrame();
            }
            StartCoroutine(Loading());
        }
        public void StopLoading()
        {
            StopCoroutine(Loading());
            loadingpnel.gameObject.SetActive(false);

        }



    }
    
}

[Serializable]
public class Win
{
    public float winAmount;
}