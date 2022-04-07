using Dragon.Utility;
using KhushbuPlugin;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Dragon.Gameplay
{

    class RoundWinningHandler : MonoBehaviour
    {
        public static RoundWinningHandler Instance;
        [SerializeField] GameObject leftRing;
        [SerializeField] GameObject middleRing;
        [SerializeField] GameObject rightRing;
        public Sprite[] Imgs;
        public Image[] previousWins;
        public List<int> PreviousWinValue;
        public Action<object> onWin;
        public Cards[] Card;
        public Sprite[] Cards_Frames;
        public Image Card1;
        public Sprite[] Card1_Frames;
        public Image Card2;
        public Sprite[] Card1_Highlight_Frames;
        public Image Card1_Highlight;
        public Sprite[] Card2_Highlight_Frames;
        public Image Card2_Highlight;
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

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            Timer.Instance.onTimeUp += () => isTimeUp = true;
            Timer.Instance.onCountDownStart += () => isTimeUp = false;

            onWin += OnWin;
            //leftDice.SetActive(false);
            //rightDice.SetActive(false);       
        }

        public IEnumerator StartCard1_Animation()
        {
            Card1.gameObject.SetActive(true);
            foreach (var item in Cards_Frames)
            {
                Card1.sprite = item;
                yield return new WaitForSeconds(0.04f);
            }
            StopCard1_Animation();
        }

        public IEnumerator StartCard2_Animation()
        {
            Card2.gameObject.SetActive(true);
            foreach (var item in Cards_Frames)
            {
                Card2.sprite = item;
                yield return new WaitForSeconds(0.04f);
            }
            StopCard2_Animation();
        }

        public void StopCard1_Animation()
        {
            StopCoroutine(StartCard1_Animation());
        }

        public void StopCard2_Animation()
        {
            StopCoroutine(StartCard2_Animation());
        }

        public IEnumerator StartCard1_Highlight_Animation()
        {
            Card1_Highlight.gameObject.SetActive(true);
            foreach (var item in Card1_Highlight_Frames)
            {
                Card1_Highlight.sprite = item;
                yield return new WaitForSeconds(0.06f);
            }
            StopCard1_Highlight_Animation();
        }
        public void StopCard1_Highlight_Animation()
        {
            StopCoroutine(StartCard1_Highlight_Animation());
            Card1_Highlight.gameObject.SetActive(false);
        }

        public IEnumerator StartCard2_Highlight_Animation()
        {
            Card2_Highlight.gameObject.SetActive(true);
            foreach (var item in Card2_Highlight_Frames)
            {
                Card2_Highlight.sprite = item;
                yield return new WaitForSeconds(0.06f);
            }
            StopCard2_Highlight_Animation();
        }
        public void StopCard2_Highlight_Animation()
        {
            StopCoroutine(StartCard2_Highlight_Animation());
            Card2_Highlight.gameObject.SetActive(false);
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
            if (Timer.Instance.is_a_FirstRound) return;
            DiceWinNos winData = Utility.Utility.GetObjectOfType<DiceWinNos>(o);
            int[] turnArray = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3 };
            System.Random rands = new System.Random();
            List<int> rand = turnArray.OrderBy(c => rands.Next()).Select(c => c).ToList();
            int No1 = winData.winNo[0] - 1;
            int No2 = winData.winNo[1] - 1;
            StartCoroutine(cardOpen(No1, No2, rand[0], rand[1]));

            // for (int i = 0; i < winData.previousWins.Count; i++)
            // {
            //     int num = winData.previousWins[i];
            //     if (num == 0)
            //     {
            //         previousWins[i].sprite = Imgs[0];//dragon
            //     }
            //     else if (num == 1)
            //     {
            //         previousWins[i].sprite = Imgs[1];//tie
            //     }
            //     else
            //     {
            //         previousWins[i].sprite = Imgs[2];//tiger
            //     }                
            // }
            if (winData.winningSpot == 0)//aniamtion add khusi
            {
                randNo = 0;
                StartCoroutine(ShowWinningRing(leftRing, Spot.left, o));//dragon
            }
            else if (winData.winningSpot == 1)
            {
                randNo = 1;
                StartCoroutine(ShowWinningRing(middleRing, Spot.middle, o));
            }
            else
            {
                randNo = 2;
                StartCoroutine(ShowWinningRing(rightRing, Spot.right, o));
            }

            if(PreviousWinValue.Count >= 14)
            {
                PreviousWinValue.RemoveAt(0);
                PreviousWinValue.Add(randNo);
            }
            else
            {
                PreviousWinValue.Add(randNo);
            }
            for(int i = 0;i < PreviousWinValue.Count; i++)
            {
                previousWins[i].enabled = true;
                if(PreviousWinValue[i] == 0)
                {
                    previousWins[i].sprite = Imgs[0];//Tiger win
                }
                else if(PreviousWinValue[i] == 1)
                {
                    previousWins[i].sprite = Imgs[1];//Tie win
                }
                else
                {
                    previousWins[i].sprite = Imgs[2];//Elephant win
                }
            }

        }
        IEnumerator cardOpen(int No1,int No2, int rand0, int rand1)
        {
            StartCoroutine(StartCard1_Animation());
            StartCoroutine(StartCard2_Animation());
            yield return new WaitForSeconds(2.0f);
            Card1.GetComponent<RectTransform>().sizeDelta = new Vector2(234.0f, 300.0f);
            Card2.GetComponent<RectTransform>().sizeDelta = new Vector2(234.0f, 300.0f);
            Card1.sprite = Card[rand0].card[No1];
            StartCoroutine(StartCard1_Highlight_Animation());
            yield return new WaitForSeconds(0.5f);
            Card2.sprite = Card[rand1].card[No2];
            StartCoroutine(StartCard2_Highlight_Animation());
        }
        IEnumerator ShowWinningRing(GameObject ring, Spot winnerSpot, object o)
        {
            yield return new WaitForSeconds(3.5f);

            ring.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            ring.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            ring.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            ring.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            ring.SetActive(true);

            BotsManager.Instance.UpdateBotData(o);
            OnlinePlayerBets.Intsance.OnWin(o);
            ChipController.Instance.TakeChipsBack(winnerSpot);
            yield return new WaitForSeconds(2f);
            ring.SetActive(false);           
            // if (winnerSpot == Spot.left)
            // {
            //     StartCoroutine(DragonAnimation());
            // }
            // else if (winnerSpot == Spot.middle)
            // {
            //     StartCoroutine(TieAnimation());
            // }
            // else if (winnerSpot == Spot.right)
            // {
            //     StartCoroutine(TigerAnimation());
            // }

            //BetManager.Instance.WinnerBets(winnerSpot);

            yield return new WaitForSeconds(2f);
            UtilitySound.Instance.Cardflipsound();
            Card1.sprite = TigerBackCard;
            Card2.sprite = ElephantBackCard;
            Card1.GetComponent<RectTransform>().sizeDelta = new Vector2(500.0f, 500.0f);
            Card2.GetComponent<RectTransform>().sizeDelta = new Vector2(500.0f, 500.0f);

        }
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