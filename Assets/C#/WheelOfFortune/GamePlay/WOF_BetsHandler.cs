using WOF.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace WOF.Gameplay
{
    /// <summary>
    /// this class will handle all the bots bets
    /// </summary>
    class WOF_BetsHandler : MonoBehaviour
    {
        public static WOF_BetsHandler Instance;
        bool isTimeUp;
        public WOF_Bot[] Bots;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            WOF_Timer.Instance.onTimeUp += () => isTimeUp = true;
            WOF_Timer.Instance.onCountDownStart += () => isTimeUp = false;
            //Bots = botsGameobject.GetComponentsInChildren<Bot>(true);
        }

        /// <summary>
        /// iterate through each bota and apply data from scriptable object
        /// it will help bots to achive realistic behaviour
        /// this function will call from SeverResponse.cs file
        /// whenever server sends any data regarding the bots
        /// </summary>
        /// <param name="data"></param>
        public void AddBotsData(object data)
        {
            if (isTimeUp || WOF_Timer.Instance.is_a_FirstRound) return;
            CurrentRoundChipsData bots = Utility.Utility.GetObjectOfType<CurrentRoundChipsData>(data);
            
            //this will only emite chips from one spot thats the spot which is 
            //present at the beginning 
            foreach (var item in bots.onlinePlayersBets)
            {
                WOF_OnlinePlayerBets.Intsance.ChipCreator(item);
            }
            foreach (var bot in bots.botsBets)
            {
                Bots[bot.botIndex].ChipCreator(bot.dataIndex);
            }
        }
    }
    public class CurrentRoundChipsData
    {
        public List<int> onlinePlayersBets;
        public BotsData[] botsBets;
    }

    public class BotsData
    {
        public int botIndex;
        public int dataIndex;
    }
}