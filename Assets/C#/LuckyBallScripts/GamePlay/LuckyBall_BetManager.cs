using System.Collections.Generic;
using UnityEngine;
using LuckyBall.Utility;
using Shared;

namespace LuckyBall.Gameplay
{
    #if ENABLE_WEBSOCKET_CLIENT
    #endif
    public class LuckyBall_BetManager : MonoBehaviour
    {
        public static LuckyBall_BetManager Instance;
        Dictionary<Spots, int> betHolder = new Dictionary<Spots, int>();
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            betHolder.Add(Spots.Zero, 0);
            betHolder.Add(Spots.One, 0);
            betHolder.Add(Spots.Two, 0);
            betHolder.Add(Spots.Three, 0);
            betHolder.Add(Spots.Four, 0);
            betHolder.Add(Spots.Five, 0);
            betHolder.Add(Spots.Six, 0);
            betHolder.Add(Spots.Seven, 0);
            betHolder.Add(Spots.Eight, 0);
            betHolder.Add(Spots.Nine, 0);
            betHolder.Add(Spots.One_Four, 0);
            LuckyBall_Timer.Instance.onTimeUp += PostBets;
            LuckyBall_Timer.Instance.onCountDownStart += ClearBet;
        }
        void PostBets()
        {
        }
        void ClearBet()
        {
            betHolder[Spots.Zero] = 0;
            betHolder[Spots.One] = 0;
            betHolder[Spots.Two] = 0;
            betHolder[Spots.Three] = 0;
            betHolder[Spots.Four] = 0;
            betHolder[Spots.Five] = 0;
            betHolder[Spots.Six] = 0;
            betHolder[Spots.Seven] = 0;
            betHolder[Spots.Eight] = 0;
            betHolder[Spots.Nine] = 0;
            betHolder[Spots.One_Four] = 0;
        }
        public void AddBets(Spots betType, Chip chipType)
        {
            betHolder[betType] = GetBetAmount(chipType);
        }

        private int GetBetAmount(Chip chipType)
        {
            int amount = 0;
            switch (chipType)
            {
                case Chip.Chip2:
                    amount = 2;
                    break;
                case Chip.Chip10:
                    amount = 10;
                    break;
                case Chip.Chip50:
                    amount = 50;
                    break;
                case Chip.Chip100:
                    amount = 100;
                    break;
                case Chip.Chip500:
                    amount = 500;
                    break;
                case Chip.Chip1000:
                    amount = 1000;
                    break;
                case Chip.Chip5000:
                    amount = 5000;
                    break;
                default:
                    break;
            }
            return amount;
        }
    }

    public class Bet
    {
        public string user_Id;
    }
}
