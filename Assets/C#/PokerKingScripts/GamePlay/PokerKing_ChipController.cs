using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;
using KhushbuPlugin;
using PokerKing.UI;
using PokerKing.player;
using PokerKing.ServerStuff;
using PokerKing.Utility;

namespace PokerKing.Gameplay
{
    public class PokerKing_ChipController : MonoBehaviour
    {
        public static PokerKing_ChipController Instance;

        [SerializeField] iTween.EaseType easeType;

        [SerializeField] Transform chipSecondLastSpot;
        [SerializeField] Transform chipLastSpot;

        /// <summary>
        /// the following types of parents is representing
        /// where the chip will be present after it 
        /// the movement
        /// </summary>
        public Transform SpadeParent, ClubParent, DiamondParent, HeartParent, Spade_ClubParent, Diamond_HeartParent, JokerParent;
        public GameObject LeftChipPos, RightChipPos, MiddleChipPos;

        [SerializeField] float chipMoveTime;
        public Action<Transform, Vector3> OnUserInput;
        public Action<Vector3, Chip, int, Spots> OnServerResponse;

        public Dictionary<Spots, Transform> chipHolder = new Dictionary<Spots, Transform>();
        public AudioClip AddChip, CoinGenerate;
        public AudioSource CoinMove_AudioSource, GameAudio;
        public PokerKing_BetManager betManager;
        public BetSpots betSpots;
        bool isTimeUp;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            OnUserInput += AddUserBet;
            OnServerResponse += CreateChipForOtherPlayers;
            chipHolder.Add(Spots.Spade, SpadeParent);
            chipHolder.Add(Spots.Club, ClubParent);
            chipHolder.Add(Spots.Diamond, DiamondParent);
            chipHolder.Add(Spots.Heart, HeartParent);
            chipHolder.Add(Spots.Spade_club, Spade_ClubParent);
            chipHolder.Add(Spots.Diamond_Heart, Diamond_HeartParent);
            chipHolder.Add(Spots.Joker, JokerParent);
            PokerKing_Timer.Instance.onTimeUp += () => isTimeUp = true;
            PokerKing_Timer.Instance.onCountDownStart += () => isTimeUp = false;
        }

        void AddUserBet(Transform bettingSpot, Vector3 target)
        {
            if (isTimeUp) return;
            if (!PokerKing_UiHandler.Instance.IsEnoughBalancePresent()) return;
            Chip chip = PokerKing_UiHandler.Instance.currentChip;
            Spots spot = bettingSpot.GetComponent<Utility.PokerKing_BettingSpot>().spotType;
            PokerKing_UiHandler.Instance.AddBets(spot);
            PokerKing_ServerRequest.instance.OnChipMove(target, chip, spot);
            CreateChip(target, chip, spot, 0);
        }

        void CreateChip(Vector3 target, Chip chip, Spots spot, int spawnNo)
        {
            switch(spot)
            {
                case Spots.Spade:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.3f , LeftChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.4f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = LeftChipPos.transform.position.y;
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    break;
                case Spots.Club:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.3f , MiddleChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.4f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = MiddleChipPos.transform.position.y;
                    target.z = MiddleChipPos.transform.position.z;
                    // target = MiddleChipPos.transform.position;
                    break;
                case Spots.Diamond:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                case Spots.Heart:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                case Spots.Spade_club:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                case Spots.Diamond_Heart:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                case Spots.Joker:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                default:
                    break;
            }
            PokerKing_ServerRequest.instance.OnChipMove(target, chip, spot);
            betManager.AddBets(spot, PokerKing_UiHandler.Instance.currentChip);
            PokerKing_UiHandler.Instance.UpDateBets(spot, chip);
            // GameObject chipInstance = WOF_ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            GameObject chipInstance = PokerKing_ChipSpawner.Instance.Spawn(7, chip, GetChipParent(spot));
            StartCoroutine(MoveChip(chipInstance, target));
        }
        public void AddData(Spot spot, Vector3 target)
        {
            int ind = UnityEngine.Random.Range(1,6);
            ChipDate chipData = new ChipDate
            {
                chip = PokerKing_UiHandler.Instance.currentChip,
                spot = spot,
                target = target,
                spawnNo = ind
            };
            GameObject chipInstance = PokerKing_ChipSpawner.Instance.Spawn(0, PokerKing_UiHandler.Instance.currentChip, this.transform);
            //StartCoroutine(MoveChip(chipInstance, target));
            betSpots.AddData(chipData);
        }
        Transform GetChipParent(Spots betType)
        {
            switch (betType)
            {
                case Spots.Spade: return SpadeParent;
                case Spots.Club: return ClubParent;
                case Spots.Diamond: return DiamondParent;
                case Spots.Heart: return HeartParent;
                case Spots.Spade_club: return Spade_ClubParent;
                case Spots.Diamond_Heart: return Diamond_HeartParent;
                case Spots.Joker: return JokerParent;
            }
            return null;
        }
        void CreateChipForOtherPlayers(Vector3 target, Chip chip, int spanNo, Spots spot)
        {
            GameObject chipInstance = PokerKing_ChipSpawner.Instance.Spawn(spanNo, chip, GetChipParent(spot));
            PokerKing_UiHandler.Instance.UpDateBets(spot, chip);
            switch (spot)
            {
                case Spots.Spade:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.3f , LeftChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.4f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = LeftChipPos.transform.position.y;
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    break;
                case Spots.Club:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.3f , MiddleChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.4f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = MiddleChipPos.transform.position.y;
                    target.z = MiddleChipPos.transform.position.z;
                    // target = MiddleChipPos.transform.position;
                    break;
                case Spots.Diamond:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                case Spots.Heart:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                case Spots.Spade_club:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                case Spots.Diamond_Heart:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                case Spots.Joker:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    target.z = RightChipPos.transform.position.z;
                    break;
                default:
                    break;
            }
            StartCoroutine(MoveChip(chipInstance, target));

        }
        float chipMovetime = .25f;
        IEnumerator MoveChip(GameObject chip, Vector3 target)
        {
            iTween.MoveTo(chip, iTween.Hash("position", target, "time", chipMovetime, "easetype", easeType));
            //var rotat = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 180));
            //iTween.RotateBy(gameObject, iTween.Hash("z", UnityEngine.Random.Range(0, 180), "easeType", "easeInOutBack", "loopType", "pingPong", "delay", chipMovetime));
            //iTween.RotateTo(chip, iTween.Hash("rotation", rotat, "time", chipMovetime, "easetype", easeType));
            yield return new WaitForSeconds(chipMovetime);
            StartCoroutine(PlayAudioClip());
            // UtilitySound.Instance.addchipsound();
            iTween.PunchScale(chip, iTween.Hash("x", .3, "y", 0.3f, "default", .1));
        }

        public IEnumerator PlayAudioClip()
        {
            yield return new WaitForSeconds(0.1f);
            CoinMove_AudioSource.clip = AddChip;
            CoinMove_AudioSource.Play();
            // CoinMove_AudioSource.Stop();
        }
        public void OnOtherPlayerMove(object data)
        {
            if (isTimeUp) return;
            OnChipMove chip = Utility.Utility.GetObjectOfType<OnChipMove>(data);
            CreateChipForOtherPlayers(chip.position, chip.chip, 0, chip.spot);
        }
        public Button test;
        float time = .5f;
        float moveTime = 1f;

        public void TakeChipsBack(Spots winner)
        {
            StartCoroutine(DestroyChips(winner));
        }
        IEnumerator DestroyChips(Spots winnerSpot)
        {
            foreach (var item in chipHolder)
            {
                if (item.Key == winnerSpot) continue;
                foreach (Transform child in item.Value)
                {
                    StartCoroutine(MoveChips(child, chipSecondLastSpot));
                }
            }
            yield return new WaitForSeconds(1);
            foreach (Transform child in chipHolder[winnerSpot])
            {
                StartCoroutine(MoveChips(child, chipLastSpot));
            }
            PokerKing_UiHandler.Instance.ResetUi();

        }
        public float speed = 1f;
        public float waitTime = 0.85f;
        IEnumerator MoveChips(Transform chip, Transform destinatio)
        {
            iTween.MoveTo(chip.gameObject, iTween.Hash("position", destinatio.position, "time", chipMoveTime, "easetype", easeType));
            yield return new WaitForSeconds(waitTime);
            Destroy(chip.gameObject);
        }
        public void CreateBotsChips(ChipDate chipData, Spots spots, int spawnNo)
        {
            if (isTimeUp) return;

            var chip = chipData.chip;
            var spot = chipData.spot;
            var spanNo = chipData.spawnNo;
            var target = chipData.target;
            PokerKing_UiHandler.Instance.UpDateBets(spots, chip);
            GameObject chipInstance = PokerKing_ChipSpawner.Instance.Spawn(spawnNo, chip, GetChipParent(spots));
            switch (spots)
            {
                case Spots.Spade:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.3f , LeftChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.4f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = LeftChipPos.transform.position.y;
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    // target = new Vector3(UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().rect.xMin, LeftChipPos.GetComponent<RectTransform>().rect.xMax), UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().rect.yMin, LeftChipPos.GetComponent<RectTransform>().rect.yMax), 0 );
                    break;
                case Spots.Club:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.3f , MiddleChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.4f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = MiddleChipPos.transform.position.y;
                    target.z = MiddleChipPos.transform.position.z;
                    // target = MiddleChipPos.transform.position;
                    break;
                case Spots.Diamond:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = RightChipPos.transform.position.y;
                    target.z = RightChipPos.transform.position.z;
                    // target = RightChipPos.transform.position;
                    break;
                case Spots.Heart:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = RightChipPos.transform.position.y;
                    target.z = RightChipPos.transform.position.z;
                    // target = RightChipPos.transform.position;
                    break;
                case Spots.Spade_club:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = RightChipPos.transform.position.y;
                    target.z = RightChipPos.transform.position.z;
                    // target = RightChipPos.transform.position;
                    break;
                case Spots.Diamond_Heart:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = RightChipPos.transform.position.y;
                    target.z = RightChipPos.transform.position.z;
                    // target = RightChipPos.transform.position;
                    break;
                case Spots.Joker:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = RightChipPos.transform.position.y;
                    target.z = RightChipPos.transform.position.z;
                    // target = RightChipPos.transform.position;
                    break;
                default:
                    break;
            }
            StartCoroutine(MoveChip(chipInstance, target));

        }
        public void CreatePlayerChip(ChipDate chipData)
        {
            if (isTimeUp) return;
            if (!PokerKing_UiHandler.Instance.IsEnoughBalancePresent()) return;
            print("create chip");
            Chip chip = chipData.chip;
            Spot spot = chipData.spot;
            // PokerKing_UiHandler.Instance.AddBets(spot);
            float XPos =  UnityEngine.Random.Range(LeftChipPos.transform.position.x - 100.0f , LeftChipPos.transform.position.x + 100.0f);
            switch (spot)
            {
                case Spot.left:
                    chipData.target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.3f , LeftChipPos.transform.position.x + 0.3f);
                    chipData.target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.4f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // chipData.target.y = LeftChipPos.transform.position.y;
                    chipData.target.z = LeftChipPos.transform.position.z;
                    // chipData.target = LeftChipPos.transform.position;
                    break;
                case Spot.middle:
                    chipData.target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.3f , MiddleChipPos.transform.position.x + 0.3f);
                    chipData.target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.4f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // chipData.target.y = MiddleChipPos.transform.position.y;
                    chipData.target.z = MiddleChipPos.transform.position.z;
                    // chipData.target = MiddleChipPos.transform.position;
                    break;
                case Spot.right:
                    chipData.target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    chipData.target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // chipData.target.y = RightChipPos.transform.position.y;
                    chipData.target.z = RightChipPos.transform.position.z;
                    // chipData.target = RightChipPos.transform.position;
                    break;
                default:
                    break;
            }
            // PokerKing_ServerRequest.instance.OnChipMove(chipData.target, chip, spot);
            // betManager.AddBets(spot, PokerKing_UiHandler.Instance.currentChip);
            PokerKing_MainPlayer.Instance.totalBet += (int)PokerKing_UiHandler.Instance.currentChip;
            //WOF_UiHandler.Instance.BetCoinTxt.text = MainPlayer.Instance.totalBet.ToString();
        //    PokerKing_UiHandler.Instance.UpDateBets(spot, chip);
            // GameObject chipInstance = PokerKing_ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            // StartCoroutine(MoveChip(chipInstance, chipData.target));
        }
    }
}
