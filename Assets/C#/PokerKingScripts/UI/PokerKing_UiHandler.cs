using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using PokerKing.player;
using PokerKing.Utility;
using PokerKing.ServerStuff;
using PokerKing.Gameplay;
using Shared;

namespace PokerKing.UI
{
    public class PokerKing_UiHandler : MonoBehaviour
    {
        public static PokerKing_UiHandler Instance;
        [SerializeField] Button lobby;
        public GameObject HistoryPanel;
        public Image[] TopBG;
        public Image[] MiddleBG;
        public Image[] slotRecordBG;
        public Text SpadeBetsTxt, ClubBetsTxt, DiamondBetsTxt, HeartBetsTxt, Spade_ClubBetsTxt, Diamond_HeartBetsTxt, JokerBetsTxt;
        int SpadeBets, ClubBets, DiamondBets, HeartBets, Spade_ClubBets, Diamond_HeartBets, JokerBets;
        [SerializeField] Text balanceTxt;
        Dictionary<Spots, Text> betUiRefrence = new Dictionary<Spots, Text>();
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
        public GameObject[] chipimg;
        public Image loadingpnel;
        bool isLoading = true;
        int SpadeTotalBets, ClubTotalBets, DiamondTotalBets, HeartTotalBets, Spade_ClubTotalBets, Diamond_HeartTotalBets, JokerTotalBets;
        // int SpadeBets, ClubBets;
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

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            // dragonBetsTxt.text = "0";
            // LocalPlayer.LoadGame();
            currentChip = Chip.Chip2;
            betUiRefrence.Add(Spots.Spade, SpadeBetsTxt);
            betUiRefrence.Add(Spots.Club, ClubBetsTxt);
            betUiRefrence.Add(Spots.Diamond, DiamondBetsTxt);
            betUiRefrence.Add(Spots.Heart, HeartBetsTxt);
            betUiRefrence.Add(Spots.Spade_club, Spade_ClubBetsTxt);
            betUiRefrence.Add(Spots.Diamond_Heart, Diamond_HeartBetsTxt);
            betUiRefrence.Add(Spots.Joker, JokerBetsTxt);

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
                Application.Quit(); 
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
            PokerKing_ChipController.Instance.CoinMove_AudioSource.mute = !PokerKing_ChipController.Instance.CoinMove_AudioSource.mute;
            PokerKing_ChipController.Instance.GameAudio.mute = !PokerKing_ChipController.Instance.GameAudio.mute;
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
                case Spots.Spade:
                    SpadeTotalBets += (int)chip;
                    betValue = SpadeTotalBets.ToString();
                    break;
                case Spots.Club:
                    ClubTotalBets += (int)chip;
                    betValue = ClubTotalBets.ToString();
                    break;
                case Spots.Diamond:
                    DiamondTotalBets += (int)chip;
                    betValue = DiamondTotalBets.ToString();
                    break;
                case Spots.Heart:
                    HeartTotalBets += (int)chip;
                    betValue = HeartTotalBets.ToString();
                    break;
                case Spots.Spade_club:
                    Spade_ClubTotalBets += (int)chip;
                    betValue = Spade_ClubTotalBets.ToString();
                    break;
                case Spots.Diamond_Heart:
                    Diamond_HeartTotalBets += (int)chip;
                    betValue = Diamond_HeartTotalBets.ToString();
                    break;
                case Spots.Joker:
                    JokerTotalBets += (int)chip;
                    betValue = JokerTotalBets.ToString();
                    break;
                default:
                    break;
            }
            SetBets(SpadeTotalBets, ClubTotalBets, DiamondTotalBets, HeartTotalBets, Spade_ClubTotalBets, Diamond_HeartTotalBets, JokerTotalBets);
        }
        public void AddBets(Spots spot)
        {
            balance -= (float)currentChip;

            switch (spot)
            {
                case Spots.Spade:SpadeBets += (int)currentChip;
                    break;
                case Spots.Club:ClubBets += (int)currentChip;
                    break;
                case Spots.Diamond:DiamondBets += (int)currentChip;
                    break;
                case Spots.Heart:HeartBets += (int)currentChip;
                    break;
                case Spots.Spade_club:Spade_ClubBets += (int)currentChip;
                    break;
                case Spots.Diamond_Heart:Diamond_HeartBets += (int)currentChip;
                    break;
                case Spots.Joker:JokerBets += (int)currentChip;
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
            PokerKing_Timer.Instance.onCountDownStart += () =>
            {
                if (PokerKing_Timer.Instance.is_a_FirstRound) return;
                StartCoroutine(StartBetting());
            };
            PokerKing_Timer.Instance.startCountDown += () =>
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
            PokerKing_MainPlayer.Instance.totalBet = 0;
            // dragonBetsTxt.text = "0";
            SpadeTotalBets = 0;
            ClubTotalBets = 0;
            DiamondTotalBets = 0;
            HeartTotalBets = 0;
            Spade_ClubTotalBets = 0;
            Diamond_HeartTotalBets = 0;
            JokerTotalBets = 0;
            SpadeBetsTxt.text = string.Empty;
            ClubBetsTxt.text = string.Empty;
            DiamondBetsTxt.text = string.Empty;
            HeartBetsTxt.text = string.Empty;
            Spade_ClubBetsTxt.text = string.Empty;
            Diamond_HeartBetsTxt.text = string.Empty;
            JokerBetsTxt.text = string.Empty;
            UpdateUi();
        }
        public void SetBets(int spade, int club, int diamond, int heart, int spade_club, int diamond_heart, int joker)
        {
            SpadeTotalBets = spade;
            ClubTotalBets = club;
            DiamondTotalBets = diamond;
            HeartTotalBets = heart;
            Spade_ClubTotalBets = spade_club;
            Diamond_HeartTotalBets = diamond_heart;
            JokerTotalBets = joker;
            UpdateUi();
        }
        public void UpdateUi()
        {
            SpadeBetsTxt.text = SpadeTotalBets.ToString();
            ClubBetsTxt.text = ClubTotalBets.ToString();
            DiamondBetsTxt.text = DiamondTotalBets.ToString();
            HeartBetsTxt.text = HeartTotalBets.ToString();
            Spade_ClubBetsTxt.text = Spade_ClubTotalBets.ToString();
            Diamond_HeartBetsTxt.text = Diamond_HeartTotalBets.ToString();
            JokerBetsTxt.text = JokerTotalBets.ToString();
            balanceTxt.text = balance.ToString();
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
                PokerKing_MainPlayer.Instance.winner(Convert.ToInt32(win.winAmount));
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
        public void StopLoading()
        {
            // StopCoroutine(Loading());
            loadingpnel.gameObject.SetActive(false);
        }
        public void ExitLobby()
        {
            PokerKing_ServerRequest.instance.socket.Emit(Events.onleaveRoom);
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
                    MiddleBG[i].sprite = PokerKing_RoundWinningHandler.Instance.Imgs[0];
                }
                else if (Data.matrixRecord[i] == 1)
                {
                    MiddleBG[i].sprite = PokerKing_RoundWinningHandler.Instance.Imgs[1];
                }
                else if (Data.matrixRecord[i] == 2)
                {
                    MiddleBG[i].sprite = PokerKing_RoundWinningHandler.Instance.Imgs[1];
                }
            }
            for (int i = 0; i < PokerKing_RoundWinningHandler.Instance.previousWins.Length; i++)
            {
                TopBG[i].sprite = PokerKing_RoundWinningHandler.Instance.previousWins[i].sprite;
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
