using WOF.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WOF.ServerStuff;
using System;
using WOF.Gameplay;
using Shared;
using UnityEngine.SceneManagement;
using System.Linq;
using WOF.player;

namespace WOF.UI
{
    public class WOF_UiHandler : MonoBehaviour
    {
        public static WOF_UiHandler Instance;
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

        public Chip currentChip;
        float balance;

        public Sprite stopBetting;
        public Sprite startBetting;
        public Image placeBets;
        public Sprite[] placeBets_Frames;
        public Sprite[] stopBets_Frames;
        public float countdownSpeed = .25f;
        public Text BlueBetsTxt;
        public Text YellowBetsTxt;
        public Text RedBetsTxt;
        public GameObject[] chipimg;
        public Image loadingpnel;
        public Image loadingImag;
        public Sprite[] loadingFrames;
        public Sprite[] DragonAnimFrames;
        public Image DragonAnimpanel;
        bool isLoading = true;
        int leftTotalBets;
        int middleTotalBets;
        int rightTotalBets;
        int BlueBets;
        int RedBets;
        int YellowBets;
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
        public Image SoundImg;
        public Sprite SoundOFF, SoundON;
        public GameObject QuitPanel;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            BlueBetsTxt.text = "Click to play";
            YellowBetsTxt.text = "Click to play";
            RedBetsTxt.text = "Click to play";
            // dragonBetsTxt.text = "0";
            // LocalPlayer.LoadGame();
            currentChip = Chip.Chip2;
            betUiRefrence.Add(Spot.left, leftBets);
            betUiRefrence.Add(Spot.middle, middleBets);
            betUiRefrence.Add(Spot.right, rightBets);
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
            WOF_ChipController.Instance.CoinMove_AudioSource.mute = !WOF_ChipController.Instance.CoinMove_AudioSource.mute;
            WOF_ChipController.Instance.GameAudio.mute = !WOF_ChipController.Instance.GameAudio.mute;
        }

        public void RulesBtn()
        {
            RulesScript.Instance.ShowRulesUI();
        }

        // public void SFXButtons(bool isChange = false)
        // {
        //     if (isChange)
        //     {
        //         UtilitySound.Instance.ButtonClickSound();
        //         UtilitySound.Instance.ToggleSound();
        //     }
        //     if (!UtilityModel.GetSound())
        //     {
        //         SoundImg.sprite = SoundOFF;
        //     }
        //     else
        //     {
        //         SoundImg.sprite = SoundON;
        //     }
        // }

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

        public IEnumerator StartCoinAnimation()
        {
            CoinBurstImage.gameObject.SetActive(true);
            foreach (var item in CoinBurstFrame)
            {
                CoinBurstImage.sprite = item;
                yield return new WaitForSeconds(0.08f);
            }
            StopCoinAnimation();
        }
        public void StopCoinAnimation()
        {
            StopCoroutine(StartCoinAnimation());
            CoinBurstImage.gameObject.SetActive(false);
        }

        public IEnumerator StartCoinBlinkAnimation()
        {
            CoinBlinkImage.gameObject.SetActive(true);
            foreach (var item in CoinBlinkFrame)
            {
                CoinBlinkImage.sprite = item;
                yield return new WaitForSeconds(0.08f);
            }
            StopCoinBlinkAnimation();
        }
        public void StopCoinBlinkAnimation()
        {
            StopCoroutine(StartCoinBlinkAnimation());
            CoinBlinkImage.gameObject.SetActive(false);
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

        void SetupProfile()
        {
            //int playerId=LocalPlayer.de
        }
        IEnumerator StartBetting()
        {
            ResetUi();
            BlueBetsTxt.text = "Click to play";
            YellowBetsTxt.text = "Click to play";
            RedBetsTxt.text = "Click to play";
            // dragonBetsTxt.text = "0";
            placeBets.gameObject.SetActive(true);
            StartCoroutine(StartplaceBets_Animation());
            placeBets.sprite = startBetting;
            yield return new WaitForSeconds(.5f);
            // placeBets.gameObject.SetActive(false);
        }

        public void UpDateBets(Spot spot, Chip chip)
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
            SetBets(leftTotalBets, middleTotalBets, rightTotalBets);
        }
        public void AddBets(Spot spot)
        {
            balance -= (float)currentChip;

            switch (spot)
            {
                case Spot.left:BlueBets += (int)currentChip;
                    break;
                case Spot.middle:RedBets += (int)currentChip;
                    break;
                case Spot.right:YellowBets += (int)currentChip;
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
            WOF_Timer.Instance.onCountDownStart += () =>
            {
                if (WOF_Timer.Instance.is_a_FirstRound) return;
                StartCoroutine(StartBetting());
            };
            WOF_Timer.Instance.startCountDown += () =>
            {
                StartCoroutine(StartCountDown());
            };
            // lobby.onClick.AddListener(() => sideMenu.ShowPopup());
        }
        public void BetValueSelect(int val)
        {
            if (val == 10)
            {
                currentChip = Chip.Chip10;
            }
            else if (val == 2)
            {
                // currentChip = Chip.Chip50;
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

        public void ResetUi()
        {
            WOF_MainPlayer.Instance.totalBet = 0;
            BlueBetsTxt.text = "Click to play";
            YellowBetsTxt.text = "Click to play";
            RedBetsTxt.text = "Click to play";
            // dragonBetsTxt.text = "0";
            leftTotalBets = 0;
            middleTotalBets = 0;
            rightTotalBets = 0;
            YellowBets = 0;
            BlueBets = 0;
            RedBets = 0;
            leftBets.text = string.Empty;
            middleBets.text = string.Empty;
            rightBets.text = string.Empty;
            UpdateUi();
        }
        public void SetBets(int left, int middle, int right)
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
            if(BlueBets == 0)
            {
                BlueBetsTxt.text = "Click to play";
            }
            else
            {
                BlueBetsTxt.text = BlueBets.ToString();
            }
            if(YellowBets == 0)
            {
                YellowBetsTxt.text = "Click to play";
            }
            else
            {
                YellowBetsTxt.text = YellowBets.ToString();
            }
            if(RedBets == 0)
            {
                RedBetsTxt.text = "Click to play";
            }
            else
            {
                RedBetsTxt.text = RedBets.ToString();
            }
            // dragonBetsTxt.text = dragonBets.ToString();
            // tieBetsTxt.text = tieBetsBets.ToString();
            // tigerBetsTxt.text = tigerBets.ToString();
            // usernameTxt.text = LocalPlayer.deviceId;
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
            Win win = Dragon.Utility.Utility.GetObjectOfType<Win>(o);
            if (win.winAmount != 0)
                WOF_MainPlayer.Instance.winner(Convert.ToInt32(win.winAmount));
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

        
        IEnumerator Loading()
        {
            // loadingpnel.gameObject.SetActive(true);
            // foreach (var item in loadingFrames)
            // {
            //     if (!isLoading) yield break;
            //     loadingImag.sprite = item;
                yield return new WaitForEndOfFrame();
            // }
            // StartCoroutine(Loading());
        }
        public void StopLoading()
        {
            // StopCoroutine(Loading());
            loadingpnel.gameObject.SetActive(false);
        }
        public void ExitLobby()
        {
            ServerRequest.instance.socket.Emit(Events.onleaveRoom);
            SceneManager.LoadScene("MainScene");
        }
        public void ShowHistoryGame(object o)
        {
            HistoryPanel.SetActive(true);
            HistoryRoot Data = Dragon.Utility.Utility.GetObjectOfType<HistoryRoot>(o);
            for (int i = 0; i < Data.matrixRecord.Count; i++)
            {
                if (Data.matrixRecord[i] == 0)
                {
                    MiddleBG[i].sprite = WOF_RoundWinningHandler.Instance.Imgs[0];
                }
                else if (Data.matrixRecord[i] == 1)
                {
                    MiddleBG[i].sprite = WOF_RoundWinningHandler.Instance.Imgs[1];
                }
                else if (Data.matrixRecord[i] == 2)
                {
                    MiddleBG[i].sprite = WOF_RoundWinningHandler.Instance.Imgs[1];
                }
            }
            for (int i = 0; i < WOF_RoundWinningHandler.Instance.previousWins.Length; i++)
            {
                TopBG[i].sprite = WOF_RoundWinningHandler.Instance.previousWins[i].sprite;
            }
            int[] turnArray = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3 };
            for (int i = 0; i < slotRecordBG.Length; i++)
            {
                System.Random rands = new System.Random();
                List<int> rand = turnArray.OrderBy(c => rands.Next()).Select(c => c).ToList();
                int T1 = Data.slotRecord[i].T - 1;
                int D2 = Data.slotRecord[i].D - 1;
                if (Data.slotRecord[i].T > Data.slotRecord[i].D)
                {
                    slotRecordBG[i].transform.GetChild(0).GetComponent<Image>().sprite = TigerSelect;
                    slotRecordBG[i].transform.GetChild(1).GetComponent<Image>().sprite = DragonUnSelect;
                }
                else if (Data.slotRecord[i].T < Data.slotRecord[i].D)
                {
                    slotRecordBG[i].transform.GetChild(0).GetComponent<Image>().sprite = TigerUnSelect;
                    slotRecordBG[i].transform.GetChild(1).GetComponent<Image>().sprite = DragonSelect;
                }
                else
                {
                    slotRecordBG[i].transform.GetChild(0).GetComponent<Image>().sprite = TigerUnSelect;
                    slotRecordBG[i].transform.GetChild(1).GetComponent<Image>().sprite = DragonUnSelect;
                }
                slotRecordBG[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = T1.ToString();
                slotRecordBG[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = D2.ToString();
            }
            
        }
        public void CloseHistoryUI()
        {
            HistoryPanel.SetActive(false);
        }
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
