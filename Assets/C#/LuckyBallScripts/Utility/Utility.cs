using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Shared;

namespace LuckyBall.Utility
{
    public static class Utility
    {
        public static string DOMAIN;
        public static string ON_CHIP_MOVE = DOMAIN + "";
        public static string JOIN_GAME = DOMAIN + "";
        public static string ADD_PLAYER = DOMAIN + "";
        public static string ON_TIME_UP = DOMAIN + "";
        public static string ON_COUNTDOWN_START = DOMAIN + "";
        public static string ON_GAME_START = DOMAIN + "";
        public static string ON_PLAYER_EXIT = DOMAIN + "";

        public static string playerId;
        public static T GetObjectOfType<T>(object json) where T : class
        {
            T t = null;
            try
            {
                t = JsonConvert.DeserializeObject<T>(json.ToString());
                return t;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return t;
        }
    };

    public class OnChipMove
    {
        public string playerId;
        public Chip chip;
        public Spots spot;
        public Vector3 position;
    }
    public class CurrentGameData
    {
        public string  playerId;
        public Chip    chip;
        public Spot    spot;
        public Vector3 position;
    }

    [Serializable]
    public class Bot
    {
        public Spots spot;
        public Chip chip;
        public Vector3 position;
    }

    public static class Events
    {
        internal static string OnChipMove = "OnChipMove";
        internal static string OnTest = "test";
        internal static string OnPlayerExit = "OnPlayerExit";
        internal static string OnJoinRoom = "OnJoinRoom";
        internal static string OnTimeUp = "OnTimeUp";
        internal static string OnWait = "OnWait";
        internal static string OnTimerStart = "OnTimerStart";
        internal static string OnDrawCompleted = "OnDrawCompleted";
        internal static string RegisterPlayer = "RegisterPlayer";
        internal static string OnGameStart= "OnGameStart";
        internal static string OnAddNewPlayer= "OnAddNewPlayer";
        internal static string OnCurrentTimer = "OnCurrentTimer";
        internal static string OnBetsPlaced = "OnBetsPlaced";
        internal static string OnWinNo = "OnWinNo";
        internal static string OnBotsData = "OnBotsData";
        internal static string OnPlayerWin = "OnPlayerWin";
        internal static string onleaveRoom = "onleaveRoom";
        internal static string OnHistoryRecord = "OnHistoryRecord";
    }
    public enum Spots
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        One_Four = 10,
        Zero_Five = 11,
        Six_Nine = 12
    }
}