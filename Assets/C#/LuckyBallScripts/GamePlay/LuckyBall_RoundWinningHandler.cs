using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LuckyBall.player;
using LuckyBall.UI;
using LuckyBall.Utility;
using KhushbuPlugin;
using Shared;

namespace LuckyBall.Gameplay
{
    public class LuckyBall_RoundWinningHandler : MonoBehaviour
    {
        public static LuckyBall_RoundWinningHandler Instance;
        [SerializeField] GameObject LeftRing;
        [SerializeField] GameObject middleRing;
        [SerializeField] GameObject rightRing;
        public Sprite[] Imgs;
        public Image[] previousWins;
        public List<int> PreviousWinValue;
        public List<GameObject> WinningRing;
        public Action<object> onWin;
        public Cards[] Card;
        public Image Card1;
        public Image Card2;
        public Sprite TigerBackCard, ElephantBackCard;
        //public string CardName[4]=new {"","","",""};
        bool isTimeUp;
        public Sprite[] TigerWinnerAnimation;
        public Sprite[] DragonWinnerAnimation;
        public Image TigerWinner;
        public Image DragonWinner;
        public Sprite[] TieWinnerAnimation;
        public Image TieWinner;

        public Image TigerWinner1;
        public Image DragonWinner1;
        int randNo;
        int wheelNo;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            LuckyBall_Timer.Instance.onTimeUp += () => isTimeUp = true;
            LuckyBall_Timer.Instance.onCountDownStart += () => isTimeUp = false;

            onWin += OnWin;
            //leftDice.SetActive(false);
            //rightDice.SetActive(false);       
        }

        public void SetWinNumbers(object o)
        {
            InitialData winData = Utility.Utility.GetObjectOfType<InitialData>(o);

            for (int i = 0; i < winData.previousWins.Count; i++)
            {
                int num = winData.previousWins[i];
                if (num == 0)
                {
                    previousWins[i].sprite = Imgs[0];//dragon
                }
                else if (num == 1)
                {
                    previousWins[i].sprite = Imgs[1];//tie
                }
                else
                {
                    previousWins[i].sprite = Imgs[2];//tiger
                }
                //previousWins[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = totalDiceNo.ToString();

            }
        }
        public void OnWin(object o)
        {
            Debug.Log("obj  " + o);
            DiceWinNos winData = Utility.Utility.GetObjectOfType<DiceWinNos>(o);
            wheelNo = winData.winningSpot;
            wheelNo = UnityEngine.Random.Range(0, 10);

            StartCoroutine(StartShuffling_Animation());

            // for(int i = 0;i < PreviousWinValue.Count; i++)
            // {
            //     previousWins[i].enabled = true;
            //     if(PreviousWinValue[i] == 0)
            //     {
            //         previousWins[i].sprite = Imgs[0];//Blue win
            //     }
            //     else if(PreviousWinValue[i] == 1)
            //     {
            //         previousWins[i].sprite = Imgs[1];//Red win
            //     }
            //     else
            //     {
            //         previousWins[i].sprite = Imgs[2];//Yellow win
            //     }
            // }

            // WOF_WheelSpin.Instance.Spin(wheelNo);
            // WOF_WheelSpin.Instance.onSpinComplete += mySpinComplete;
        }
        void mySpinComplete(){
            // if (randNo == 0)//aniamtion add khusi
            // {
            //     StartCoroutine(ShowWinningRing(LeftRing, Spot.left ));//dragon
            // }
            // else if (randNo == 1)
            // {
            //     StartCoroutine(ShowWinningRing(middleRing, Spot.middle ));
            // }
            // else
            // {
            //     StartCoroutine(ShowWinningRing(rightRing, Spot.right ));
            // }
            // // StartCoroutine(LuckyBall_UiHandler.Instance.StartCoinAnimation());
            // // StartCoroutine(LuckyBall_UiHandler.Instance.StartCoinBlinkAnimation());
            // // WOF_WheelSpin.Instance.onSpinComplete -= mySpinComplete;
        }

        /* Need to call below function of OnWin once api is integrated */
        // public void OnWin1(object o)
        // {
        //     if (WOF_Timer.Instance.is_a_FirstRound) return;
        //     DiceWinNos winData = Utility.Utility.GetObjectOfType<DiceWinNos>(o);
        //     int[] turnArray = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3 };
        //     System.Random rands = new System.Random();
        //     List<int> rand = turnArray.OrderBy(c => rands.Next()).Select(c => c).ToList();
        //     int No1 = winData.winNo[0] - 1;
        //     int No2 = winData.winNo[1] - 1;
        //     StartCoroutine(cardOpen(No1, No2, rand[0], rand[1]));

        //     for (int i = 0; i < winData.previousWins.Count; i++)
        //     {
        //         int num = winData.previousWins[i];
        //         if (num == 0)
        //         {
        //             previousWins[i].sprite = Imgs[0];//dragon
        //         }
        //         else if (num == 1)
        //         {
        //             previousWins[i].sprite = Imgs[1];//tie
        //         }
        //         else
        //         {
        //             previousWins[i].sprite = Imgs[2];//tiger
        //         }                
        //     }
        //     if (winData.winningSpot == 0)//aniamtion add khusi
        //     {
        //         StartCoroutine(ShowWinningRing(leftRing, Spot.left, o));//dragon
        //     }
        //     else if (winData.winningSpot == 1)
        //     {
        //         StartCoroutine(ShowWinningRing(middleRing, Spot.middle, o));
        //     }
        //     else
        //     {
        //         StartCoroutine(ShowWinningRing(rightRing, Spot.right, o));
        //     }
        // }
        IEnumerator cardOpen(int No1,int No2, int rand0, int rand1)
        {
            Card1.sprite = Card[rand0].card[No1];
            yield return new WaitForSeconds(0.5f);
            Card2.sprite = Card[rand1].card[No2];
        }
        IEnumerator ShowWinningRing( GameObject ring , Spots winnerSpot )
        {
            yield return new WaitForSeconds(1f);

            ring.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            ring.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            ring.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            ring.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            ring.SetActive(true);
            LuckyBall_ChipController.Instance.TakeChipsBack(winnerSpot);
            yield return new WaitForSeconds(2f);
            ring.SetActive(false);
        }

        IEnumerator StartShuffling_Animation()
        {
            LuckyBall_UiHandler.Instance.ShufflingImage.gameObject.SetActive(true);
            foreach (var item in LuckyBall_UiHandler.Instance.ShufflingImage_Sprites)
            {
                LuckyBall_UiHandler.Instance.ShufflingImage.sprite = item;
                yield return new WaitForSeconds(0.02f);
            }
            StopShuffling_Animation();
        }
        void StopShuffling_Animation()
        {
            StopCoroutine(StartShuffling_Animation());
            LuckyBall_UiHandler.Instance.ShufflingImage.sprite = LuckyBall_UiHandler.Instance.ShufflingImage_Sprites[0];
            LuckyBall_UiHandler.Instance.WinnerImage.enabled = true;
            LuckyBall_UiHandler.Instance.WinnerImage.sprite = LuckyBall_UiHandler.Instance.WinnerImage_Sprite[wheelNo];
            StartCoroutine(WinData());
        }

        IEnumerator WinData()
        {
            yield return new WaitForSeconds(2.0f);
            LuckyBall_UiHandler.Instance.WinnerImage.enabled = false;
            if(PreviousWinValue.Count > 11)
            {
                PreviousWinValue.RemoveAt(0);
                PreviousWinValue.Add(wheelNo);
            }
            else
            {
                PreviousWinValue.Add(wheelNo);
            }
            for(int i = 0; i < PreviousWinValue.Count; i++)
            {
                previousWins[i].enabled = true;
                previousWins[i].sprite = Imgs[PreviousWinValue[i]];
            }

            if(wheelNo == 0)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Zero ));
            }
            else if(wheelNo == 1)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.One ));
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.One_Four ));
            }
            else if(wheelNo == 2)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Two ));
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.One_Four ));
            }
            else if(wheelNo == 3)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Three ));
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.One_Four ));
            }
            else if(wheelNo == 4)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Four ));
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.One_Four ));
            }
            else if(wheelNo == 5)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Five ));
            }
            else if(wheelNo == 6)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Six ));
            }
            else if(wheelNo == 7)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Seven ));
            }
            else if(wheelNo == 8)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Eight ));
            }
            else if(wheelNo == 9)
            {
                StartCoroutine(ShowWinningRing( WinningRing[wheelNo], Spots.Nine ));
            }
        }

        /* Need to call below function of OnWin once api is integrated */

        // IEnumerator ShowWinningRing(GameObject ring, Spot winnerSpot, object o)
        // {
        //     yield return new WaitForSeconds(1f);
        //     WOF_BotsManager.Instance.UpdateBotData(o);
        //     WOF_OnlinePlayerBets.Intsance.OnWin(o);
        //     WOF_ChipController.Instance.TakeChipsBack(winnerSpot);
        //     yield return new WaitForSeconds(2f);
        //     ring.SetActive(false);
        //     if (winnerSpot == Spot.left)
        //     {
        //         StartCoroutine(DragonAnimation());
        //     }
        //     else if (winnerSpot == Spot.middle)
        //     {
        //         StartCoroutine(TieAnimation());
        //     }
        //     else if (winnerSpot == Spot.right)
        //     {
        //         StartCoroutine(TigerAnimation());
        //     }

        //     //BetManager.Instance.WinnerBets(winnerSpot);

        //     yield return new WaitForSeconds(2f);
        //     UtilitySound.Instance.Cardflipsound();
        //     Card1.sprite = TigerBackCard;           
        //     Card2.sprite = ElephantBackCard;

        // }
        IEnumerator DragonAnimation()
        {
            DragonWinner1.gameObject.SetActive(false);
            UtilitySound.Instance.DragonRoarsound();
            DragonWinner.gameObject.SetActive(true);
            foreach (var item in DragonWinnerAnimation)
            {
                DragonWinner.sprite = item;
                yield return new WaitForSeconds(0.03f);
            }
            DragonWinner.gameObject.SetActive(false);
            DragonWinner1.gameObject.SetActive(true);
        }
        IEnumerator TieAnimation()
        {
            UtilitySound.Instance.DragonRoarsound();
            TieWinner.gameObject.SetActive(true);
            foreach (var item in TieWinnerAnimation)
            {
                TieWinner.sprite = item;
                yield return new WaitForSeconds(0.1f);
            }
            TieWinner.gameObject.SetActive(false);
        }
        IEnumerator TigerAnimation()
        {
            UtilitySound.Instance.TigerRoarsound();
            TigerWinner1.gameObject.SetActive(false);
            TigerWinner.gameObject.SetActive(true);
            foreach (var item in TigerWinnerAnimation)
            {
                TigerWinner.sprite = item;
                yield return new WaitForSeconds(0.03f);
            }
            TigerWinner.gameObject.SetActive(false);
            TigerWinner1.gameObject.SetActive(true);
        }
    }

    [System.Serializable]
    public class Cards
    {
        public Sprite[] card;
    }
    public class DiceWinNos
    {
        public List<int> winNo;
        public List<int> wins;
        public List<int> previousWins;
        public int winningSpot;
    }
}
