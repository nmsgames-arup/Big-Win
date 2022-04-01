using WOF.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WOF.ServerStuff;
using WOF.Utility;
using Shared;
using KhushbuPlugin;
using WOF.UI;
using WOF.player;

namespace WOF.Gameplay
{
    public class WOF_ChipController : MonoBehaviour
    {
        public static WOF_ChipController Instance;

        [SerializeField] iTween.EaseType easeType;

        [SerializeField] Transform chipSecondLastSpot;
        [SerializeField] Transform chipLastSpot;

        /// <summary>
        /// the following types of parents is representing
        /// where the chip will be present after it 
        /// the movement
        /// </summary>
        public Transform leftParent;
        public Transform rightParent;
        public Transform middleParent;
        public GameObject LeftChipPos, RightChipPos, MiddleChipPos;

        [SerializeField] float chipMoveTime;
        public Action<Transform, Vector3> OnUserInput;
        public Action<Vector3, Chip, int, Spot> OnServerResponse;

        public Dictionary<Spot, Transform> chipHolder = new Dictionary<Spot, Transform>();
        public AudioClip AddChip, CoinGenerate;
        public AudioSource CoinMove_AudioSource, CoinGenerate_AudioSource;
        bool isTimeUp;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            OnUserInput += CreateChip;
            OnServerResponse += CreateChipForOtherPlayers;
            chipHolder.Add(Spot.right, rightParent);
            chipHolder.Add(Spot.left, leftParent);
            chipHolder.Add(Spot.middle, middleParent);
            WOF_Timer.Instance.onTimeUp += () => isTimeUp = true;
            WOF_Timer.Instance.onCountDownStart += () => isTimeUp = false;
        }
        public WOF_BetManager betManager;
        public BetSpots betSpots;
        void CreateChip(Transform bettingSpot, Vector3 target)
        {
            if (isTimeUp) return;
            if (!WOF_UiHandler.Instance.IsEnoughBalancePresent()) return;
            Chip chip = WOF_UiHandler.Instance.currentChip;
            Spot spot = bettingSpot.GetComponent<BettingSpot>().spotType;
            WOF_UiHandler.Instance.AddBets(spot);
            switch (spot)
            {
                case Spot.left:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.3f , LeftChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.4f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = LeftChipPos.transform.position.y;
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    break;
                case Spot.middle:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.3f , MiddleChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.4f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = MiddleChipPos.transform.position.y;
                    target.z = MiddleChipPos.transform.position.z;
                    // target = MiddleChipPos.transform.position;
                    break;
                case Spot.right:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.3f , RightChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.4f, RightChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = RightChipPos.transform.position.y;
                    target.z = RightChipPos.transform.position.z;
                    // target = RightChipPos.transform.position;
                    break;
                default:
                    break;
            }
            ServerRequest.instance.OnChipMove(target, chip, spot);
            betManager.AddBets(spot, WOF_UiHandler.Instance.currentChip);
            WOF_UiHandler.Instance.UpDateBets(spot, chip);
            // GameObject chipInstance = WOF_ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            GameObject chipInstance = WOF_ChipSpawner.Instance.Spawn(7, chip, GetChipParent(spot));
            StartCoroutine(MoveChip(chipInstance, target));
        }
        public void AddData(Spot spot, Vector3 target)
        {
            int ind = UnityEngine.Random.Range(1,6);
            ChipDate chipData = new ChipDate
            {
                chip = WOF_UiHandler.Instance.currentChip,
                spot = spot,
                target = target,
                spawnNo = ind
            };
            GameObject chipInstance = WOF_ChipSpawner.Instance.Spawn(0, WOF_UiHandler.Instance.currentChip, this.transform);
            //StartCoroutine(MoveChip(chipInstance, target));
            betSpots.AddData(chipData);
        }
        Transform GetChipParent(Spot betType)
        {
            switch (betType)
            {
                case Spot.left: return leftParent;
                case Spot.middle: return middleParent;
                case Spot.right: return rightParent;
            }
            return null;
        }
        void CreateChipForOtherPlayers(Vector3 target, Chip chip, int spanNo, Spot spot)
        {
            GameObject chipInstance = WOF_ChipSpawner.Instance.Spawn(spanNo, chip, GetChipParent(spot));
            WOF_UiHandler.Instance.UpDateBets(spot, chip);
            switch (spot)
            {
                case Spot.left:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.3f , LeftChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.4f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = LeftChipPos.transform.position.y;
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    break;
                case Spot.middle:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.3f , MiddleChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.4f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = MiddleChipPos.transform.position.y;
                    target.z = MiddleChipPos.transform.position.z;
                    // target = MiddleChipPos.transform.position;
                    break;
                case Spot.right:
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

        public void TakeChipsBack(Spot winner)
        {
            StartCoroutine(DestroyChips(winner));
        }
        IEnumerator DestroyChips(Spot winnerSpot)
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
            WOF_UiHandler.Instance.ResetUi();

        }
        public float speed = 1f;
        public float waitTime = 0.85f;
        IEnumerator MoveChips(Transform chip, Transform destinatio)
        {
            iTween.MoveTo(chip.gameObject, iTween.Hash("position", destinatio.position, "time", chipMoveTime, "easetype", easeType));
            yield return new WaitForSeconds(waitTime);
            Destroy(chip.gameObject);
        }
        public void CreateBotsChips(ChipDate chipData)
        {
            if (isTimeUp) return;

            var chip = chipData.chip;
            var spot = chipData.spot;
            var spanNo = chipData.spawnNo;
            var target = chipData.target;
            WOF_UiHandler.Instance.UpDateBets(spot, chip);
            GameObject chipInstance = WOF_ChipSpawner.Instance.Spawn(spanNo, chip, GetChipParent(spot));
            switch (spot)
            {
                case Spot.left:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.3f , LeftChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.4f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = LeftChipPos.transform.position.y;
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    // target = new Vector3(UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().rect.xMin, LeftChipPos.GetComponent<RectTransform>().rect.xMax), UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().rect.yMin, LeftChipPos.GetComponent<RectTransform>().rect.yMax), 0 );
                    break;
                case Spot.middle:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.3f , MiddleChipPos.transform.position.x + 0.3f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.4f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.4f );
                    // target.y = MiddleChipPos.transform.position.y;
                    target.z = MiddleChipPos.transform.position.z;
                    // target = MiddleChipPos.transform.position;
                    break;
                case Spot.right:
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
            if (!WOF_UiHandler.Instance.IsEnoughBalancePresent()) return;
            print("create chip");
            Chip chip = chipData.chip;
            Spot spot = chipData.spot;
            WOF_UiHandler.Instance.AddBets(spot);
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
            ServerRequest.instance.OnChipMove(chipData.target, chip, spot);
            betManager.AddBets(spot, WOF_UiHandler.Instance.currentChip);
            WOF_MainPlayer.Instance.totalBet += (int)WOF_UiHandler.Instance.currentChip;
            //WOF_UiHandler.Instance.BetCoinTxt.text = MainPlayer.Instance.totalBet.ToString();
           WOF_UiHandler.Instance.UpDateBets(spot, chip);
            GameObject chipInstance = WOF_ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            StartCoroutine(MoveChip(chipInstance, chipData.target));
        }

    }
}

