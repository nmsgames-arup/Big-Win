using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using PokerKing.player;
using PokerKing.UI;
using PokerKing.Utility;
using KhushbuPlugin;
using Shared;

namespace PokerKing.Gameplay
{
    public class PokerKing_RoundWinningHandler : MonoBehaviour
    {
        public static PokerKing_RoundWinningHandler Instance;
        [SerializeField] GameObject LeftRing;
        [SerializeField] GameObject middleRing;
        [SerializeField] GameObject rightRing;
        [SerializeField] GameObject SpadeRing, ClubRing, DiamondRing, HeartRing, Spade_ClubRing, Diamond_HeartRing, JokerRing;
        public Sprite[] Imgs;
        public Image[] previousWins;
        public List<int> PreviousWinValue;
        public Action<object> onWin;
        public Sprite[] Card;
        public Image WinCard;
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
        public Cards[] CardsFrame;
        public Image CardsImage;
        [SerializeField]
        private Sprite faceSprite, backSprite;
        private bool coroutineAllowed, facedUp;
        int winNumber;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            PokerKing_Timer.Instance.onTimeUp += () => isTimeUp = true;
            PokerKing_Timer.Instance.onCountDownStart += () => isTimeUp = false;

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
            DiceWinNos winData = Utility.Utility.GetObjectOfType<DiceWinNos>(o);
            winNumber = winData.winningSpot;
            StartCoroutine(StartCardsAnimation());
        }

        IEnumerator StartCardsAnimation()
        {
            CardsImage.gameObject.SetActive(true);
            int random = UnityEngine.Random.Range(0,5);
            foreach (var item in CardsFrame[random].card)
            {
                CardsImage.sprite = item;
                yield return new WaitForSeconds(0.03f);
            }
            StopCardsAnimation();
        }

        void StopCardsAnimation()
        {
            StopCoroutine(StartCardsAnimation());
            CardsImage.sprite = CardsFrame[0].card[0];
            winFunction();
            coroutineAllowed = true;
            facedUp = false;
            StartCoroutine(RotateCard());
        }

        public void winFunction()
        {
            // winNumber = UnityEngine.Random.Range(0, 53);

            if(PreviousWinValue.Count >= 10)
            {
                PreviousWinValue.RemoveAt(0);
                PreviousWinValue.Add(winNumber);
            }
            else
            {
                PreviousWinValue.Add(winNumber);
            }
            for(int i = 0; i < PreviousWinValue.Count; i++)
            {
                previousWins[i].enabled = true;
                // Debug.LogError("name  " + previousWins[i].name + "     " + Card[PreviousWinValue[i]].name );
                previousWins[i].sprite = Card[PreviousWinValue[i]];
            }
            // WinCard.sprite = Card[winNumber];

            if (winNumber >= 0 && winNumber <= 12)
            {
                StartCoroutine(ShowWinningRing(SpadeRing, Spots.Spade ));
                StartCoroutine(ShowWinningRing(Spade_ClubRing, Spots.Spade_club ));
            }
            else if (winNumber >= 13 && winNumber <= 25)
            {
                StartCoroutine(ShowWinningRing(ClubRing, Spots.Club ));
                StartCoroutine(ShowWinningRing(Spade_ClubRing, Spots.Spade_club ));
            }
            else if(winNumber >= 26 && winNumber <= 38)
            {
                StartCoroutine(ShowWinningRing(DiamondRing, Spots.Diamond ));
                StartCoroutine(ShowWinningRing(Diamond_HeartRing, Spots.Diamond_Heart ));
            }
            else if(winNumber >= 39 && winNumber <= 51)
            {
                StartCoroutine(ShowWinningRing(HeartRing, Spots.Heart ));
                StartCoroutine(ShowWinningRing(Diamond_HeartRing, Spots.Diamond_Heart ));
            }
        }

        private IEnumerator RotateCard()
        {
            coroutineAllowed = false;

            if (!facedUp)
            {
                WinCard.enabled = true;
                // WinCard.rectTransform.sizeDelta = new Vector2(165.0f, 220.0f);
                // for (float i = 0f; i <= 180f; i += 10f)
                // {
                //     transform.rotation = Quaternion.Euler(0f, i, 0f);
                //     if (i == 90f)
                //     {
                        // CardsImage.sprite = faceSprite;
                        WinCard.GetComponent<Animation>().Play("WinnerCardOpen");
                        WinCard.sprite = Card[winNumber];
                    // }
                    yield return new WaitForSeconds(0.01f);
                // }
            }

            else if (facedUp)
            {
                // WinCard.rectTransform.sizeDelta = new Vector2(600.0f, 500.0f);
                // for (float i = 180f; i >= 0f; i -= 10f)
                // {
                //     transform.rotation = Quaternion.Euler(0f, i, 0f);
                //     if (i == 90f)
                //     {
                        WinCard.GetComponent<Animation>().Play("WinnerCardClose");
                        WinCard.sprite = backSprite;
                    // }
                    yield return new WaitForSeconds(0.01f);
                    // WinCard.enabled = false;
                // }
            }

            coroutineAllowed = true;
        }

        /* Need to call below function of OnWin once api is integrated */
        // public void OnWin(object o)
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
            PokerKing_ChipController.Instance.TakeChipsBack(winnerSpot);
            yield return new WaitForSeconds(2f);
            ring.SetActive(false);
            facedUp = true;
            StartCoroutine(RotateCard());
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
