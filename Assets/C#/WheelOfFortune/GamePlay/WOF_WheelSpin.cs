using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WOF.Gameplay
{
    public class WOF_WheelSpin : MonoBehaviour
    {
        public static WOF_WheelSpin Instance;
        public Card Currentcard;
        public float faceCardcustomAngle;
        int nextFaceCardNumber;
        private int currentFaceCardNo;
        [SerializeField] GameObject outerWheel;
        [SerializeField] private int wheelTime = 7;
        [SerializeField] private int noOfRounds = 3;
        public iTween.EaseType easetype;

        /* New formula methods variable */
        [SerializeField] private int currentImageIndex;
        [SerializeField] private int lastImageIndex;
        public AnimationCurve animationCurve;
        [SerializeField] private GameObject _fortuneWheel;
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject[] _awardImages;
        public Action onSpinComplete;
        public float spaceBetweenTwoImages=100;
        public bool isSpinning;
        public Transform centerObj;
        // int[] angles = { 0, 36, 72, 108, -216, -180, -144, -108, -72, -36 };
        int[] angles = { 0, 18, 36, 54, 72, 90, 108, 126, 144, 162, 180, -162, -144, -126, -108, -90, -72, -54, -36, -18 };
        float temp = 0f;
        public int desireNo = 5;
        public float speed;
        public float rateOfChangeOfSpeed = 0.01f;
        public int maxRounds = 5;
        public float singleAngle = 360 / 10;
        public float totalAngles;
        public float anglesUntillNow = 0f;
        public float initialTime = 1;
        public float t = 2f;
        public float m = 2f;
        public int currentNo = 0;
        public int currentRound = 0;
        public float Angle;
        public float initailDistance;
        public float initalposX;
        public float remainDistance;

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            _fortuneWheel = gameObject;
            lastImageIndex = 0;
            currentImageIndex = 0;
        }

        // public void Spin(Card number)
        // {
        //     Currentcard = number;
        //     nextFaceCardNumber = (int)number;
        //     if (currentFaceCardNo == nextFaceCardNumber)
        //     {
        //         faceCardcustomAngle = 0;
        //     }
        //     else if (currentFaceCardNo > nextFaceCardNumber)
        //     {
        //         // faceCardcustomAngle = Mathf.Abs(currentFaceCardNo - nextFaceCardNumber) / 12f;
        //         faceCardcustomAngle = Mathf.Abs(currentFaceCardNo - nextFaceCardNumber) / 20f;
        //     }
        //     else
        //     {
        //         // faceCardcustomAngle = Mathf.Abs(12 - (nextFaceCardNumber - currentFaceCardNo)) / 12f;
        //         faceCardcustomAngle = Mathf.Abs(20 - (nextFaceCardNumber - currentFaceCardNo)) / 20f;

        //     }
        //     faceCardcustomAngle += noOfRounds;
        //     Currentcard = number;

        //     // iTween.RotateBy(outerWheel, iTween.Hash("z", faceCardcustomAngle, "time", wheelTime,
        //     //      "easetype", easetype, "oncompletetarget", this.gameObject));
        //     iTween.RotateBy(outerWheel, iTween.Hash("z", faceCardcustomAngle, "time", wheelTime, "easetype", easetype,
        //         "oncomplete", "OnAnimationComplete", "oncompletetarget", this.gameObject));
        // }

        // void OnAnimationComplete()
        // {
        //     faceCardcustomAngle = noOfRounds - faceCardcustomAngle;
        //     currentFaceCardNo = nextFaceCardNumber;
        // }


        // public void Spin(int wheelNo, string imageXfactor)
        public void Spin(int wheelNo)
        {
            desireNo = wheelNo;
            isSpinning = true;
            CalculateAngle();
            initailDistance = centerObj.transform.position.x;
        }
        void Update()
        {
            if (isSpinning)
            {
                if (anglesUntillNow < totalAngles)
                {
                    float angle = speed * t;


                    Angle = angle;
                    anglesUntillNow += Mathf.Abs(angle);

                    float mapTime = animationCurve.Evaluate(
                        NumberMapping(anglesUntillNow, 0, totalAngles,
                            rateOfChangeOfSpeed, initialTime));

                    float distanceLeft = NumberMapping(anglesUntillNow, 0, totalAngles,
                            initalposX, initailDistance);
                    remainDistance = distanceLeft;
                    m = mapTime;
                    t = mapTime;
                    //move images
                    // _awardImages[currentImageIndex].transform.position = new Vector2(distanceLeft,
                    //     _awardImages[currentImageIndex].transform.position.y);
                    //rotate wheel
                    _fortuneWheel.transform.Rotate(0f, 0f, -angle);
                    temp += Mathf.Abs(angle);
                    if (temp > 360)
                    {
                        currentRound++;
                        temp = 0;
                    }
                }
                else
                {
                    currentNo = desireNo;
                    AfterSpin(currentNo);
                }
            }
        }
        void AfterSpin(int wheelNo)
        {
            onSpinComplete?.Invoke();
            _fortuneWheel.transform.eulerAngles = new Vector3(0, 0, angles[wheelNo]);
            isSpinning = false;
            lastImageIndex = currentImageIndex;
            // RemoveParents();
            // if (onSpinComplete != null)
            //     onSpinComplete();
        }
        // private void RemoveParents()
        // {
        //     for (int i = 0; i < _awardImages.Length; i++)
        //     {
        //         if (i == currentImageIndex) continue;
        //         _awardImages[i].transform.parent = _content.transform;
        //     }
        // }
        private void CalculateAngle()
        {
            anglesUntillNow = 0f;
            currentRound = 1;
            t = initialTime;
            if (currentNo == desireNo)
            {
                totalAngles = maxRounds * 360;
            }
            else if (currentNo < desireNo)
            {
                totalAngles = (10 - Mathf.Abs(currentNo - desireNo)) * singleAngle + maxRounds * 360;
            }
            else
            {
                totalAngles = Mathf.Abs(currentNo - desireNo) * singleAngle + maxRounds * 360;

            }
        } 
        float NumberMapping(float num, float in_min, float in_max, float out_min, float out_max)
        {
            return (num - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
        // public void SetWheelInitialAngle(int wheelNo, string xfactor)
        // {
        //     print("set initialangle ");
        //     _fortuneWheel.transform.eulerAngles = new Vector3(0, 0, angles[wheelNo]);
        //     lastImageIndex = 0;
        //     if (xfactor == "4x")
        //     {
        //         lastImageIndex = _awardImages.Length - 1;
        //     }
        //     else if (xfactor == "2x")
        //     {
        //         lastImageIndex = _awardImages.Length - 2;
        //     }
        //     for (int i = 0; i < _awardImages.Length; i++)
        //     {
        //         _awardImages[i].transform.parent = _content.transform;
        //         _awardImages[i].transform.localPosition = new Vector3(200, _awardImages[i].transform.localPosition.y, 0);
        //     }
        //     currentNo = wheelNo;
        //     _awardImages[lastImageIndex].transform.localPosition = new Vector3(0, _awardImages[currentImageIndex].transform.localPosition.y, 0);

        // }
        // private void RearangeAwardImages(string img_xFactor)
        // {
        //     print("rearange xfactor is " + img_xFactor);
        //     if (img_xFactor == "1x")
        //     {
        //         while (lastImageIndex == currentImageIndex)
        //         {
        //             currentImageIndex = UnityEngine.Random.Range(0, _awardImages.Length - 4);
        //         }
        //     }
        //     else if (img_xFactor == "2x")
        //     {
        //         currentImageIndex = lastImageIndex == _awardImages.Length - 4 ? _awardImages.Length - 2 : _awardImages.Length - 4;
        //     }
        //     else if (img_xFactor == "4x")
        //     {
        //         currentImageIndex = lastImageIndex == _awardImages.Length - 3 ? _awardImages.Length - 1 : _awardImages.Length - 3;
        //     }
        //     _awardImages[currentImageIndex].transform.SetAsLastSibling();
        //     _awardImages[lastImageIndex].transform.SetAsFirstSibling();
        //     float finalXCoordinate = (_awardImages[currentImageIndex].GetComponent<RectTransform>().rect.width * (_awardImages.Length - 1) + spaceBetweenTwoImages * (_awardImages.Length - 1));
        //     _awardImages[currentImageIndex].transform.localPosition = new Vector2(-finalXCoordinate, _awardImages[currentImageIndex].transform.localPosition.y);

        //     int index = 0;
        //     foreach (Transform image in _content.transform)
        //     {

        //         float xCoordinate = 0;
        //         if (index == 0)
        //         {
        //             xCoordinate = 0f;
        //         }
        //         else xCoordinate = (100 * index + spaceBetweenTwoImages * index);
        //         image.transform.localPosition = new Vector2(-xCoordinate, image.transform.localPosition.y);
        //         index++;
        //     }
        //     for (int i = 0; i < _awardImages.Length; i++)
        //     {
        //         if (i == currentImageIndex) continue;
        //         _awardImages[i].transform.parent = _awardImages[currentImageIndex].transform;
        //     }
        // }

        // public void ForceFullyStopWheel()
        // {
        //     if (isSpinning)
        //     {
        //         iTween.Stop(_fortuneWheel);
        //         iTween.Stop(_awardImages[currentImageIndex], true);
        //         _fortuneWheel.transform.eulerAngles = new Vector3(0, 0, 0);
        //         lastImageIndex = currentImageIndex;

        //         for (int i = 0; i < _awardImages.Length; i++)
        //         {
        //             _awardImages[i].transform.parent = _content.transform;
        //             _awardImages[i].transform.localPosition = new Vector3(200, _awardImages[i].transform.localPosition.y, 0);
        //         }
        //         _awardImages[currentImageIndex].transform.localPosition = new Vector3(0, _awardImages[currentImageIndex].transform.localPosition.y, 0);
        //         isSpinning = false;
        //     }
        // }
    

    }

    public enum Card
    {
        Blue = 0,
        Red = 1,
        Yellow = 2,
        NONE=-1,
    }
}
