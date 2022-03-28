using Updown7.UI;
using Updown7.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;
using Updown7.ServerStuff;
using UpDown7.Utility;

namespace Updown7.Gameplay
{
    /// <summary>
    /// this script is only usw for creating chip and move to different
    /// places this script also send request to sever the a chip is moved
    /// and also receive msg from sever that a chip moved
    /// </summary>
    class _7updown_ChipController : MonoBehaviour, IChipMovement
    {
        [SerializeField] _7updown_ChipSpawner chipSpawner;
        [SerializeField] iTween.EaseType easeType;
        [SerializeField] _7updown_UiHandler uiHandler;
        [SerializeField] Transform chipSecondLastSpot;
        [SerializeField] Transform chipLastSpot;

        /// <summary>
        /// the following types of parents is representing
        /// where the chip will be present after it 
        /// the movement
        /// </summary>
        [SerializeField] Transform leftParent;
        [SerializeField] Transform rightParent;
        [SerializeField] Transform middleParent;
        public GameObject LeftChipPos, RightChipPos, MiddleChipPos;

        [SerializeField] float chipMoveTime;
        public Action<Transform, Vector3> OnUserInput;
        public Action<Vector3, Chip, int, Spot> OnServerResponse;
        
        public Dictionary<Spot, Transform> chipHolder = new Dictionary<Spot, Transform>();
        _7updown_Timer timer;
        bool isTimeUp;
        private void Start()
        {
            OnUserInput += CreateChip;
            OnServerResponse += CreateChipForOtherPlayers;
            chipHolder.Add(Spot.left, leftParent);
            chipHolder.Add(Spot.right, rightParent);
            chipHolder.Add(Spot.middle, middleParent);
            timer = GetComponent<_7updown_Timer>();
            timer.onTimeUp += () => isTimeUp = true;
            timer.onCountDownStart += () => isTimeUp = false;
        }
        public BetSpots betSpots;
        void CreateChip(Transform bettingSpot, Vector3 target)
        {
            if (isTimeUp) return;
            if (!uiHandler.IsEnoughBalancePresent()) return;
            uiHandler.AddPlayerBets();
            Chip chip = uiHandler.currentChip;
            Spot spot = bettingSpot.GetComponent<BettingSpot>().spotType;
            ServerRequest.instance.OnChipMove(target, chip, spot);
            uiHandler.AddBotsBets(spot, chip);
            GameObject chipInstance = chipSpawner.Spawn(0, chip, GetChipParent(spot));
            ChipDate data = new ChipDate()
            {
                spawnNo = 0,
                spot = spot,
                target = target,
                chip=chip
            };
            //betSpots.AddData(data);
            switch (spot)
            {
                case Spot.left:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.7f , LeftChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.7f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.7f );
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    break;
                case Spot.middle:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.7f , MiddleChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.1f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.1f );
                    target.z = MiddleChipPos.transform.position.z;
                    // target = MiddleChipPos.transform.position;
                    break;
                case Spot.right:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.7f , RightChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.7f, RightChipPos.GetComponent<RectTransform>().position.y + 0.7f );
                    target.z = RightChipPos.transform.position.z;
                    // target = RightChipPos.transform.position;
                    break;
                default:
                    break;
            }
            StartCoroutine(MoveChip(chipInstance, target));
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
            GameObject chipInstance = chipSpawner.Spawn(spanNo, chip, GetChipParent(spot));
            uiHandler.AddBotsBets(spot, chip);
            switch (spot)
            {
                case Spot.left:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.7f , LeftChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.7f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.7f );
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    break;
                case Spot.middle:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.7f , MiddleChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.1f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.1f );
                    target.z = MiddleChipPos.transform.position.z;
                    // target = MiddleChipPos.transform.position;
                    break;
                case Spot.right:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.7f , RightChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.7f, RightChipPos.GetComponent<RectTransform>().position.y + 0.7f );
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
            // float x = chip.transform.localScale.x;
            // float y = chip.transform.localScale.x;
            // float distance = Vector3.Distance(chip.transform.position, target);
            // //var rotat = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 180));
            // //iTween.RotateTo(chip, iTween.Hash("rotation", rotat, "time", chipMovetime, "easetype", easeType));

            // while (distance > .1f)
            // {
            //     chip.transform.position = Vector3.MoveTowards(chip.transform.position, target, speed);
            //     distance = Vector3.Distance(chip.transform.position, target);
            //     yield return new WaitForEndOfFrame();
            // }
            iTween.MoveTo(chip, iTween.Hash("position", target, "time", chipMovetime, "easetype", easeType));
            yield return new WaitForSeconds(chipMovetime);
            //scale up
            SoundManager.instance.PlayClip("CoinAudio");
            // UtilitySound.Instance.addchipsound();
            iTween.PunchScale(chip, iTween.Hash("x", .1, "y",0.1f, "default", .1));
            
        }
        public void OnOtherPlayerMove(object data)
        {
            if (isTimeUp) return;
            OnChipMove chip = UpDown7.Utility.Utility.GetObjectOfType<OnChipMove>(data);
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
            Debug.LogError("spot  " + winnerSpot);
            foreach (var item in chipHolder)
            {
                if (item.Key == winnerSpot) continue;
                Debug.LogError("item " + item.Key);
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
            uiHandler.ResetUi();

        }
        public float waitTime = 0.85f;
        public float speed = 0.4f;
        IEnumerator MoveChips(Transform chip, Transform destinatio)
        {
            // var target = destinatio.position;
            // float distance = Vector3.Distance(chip.transform.position, target);
            // while (distance > .1f)
            // {
            //     chip.transform.position = Vector3.MoveTowards(chip.transform.position, target, speed);
            //     distance = Vector3.Distance(chip.transform.position, target);
            //     yield return new WaitForEndOfFrame();
            // }
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
            GameObject chipInstance = chipSpawner.Spawn(spanNo, chip, GetChipParent(spot));
            switch (spot)
            {
                case Spot.left:
                    target.x = UnityEngine.Random.Range(LeftChipPos.transform.position.x - 0.7f , LeftChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(LeftChipPos.GetComponent<RectTransform>().position.y - 0.7f, LeftChipPos.GetComponent<RectTransform>().position.y + 0.7f );
                    target.z = LeftChipPos.transform.position.z;
                    // target = LeftChipPos.transform.position;
                    break;
                case Spot.middle:
                    target.x = UnityEngine.Random.Range(MiddleChipPos.transform.position.x - 0.7f , MiddleChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(MiddleChipPos.GetComponent<RectTransform>().position.y - 0.1f, MiddleChipPos.GetComponent<RectTransform>().position.y + 0.1f );
                    target.z = MiddleChipPos.transform.position.z;
                    break;
                case Spot.right:
                    target.x = UnityEngine.Random.Range(RightChipPos.transform.position.x - 0.7f , RightChipPos.transform.position.x + 0.7f);
                    target.y = UnityEngine.Random.Range(RightChipPos.GetComponent<RectTransform>().position.y - 0.7f, RightChipPos.GetComponent<RectTransform>().position.y + 0.7f );
                    target.z = RightChipPos.transform.position.z;
                    // target = RightChipPos.transform.position;
                    break;
                default:
                    break;
            }
            StartCoroutine(MoveChip(chipInstance, target));
            uiHandler.AddBotsBets(spot, chip);
        }
    }
}


