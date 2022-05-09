using SocketIO;
using System.Collections;
using UnityEngine;
using Shared;
using PokerKing.Utility;

namespace PokerKing.ServerStuff
{
    public class PokerKing_ServerRequest : PokerKing_SocketHandler
    {
        public static PokerKing_ServerRequest instance;
        public void Awake()
        {
            socket = GameObject.Find("SocketIOComponents").GetComponent<SocketIOComponent>();
            instance = this;

        }
        public void JoinGame()
        {
            Debug.Log($"player { UserDetail.UserId.ToString()} Join game");
            Player player = new Player()
            {
                balance = LocalPlayer.balance,
                playerId = UserDetail.UserId.ToString(),
                profilePic = LocalPlayer.profilePic,
                gameId = "2"
            };
            socket.Emit(Events.RegisterPlayer, new JSONObject(JsonUtility.ToJson(player)));
        }

        public void OnChipMove(Vector3 position, Chip chip, Spots spot)
        {
            OnChipMove Obj = new OnChipMove()
            {
                position = position,
                playerId = UserDetail.UserId.ToString(),
                chip = chip,
                spot = spot
            };
            socket.Emit(Events.OnChipMove, new JSONObject(JsonUtility.ToJson(Obj)));
        }
        public void OnTest()
        {
            socket.Emit(Events.OnTest);
        }       
        public void OnHistoryRecordGame()
        {
            socket.Emit(Events.OnHistoryRecord);
        }
    }
}
