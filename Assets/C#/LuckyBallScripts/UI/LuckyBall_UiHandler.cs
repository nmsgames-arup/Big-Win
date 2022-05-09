using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using LuckyBall.Utility;
using LuckyBall.Gameplay;
using LuckyBall.ServerStuff;
using LuckyBall.player;
using Shared;

namespace LuckyBall.UI
{
    public class LuckyBall_UiHandler : MonoBehaviour
    {
        public static LuckyBall_UiHandler Instance;
        public Chip currentChip;
        float balance;
        bool isLoading = true;
        public GameObject[] chipimg;
        public Image loadingpnel;

        [SerializeField] Button lobby;
        public GameObject HistoryPanel;
        public GameObject PredictionTiger;
        public GameObject PredictionDragon;
        public Image[] TopBG;
        public Image[] MiddleBG;
        public Image[] slotRecordBG;
        [SerializeField] Text leftBets;
        [SerializeField] Text middleBets;
        [SerializeField] Text rightBets;
        [SerializeField] Text usernameTxt;
        [SerializeField] Text balanceTxt;
        Dictionary<Spot, Text> betUiRefrence = new Dictionary<Spot, Text>();
        public Sprite TigerSelect;
        public Sprite TigerUnSelect;
        public Sprite DragonSelect;
        public Sprite DragonUnSelect;

        public Sprite stopBetting;
        public Sprite startBetting;
        public Image placeBets;
        public Sprite[] placeBets_Frames;
        public Sprite[] stopBets_Frames;
        public float countdownSpeed = .25f;
        public Text ZeroBetsTxt, OneBetsTxt, TwoBetsTxt, ThreeBetsTxt, FourBetsTxt, FiveBetsTxt, SixBetsTxt, SevenBetsTxt,
            EightBetsTxt, NineBetsTxt, One_FourBetsTxt, Zero_FiveBetsTxt, Six_NineBetsTxt;
        public Text YellowBetsTxt;
        public Text RedBetsTxt;
        public Image loadingImag;
        public Sprite[] loadingFrames;
        public Sprite[] DragonAnimFrames;
        public Image DragonAnimpanel;
        int leftTotalBets;
        int middleTotalBets;
        int rightTotalBets;
        int ZeroBets, OneBets, TwoBets, ThreeBets, FourBets, FiveBets, SixBets, SevenBets, EightBets, NineBets,
        one_FourBets, Zero_FiveBets, Six_NineBets;
        public Image StartImg;
        public Sprite[] StartFrames;
        public Image CoinBurstImage;
        public Sprite[] CoinBurstFrame;
        public Image CoinBlinkImage;
        public Sprite[] CoinBlinkFrame;
        public Sprite[] ServerImage_Frames;
        public Image ServerImg;
        [SerializeField] GameObject messagePopUP;
        [SerializeField] Text msgTxt;
        public Sprite[] WaitTxt_Frames;
        public Image WaitTxt_Img;
        public Sprite[] WaitStar_Frames;
        public Image WaitStar_Img;
        public Sprite[] ShufflingImage_Sprites;
        public Image ShufflingImage;
        public Sprite[] WinnerImage_Sprite;
        public Image WinnerImage;
        public Image SoundImg;
        public Sprite SoundOFF, SoundON;
        public GameObject QuitPanel;

        void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            ZeroBetsTxt.text = OneBetsTxt.text = TwoBetsTxt.text = ThreeBetsTxt.text = FourBetsTxt.text = 
            FiveBetsTxt.text = SixBetsTxt.text = SevenBetsTxt.text = EightBetsTxt.text = NineBetsTxt.text = One_FourBetsTxt.text = Zero_FiveBetsTxt.text = Six_NineBetsTxt.text = "0";
            // dragonBetsTxt.text = "0";
            // LocalPlayer.LoadGame();
            currentChip = Chip.Chip2;
            // betUiRefrence.Add(Spot.left, leftBets);
            // betUiRefrence.Add(Spot.middle, middleBets);
            // betUiRefrence.Add(Spot.right, rightBets);
            placeBets.gameObject.SetActive(false);
            AddListeners();
            ResetUi();
            // balance = float.Parse(LocalPlayer.balance);
            balance = 10000f;
            UpdateUi();
            // StartCoroutine(Loading());
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitPanel.SetActive(true);
            }
        }

        public void SoundOnOff()
        {
            if(SoundImg.sprite == SoundOFF)
            {
                SoundImg.sprite = SoundON;
            }
            else
            {
                SoundImg.sprite = SoundOFF;
            }
            LuckyBall_ChipController.Instance.CoinMove_AudioSource.mute = !LuckyBall_ChipController.Instance.CoinMove_AudioSource.mute;
        }

        public void RulesBtn()
        {
            RulesScript.Instance.ShowRulesUI();
        }

        public IEnumerator StartImageAnimation()
        {
            StartImg.gameObject.SetActive(true);
            foreach (var item in StartFrames)
            {
                StartImg.sprite = item;
                yield return new WaitForSeconds(0.08f);
            }
            StopImageAnimation();
        }
        public void StopImageAnimation()
        {
            StopCoroutine(StartImageAnimation());
            StartImg.gameObject.SetActive(false);
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
            ResetUi();
            ZeroBetsTxt.text = OneBetsTxt.text = TwoBetsTxt.text = ThreeBetsTxt.text = FourBetsTxt.text = 
            FiveBetsTxt.text = SixBetsTxt.text = SevenBetsTxt.text = EightBetsTxt.text = NineBetsTxt.text = One_FourBetsTxt.text = Zero_FiveBetsTxt.text = Six_NineBetsTxt.text = "0";
            // dragonBetsTxt.text = "0";
            placeBets.gameObject.SetActive(true);
            StartCoroutine(StartplaceBets_Animation());
            placeBets.sprite = startBetting;
            yield return new WaitForSeconds(.5f);
            // placeBets.gameObject.SetActive(false);
        }

        public void UpDateBets(Spots spot, Chip chip)
        {
            string betValue = string.Empty;
            switch (spot)
            {
                case Spots.Zero:
                    ZeroBets += (int)chip;
                    betValue = ZeroBets.ToString();
                    break;
                case Spots.One:
                    OneBets += (int)chip;
                    betValue = OneBets.ToString();
                    break;
                case Spots.Two:
                    TwoBets += (int)chip;
                    betValue = TwoBets.ToString();
                    break;
                case Spots.Three:
                    ThreeBets += (int)chip;
                    betValue = ThreeBets.ToString();
                    break;
                case Spots.Four:
                    FourBets += (int)chip;
                    betValue = FourBets.ToString();
                    break;
                case Spots.Five:
                    FiveBets += (int)chip;
                    betValue = FiveBets.ToString();
                    break;
                case Spots.Six:
                    SixBets += (int)chip;
                    betValue = SixBets.ToString();
                    break;
                case Spots.Seven:
                    SevenBets += (int)chip;
                    betValue = SevenBets.ToString();
                    break;
                case Spots.Eight:
                    EightBets += (int)chip;
                    betValue = EightBets.ToString();
                    break;
                case Spots.Nine:
                    NineBets += (int)chip;
                    betValue = NineBets.ToString();
                    break;
                case Spots.One_Four:
                    one_FourBets += (int)chip;
                    betValue = one_FourBets.ToString();
                    break;
                case Spots.Zero_Five:
                    Zero_FiveBets += (int)chip;
                    betValue = Zero_FiveBets.ToString();
                    break;
                case Spots.Six_Nine:
                    Six_NineBets += (int)chip;
                    betValue = Six_NineBets.ToString();
                    break;
                default:
                    break;
            }
            SetBets(leftTotalBets, middleTotalBets, rightTotalBets);
        }

        public void AddBets(Spots spot)
        {
            balance -= (float)currentChip;

            switch (spot)
            {
                case Spots.Zero:
                    ZeroBets += (int)currentChip;
                    break;
                case Spots.One:
                    OneBets += (int)currentChip;
                    break;
                case Spots.Two:
                    TwoBets += (int)currentChip;
                    break;
                case Spots.Three:
                    ThreeBets += (int)currentChip;
                    break;
                case Spots.Four:
                    FourBets += (int)currentChip;
                    break;
                case Spots.Five:
                    FiveBets += (int)currentChip;
                    break;
                case Spots.Six:
                    SixBets += (int)currentChip;
                    break;
                case Spots.Seven:
                    SevenBets += (int)currentChip;
                    break;
                case Spots.Eight:
                    EightBets += (int)currentChip;
                    break;
                case Spots.Nine:
                    NineBets += (int)currentChip;
                    break;
                case Spots.One_Four:
                    one_FourBets += (int)currentChip;
                    break;
                case Spots.Zero_Five:
                    Zero_FiveBets += (int)currentChip;
                    break;
                case Spots.Six_Nine:
                    Six_NineBets += (int)currentChip;
                    break;
                default:
                    break;
            }
            UpdateUi();
        }
        public bool IsEnoughBalancePresent()
        {
            return balance - (float)currentChip > 0;
        }
        public void UpDateBalance(float amount)
        {
            Debug.Log("balance updated");
            StopLoading();
            isLoading = false;
            // balance = amount;
            balance = 10000f;
            UpdateUi();
        }
        private void AddListeners()
        {
            LuckyBall_Timer.Instance.onCountDownStart += () =>
            {
                if (LuckyBall_Timer.Instance.is_a_FirstRound) return;
                StartCoroutine(StartBetting());
            };
            LuckyBall_Timer.Instance.startCountDown += () =>
            {
                StartCoroutine(StartCountDown());
            };
            // lobby.onClick.AddListener(() => sideMenu.ShowPopup());
        }

        public void ResetUi()
        {
            LuckyBall_MainPlayer.Instance.totalBet = 0;
            ZeroBetsTxt.text = OneBetsTxt.text = TwoBetsTxt.text = ThreeBetsTxt.text = FourBetsTxt.text = 
            FiveBetsTxt.text = SixBetsTxt.text = SevenBetsTxt.text = EightBetsTxt.text = NineBetsTxt.text = One_FourBetsTxt.text = Zero_FiveBetsTxt.text = Six_NineBetsTxt.text = "0";
            // dragonBetsTxt.text = "0";
            leftTotalBets = 0;
            middleTotalBets = 0;
            rightTotalBets = 0;
            ZeroBets = 0;
            OneBets = 0;
            TwoBets = 0;
            ThreeBets = 0;
            FourBets = 0;
            FiveBets = 0;
            SixBets = 0;
            SevenBets = 0;
            EightBets = 0;
            NineBets = 0;
            one_FourBets = 0;
            Zero_FiveBets = 0;
            Six_NineBets = 0;
            // leftBets.text = string.Empty;
            // middleBets.text = string.Empty;
            // rightBets.text = string.Empty;
            UpdateUi();
        }
        public void SetBets(int left, int middle, int right)
        {
            leftTotalBets = left;
            middleTotalBets = middle;
            rightTotalBets = right;
            UpdateUi();
        }

        public void StopLoading()
        {
            // StopCoroutine(Loading());
            loadingpnel.gameObject.SetActive(false);
        }

        public void UpdateUi()
        {
            // leftBets.text = leftTotalBets.ToString();
            // middleBets.text = middleTotalBets.ToString();
            // rightBets.text = rightTotalBets.ToString();
            balanceTxt.text = balance.ToString();
            
            ZeroBetsTxt.text = ZeroBets.ToString();
            OneBetsTxt.text = OneBets.ToString();
            TwoBetsTxt.text = TwoBets.ToString();
            ThreeBetsTxt.text = ThreeBets.ToString();
            FourBetsTxt.text = FourBets.ToString();
            FiveBetsTxt.text = FiveBets.ToString();
            SixBetsTxt.text = SixBets.ToString();
            SevenBetsTxt.text = SevenBets.ToString();
            EightBetsTxt.text = EightBets.ToString();
            NineBetsTxt.text = NineBets.ToString();
            One_FourBetsTxt.text = one_FourBets.ToString();
            Zero_FiveBetsTxt.text = Zero_FiveBets.ToString();
            Six_NineBetsTxt.text = Six_NineBets.ToString();
        }

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
            // LocalPlayer.SaveGame();
        }

        public void OnPlayerWin(object o)
        {
            Win win = Dragon.Utility.Utility.GetObjectOfType<Win>(o);
            if (win.winAmount != 0)
                LuckyBall_MainPlayer.Instance.winner(Convert.ToInt32(win.winAmount));
            balance += win.winAmount;
            UpdateUi();

        }
        IEnumerator StartCountDown()
        {
            placeBets.gameObject.SetActive(true);
            placeBets.sprite = stopBetting;
            StartCoroutine(Start_StopBets_Animation());
            yield return new WaitForSeconds(0.5f);
            // placeBets.gameObject.SetActive(false);
        }

        public void ExitLobby()
        {
            LuckyBall_ServerRequest.instance.socket.Emit(Events.onleaveRoom);
            SceneManager.LoadScene("MainScene");
        }

        public void BetValueSelect(int val)
        {
            if (val == 10)
            {
                currentChip = Chip.Chip10;
            }
            else if (val == 50)
            {
                currentChip = Chip.Chip50;
            }
            else if (val == 2)
            {
                currentChip = Chip.Chip2;
            }
            else if (val == 100)
            {
                currentChip = Chip.Chip100;
            }
            else if (val == 500)
            {
                currentChip = Chip.Chip500;
            }
            else if (val == 1000)
            {
                currentChip = Chip.Chip1000;
            }
            else if (val == 5000)
            {
                currentChip = Chip.Chip5000;
            }      
        }    
        public void ChipImgSelect(int ind)
        {
            for (int i = 0; i < chipimg.Length; i++)
            {
                chipimg[i].SetActive(false);
            }
            chipimg[ind].SetActive(true);
        }

    }

    
    [Serializable]
    public class Win
    {
        public float winAmount;
    }
    [System.Serializable]
    public class SlotRecord
    {
        public int D;
        public int T;
    }
    [System.Serializable]
    public class HistoryRoot
    {
        public List<int> matrixRecord;
        public List<SlotRecord> slotRecord;
    }
}
