using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenScript : MonoBehaviour
{
    public GameObject SplashScreen;
    public Image RingFillImg;
    public Sprite[] BackgroundFrames;
    public Image BackgroundImage;

    public void Start()
    {
        // StartCoroutine(StartBackgroundAnimation());
        StartCoroutine(Wait());
    }

    public IEnumerator StartBackgroundAnimation()
    {
        BackgroundImage.gameObject.SetActive(true);
        foreach (var item in BackgroundFrames)
        {
            BackgroundImage.sprite = item;
            yield return new WaitForSeconds(0.08f);
        }
        StartCoroutine(StartBackgroundAnimation());
    }

    public void StopBackgroundAnimation()
    {
        StopCoroutine(StartBackgroundAnimation());
        BackgroundImage.gameObject.SetActive(false);
    }
    
    IEnumerator Wait()
    {
        RingFillImg.fillAmount = 0;
        SplashScreen.SetActive(true);
        while (RingFillImg.fillAmount < 1)
        {
            RingFillImg.fillAmount += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        SplashScreen.SetActive(false);
        if (PlayerPrefs.GetString("MobileNum", "") != "")
        {
            string str = PlayerPrefs.GetString("MobileNum", "");
            LoginScript.Instance.Mobile.text = PlayerPrefs.GetString("MobileNum", "");
            LoginScript.Instance.Password.text = PlayerPrefs.GetString("password", "");
            LoginScript.Instance.LoginBtn();
            
        }
        else if (PlayerPrefs.GetString("GuestData", "") != "")
        {
            LoginScript.Instance.PlayGuestBtn();
        }
        else
        {
            LoginScript.Instance.ShowLoginUI();
        }
        //res myObject = new res();
        //myObject.en = "hello";
        ////myObject.data.profilePic = "1";
        //string json = JsonUtility.ToJson(myObject);
        ////object o = new { en = "hello" };
        //print(json);

    }
}
[Serializable]
public class res//{en:"HELLO","data":{"Abc":"bbb"}}
{
    public string en;
    public Datas data;
    [Serializable]
    public class Datas//{en:"HELLO","data":{"Abc":"bbb"}}
    {
        public string profilePic;
        public string balance;
    }
}
