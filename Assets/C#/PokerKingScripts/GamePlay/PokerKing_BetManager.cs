using System.Collections.Generic;
using UnityEngine;
using PokerKing.Utility;
using Shared;

namespace PokerKing.Gameplay
{
    #if ENABLE_WEBSOCKET_CLIENT
    #endif
    public class PokerKing_BetManager : MonoBehaviour
    {
        public static PokerKing_BetManager Instance;
        Dictionary<Spots, int> betHolder = new Dictionary<Spots, int>();
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            betHolder.Add(Spots.Spade, 0);
            betHolder.Add(Spots.Club, 0);
            betHolder.Add(Spots.Diamond, 0);
            betHolder.Add(Spots.Heart, 0);
            betHolder.Add(Spots.Spade_club, 0);
            betHolder.Add(Spots.Diamond_Heart, 0);
            betHolder.Add(Spots.Joker, 0);
            PokerKing_Timer.Instance.onTimeUp += PostBets;
            PokerKing_Timer.Instance.onCountDownStart += ClearBet;
        }
        void PostBets()
        {
        }
        void ClearBet()
        {
            betHolder[Spots.Spade] = 0;
            betHolder[Spots.Club] = 0;
            betHolder[Spots.Diamond] = 0;
            betHolder[Spots.Heart] = 0;
            betHolder[Spots.Spade_club] = 0;
            betHolder[Spots.Diamond_Heart] = 0;
            betHolder[Spots.Joker] = 0;
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
}
