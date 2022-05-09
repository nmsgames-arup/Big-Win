using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;
using LuckyBall.ServerStuff;
using LuckyBall.Utility;
using KhushbuPlugin;
using LuckyBall.UI;
using LuckyBall.player;

namespace LuckyBall.Gameplay
{
    public class LuckyBall_ChipController : MonoBehaviour
    {
        public static LuckyBall_ChipController Instance;

        [SerializeField] iTween.EaseType easeType;

        [SerializeField] Transform chipSecondLastSpot;
        [SerializeField] Transform chipLastSpot;

        /// <summary>
        /// the following types of parents is representing
        /// where the chip will be present after it 
        /// the movement
        /// </summary>
        public Transform ZeroParent, OneParent, TwoParent, ThreeParent, FourParent, 
        FiveParent, SixParent, SevenParent, EightParent, NineParent, One_FourParent, Zero_FiveParent, Six_NineParent;
        public Transform rightParent;
        public Transform middleParent;
        public GameObject LeftChipPos, RightChipPos, MiddleChipPos;
        public List<GameObject> ChipPositionObj;

        [SerializeField] float chipMoveTime;
        public Action<Transform, Vector3> OnUserInput;
        public Action<Vector3, Chip, int, Spots> OnServerResponse;

        public Dictionary<Spots, Transform> chipHolder = new Dictionary<Spots, Transform>();
        public AudioClip AddChip, CoinGenerate;
        public AudioSource CoinMove_AudioSource, CoinGenerate_AudioSource;
        bool isTimeUp;
        public LuckyBall_BetManager betManager;
        public BetSpots betSpots;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            OnUserInput += AddUserBet;
            OnServerResponse += CreateChipForOtherPlayers;
            chipHolder.Add(Spots.Zero, ZeroParent);
            chipHolder.Add(Spots.One, OneParent);
            chipHolder.Add(Spots.Two, TwoParent);
            chipHolder.Add(Spots.Three, ThreeParent);
            chipHolder.Add(Spots.Four, FourParent);
            chipHolder.Add(Spots.Five, FiveParent);
            chipHolder.Add(Spots.Six, SixParent);
            chipHolder.Add(Spots.Seven, SevenParent);
            chipHolder.Add(Spots.Eight, EightParent);
            chipHolder.Add(Spots.Nine, NineParent);
            chipHolder.Add(Spots.One_Four, One_FourParent);
            chipHolder.Add(Spots.Zero_Five, Zero_FiveParent);
            chipHolder.Add(Spots.Six_Nine, Six_NineParent);
            LuckyBall_Timer.Instance.onTimeUp += () => isTimeUp = true;
            LuckyBall_Timer.Instance.onCountDownStart += () => isTimeUp = false;
        }

        void AddUserBet(Transform bettingSpot, Vector3 target)
        {
            if (isTimeUp) return;
            if (!LuckyBall_UiHandler.Instance.IsEnoughBalancePresent()) return;
            Chip chip = LuckyBall_UiHandler.Instance.currentChip;
            Spots spot = bettingSpot.GetComponent<Utility.LuckyBall_BettingSpot>().spotType;
            LuckyBall_UiHandler.Instance.AddBets(spot);
            LuckyBall_ServerRequest.instance.OnChipMove(target, chip, spot);
            CreateChip(target, chip, spot, 0);
        }

        public void CreateChip(Vector3 target, Chip chip, Spots spot, int spawnNo)
        {
            switch (spot)
            {
                case Spots.Zero:
                    int _zeroVal = UnityEngine.Random.Range(0, 2);
                    target.x = UnityEngine.Random.Range(ChipPositionObj[0].transform.position.x - 0.35f, ChipPositionObj[0].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[0].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[0].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[0].transform.position.z;
                    break;
                case Spots.One:
                    int _oneVal = UnityEngine.Random.Range(0, 2);
                    target.x = UnityEngine.Random.Range(ChipPositionObj[1].transform.position.x - 0.35f, ChipPositionObj[1].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[1].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[1].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[1].transform.position.z;
                    break;
                case Spots.Two:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[2].transform.position.x - 0.35f, ChipPositionObj[2].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[2].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[2].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[2].transform.position.z;
                    break;
                case Spots.Three:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[3].transform.position.x - 0.35f, ChipPositionObj[3].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[3].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[3].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[3].transform.position.z;
                    break;
                case Spots.Four:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[4].transform.position.x - 0.35f, ChipPositionObj[4].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[4].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[4].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[4].transform.position.z;
                    break;
                case Spots.Five:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[5].transform.position.x - 0.35f, ChipPositionObj[5].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[5].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[5].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[5].transform.position.z;
                    break;
                case Spots.Six:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[6].transform.position.x - 0.35f, ChipPositionObj[6].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[6].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[6].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[6].transform.position.z;
                    break;
                case Spots.Seven:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[7].transform.position.x - 0.35f, ChipPositionObj[7].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[7].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[7].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[7].transform.position.z;
                    break;
                case Spots.Eight:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[8].transform.position.x - 0.35f, ChipPositionObj[8].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[8].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[8].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[8].transform.position.z;
                    break;
                case Spots.Nine:
                    target = ChipPositionObj[9].transform.position;
                    break;
                case Spots.One_Four:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[10].transform.position.x - 0.35f, ChipPositionObj[10].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[10].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[10].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[10].transform.position.z;
                    break;
                case Spots.Zero_Five:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[11].transform.position.x - 0.35f, ChipPositionObj[11].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[11].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[11].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[11].transform.position.z;
                    break;
                case Spots.Six_Nine:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[12].transform.position.x - 0.35f, ChipPositionObj[12].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[12].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[12].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[12].transform.position.z;
                    break;
                default:
                    break;
            }
            LuckyBall_ServerRequest.instance.OnChipMove(target, chip, spot);
            betManager.AddBets(spot, LuckyBall_UiHandler.Instance.currentChip);
            LuckyBall_UiHandler.Instance.UpDateBets(spot, chip);
            // GameObject chipInstance = WOF_ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            GameObject chipInstance = LuckyBall_ChipSpawner.Instance.Spawn(7, chip, GetChipParent(spot));
            StartCoroutine(MoveChip(chipInstance, target));
        }
        public void AddData(Spot spot, Vector3 target)
        {
            int ind = UnityEngine.Random.Range(1,6);
            ChipDate chipData = new ChipDate
            {
                chip = LuckyBall_UiHandler.Instance.currentChip,
                spot = spot,
                target = target,
                spawnNo = ind
            };
            GameObject chipInstance = LuckyBall_ChipSpawner.Instance.Spawn(0, LuckyBall_UiHandler.Instance.currentChip, this.transform);
            //StartCoroutine(MoveChip(chipInstance, target));
            betSpots.AddData(chipData);
        }
        Transform GetChipParent(Spots betType)
        {
            switch (betType)
            {
                case Spots.Zero: return ZeroParent;
                case Spots.One: return OneParent;
                case Spots.Two: return TwoParent;
                case Spots.Three: return ThreeParent;
                case Spots.Four: return FourParent;
                case Spots.Five: return FiveParent;
                case Spots.Six: return SixParent;
                case Spots.Seven: return SevenParent;
                case Spots.Eight: return EightParent;
                case Spots.Nine: return NineParent;
                case Spots.One_Four: return One_FourParent;
                case Spots.Zero_Five: return Zero_FiveParent;
                case Spots.Six_Nine: return Six_NineParent;
            }
            return null;
        }
        void CreateChipForOtherPlayers(Vector3 target, Chip chip, int spanNo, Spots spot)
        {
            GameObject chipInstance = LuckyBall_ChipSpawner.Instance.Spawn(spanNo, chip, GetChipParent(spot));
            LuckyBall_UiHandler.Instance.UpDateBets(spot, chip);
            switch (spot)
            {
                case Spots.Zero:
                    int _zeroVal = UnityEngine.Random.Range(0, 2);
                    target.x = UnityEngine.Random.Range(ChipPositionObj[0].transform.position.x - 0.35f, ChipPositionObj[0].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[0].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[0].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[0].transform.position.z;
                    break;
                case Spots.One:
                    int _oneVal = UnityEngine.Random.Range(0, 2);
                    target.x = UnityEngine.Random.Range(ChipPositionObj[1].transform.position.x - 0.35f, ChipPositionObj[1].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[1].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[1].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[1].transform.position.z;
                    break;
                case Spots.Two:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[2].transform.position.x - 0.35f, ChipPositionObj[2].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[2].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[2].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[2].transform.position.z;
                    break;
                case Spots.Three:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[3].transform.position.x - 0.35f, ChipPositionObj[3].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[3].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[3].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[3].transform.position.z;
                    break;
                case Spots.Four:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[4].transform.position.x - 0.35f, ChipPositionObj[4].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[4].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[4].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[4].transform.position.z;
                    break;
                case Spots.Five:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[5].transform.position.x - 0.35f, ChipPositionObj[5].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[5].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[5].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[5].transform.position.z;
                    break;
                case Spots.Six:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[6].transform.position.x - 0.35f, ChipPositionObj[6].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[6].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[6].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[6].transform.position.z;
                    break;
                case Spots.Seven:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[7].transform.position.x - 0.35f, ChipPositionObj[7].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[7].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[7].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[7].transform.position.z;
                    break;
                case Spots.Eight:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[8].transform.position.x - 0.35f, ChipPositionObj[8].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[8].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[8].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[8].transform.position.z;
                    break;
                case Spots.Nine:
                    target = ChipPositionObj[9].transform.position;
                    break;
                case Spots.One_Four:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[10].transform.position.x - 0.35f, ChipPositionObj[10].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[10].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[10].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[10].transform.position.z;
                    break;
                case Spots.Zero_Five:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[11].transform.position.x - 0.35f, ChipPositionObj[11].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[11].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[11].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[11].transform.position.z;
                    break;
                case Spots.Six_Nine:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[12].transform.position.x - 0.35f, ChipPositionObj[12].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[12].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[12].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[12].transform.position.z;
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
            yield return new WaitForSeconds(chipMovetime);
            // StartCoroutine(PlayAudioClip()); 
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
            // CreateChipForOtherPlayers(chip.position, chip.chip, 0, chip.spot);
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
                    // StartCoroutine(MoveChips(child, chipSecondLastSpot));
                    Destroy(child.gameObject);
                }
            }
            yield return new WaitForSeconds(1);
            foreach (Transform child in chipHolder[winnerSpot])
            {
                StartCoroutine(MoveChips(child, chipLastSpot));
            }
            LuckyBall_UiHandler.Instance.ResetUi();

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
            LuckyBall_UiHandler.Instance.UpDateBets(spots, chip);
            GameObject chipInstance = LuckyBall_ChipSpawner.Instance.Spawn(spawnNo, chip, GetChipParent(spots));
            switch (spots)
            {
                case Spots.Zero:
                    int _zeroVal = UnityEngine.Random.Range(0, 2);
                    target.x = UnityEngine.Random.Range(ChipPositionObj[0].transform.position.x - 0.35f, ChipPositionObj[0].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[0].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[0].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[0].transform.position.z;
                    break;
                case Spots.One:
                    int _oneVal = UnityEngine.Random.Range(0, 2);
                    target.x = UnityEngine.Random.Range(ChipPositionObj[1].transform.position.x - 0.35f, ChipPositionObj[1].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[1].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[1].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[1].transform.position.z;
                    break;
                case Spots.Two:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[2].transform.position.x - 0.35f, ChipPositionObj[2].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[2].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[2].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[2].transform.position.z;
                    break;
                case Spots.Three:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[3].transform.position.x - 0.35f, ChipPositionObj[3].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[3].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[3].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[3].transform.position.z;
                    break;
                case Spots.Four:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[4].transform.position.x - 0.35f, ChipPositionObj[4].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[4].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[4].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[4].transform.position.z;
                    break;
                case Spots.Five:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[5].transform.position.x - 0.35f, ChipPositionObj[5].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[5].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[5].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[5].transform.position.z;
                    break;
                case Spots.Six:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[6].transform.position.x - 0.35f, ChipPositionObj[6].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[6].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[6].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[6].transform.position.z;
                    break;
                case Spots.Seven:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[7].transform.position.x - 0.35f, ChipPositionObj[7].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[7].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[7].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[7].transform.position.z;
                    break;
                case Spots.Eight:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[8].transform.position.x - 0.35f, ChipPositionObj[8].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[8].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[8].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[8].transform.position.z;
                    break;
                case Spots.Nine:
                    target = ChipPositionObj[9].transform.position;
                    break;
                case Spots.One_Four:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[10].transform.position.x - 0.35f, ChipPositionObj[10].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[10].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[10].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[10].transform.position.z;
                    break;
                case Spots.Zero_Five:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[11].transform.position.x - 0.35f, ChipPositionObj[11].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[11].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[11].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[11].transform.position.z;
                    break;
                case Spots.Six_Nine:
                    target.x = UnityEngine.Random.Range(ChipPositionObj[12].transform.position.x - 0.35f, ChipPositionObj[12].transform.position.x + 0.35f);
                    target.y = UnityEngine.Random.Range(ChipPositionObj[12].GetComponent<RectTransform>().position.y - 0.2f, ChipPositionObj[12].GetComponent<RectTransform>().position.y + 0.2f );
                    target.z = ChipPositionObj[12].transform.position.z;
                    break;
                default:
                    break;
            }
            StartCoroutine(MoveChip(chipInstance, target));

        }
        public void CreatePlayerChip(ChipDate chipData)
        {
            if (isTimeUp) return;
            if (!LuckyBall_UiHandler.Instance.IsEnoughBalancePresent()) return;
            print("create chip");
            Chip chip = chipData.chip;
            Spot spot = chipData.spot;
            // LuckyBall_UiHandler.Instance.AddBets(spot);
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
            // LuckyBall_ServerRequest.instance.OnChipMove(chipData.target, chip, spot);
            // betManager.AddBets(spot, LuckyBall_UiHandler.Instance.currentChip);
            LuckyBall_MainPlayer.Instance.totalBet += (int)LuckyBall_UiHandler.Instance.currentChip;
            //WOF_UiHandler.Instance.BetCoinTxt.text = MainPlayer.Instance.totalBet.ToString();
        //    LuckyBall_UiHandler.Instance.UpDateBets(spot, chip);
            // GameObject chipInstance = LuckyBall_ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            // StartCoroutine(MoveChip(chipInstance, chipData.target));
        }
    }

    [Serializable]
    public class ChipPosition
    {
        public List<GameObject> ChipPos;
    }
}
