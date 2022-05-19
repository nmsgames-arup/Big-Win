using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Text;
using SimpleJSON;

namespace Com.BigWin.WebUtils
{
    public class WebRequestHandler : MonoBehaviour
    {
        public static WebRequestHandler instance;
        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            // StartCoroutine(Upload());
        }
        public void Get(string url, Action<string, bool> OnRequestProcessed)
        {
            StartCoroutine(GetRequest(url, OnRequestProcessed));
        }
        private IEnumerator GetRequest(string url, Action<string, bool> OnRequestProcessed)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                print("check internet connection");
                AndroidToastMsg.ShowAndroidToastMessage("check internet connection");
                yield break;
            }
           
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("web request error in Get method with responce code : " + request.responseCode);
                OnRequestProcessed(request.error, false);
            }
            else
            {
                Debug.Log("sending get web request  : " + url + "got response:" + request.downloadHandler.text);
                OnRequestProcessed(request.downloadHandler.text, true);
            }
            request.Dispose();
        }
        public void Post(string url, string json, Action<string, bool> OnRequestProcessed)
        {
            Debug.Log("URL " + url);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                print("check internet connection");
                AndroidToastMsg.ShowAndroidToastMessage("check internet connection");
                return;
            }
            Debug.Log(url + " json request: " + json);
            // StartCoroutine(PostRequest_new(url, json, OnRequestProcessed));
        }

        private IEnumerator PostRequest(string url, string json, Action<string, bool> OnRequestProcessed, int attemps = 2)
        {
            Debug.Log("url>>>>>  " + url);
            Debug.Log("PostRequest " + json);
            var request = new UnityWebRequest(url, "POST");

            // var formContent = new StringContent(json, Encoding.UTF8, "application/json");
            // HttpResponseMessage response = client.PostAsync(url, formContent).GetAwaiter().GetResult();
            // string responseStr  = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            // JSONNode jsonNode = SimpleJSON.JSON.Parse(responseStr);

            request.SetRequestHeader("content-type", "application/json");
            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));


            // byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            // request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            // request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            // request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
           
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {

                if (attemps == 0)
                {
                    Debug.Log(">>>>>>>>" + request.error);
                    OnRequestProcessed(request.error, false);
                }
                else
                {
                    Debug.Log(">>>>>PostRequest>>>" );
                    StartCoroutine(PostRequest(url, json, OnRequestProcessed, --attemps));
                }
                    
            }
            else
            {
                Debug.Log(url + " json response: " + request.downloadHandler.data);
                OnRequestProcessed(request.downloadHandler.text, true);
            }
            request.Dispose();
        }

        public IEnumerator GetOTP(string OTPURL, string email, string password, string username, string phone)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);
            form.AddField("username", username);
            form.AddField("phone", phone);

            using (UnityWebRequest www = UnityWebRequest.Post(OTPURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    RegisterScript.Instance.OTPBtn.interactable = false;
                    Debug.Log("get otp func  ");
                }
            }
        }

        public IEnumerator RegisterAPI(string registerURL, string email, string OTP)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("otp", OTP);

            using (UnityWebRequest www = UnityWebRequest.Post(registerURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    RegisterScript.Instance.RegisterPanel.SetActive(false);
                }
            }
        }

        public IEnumerator LoginAPI(string registerURL, string email, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);

            using (UnityWebRequest www = UnityWebRequest.Post(registerURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    PlayerPrefs.SetString("email", email);
                    PlayerPrefs.SetString("password", password);
                    LoginScript.Instance.ShowLoginUI();
                }
            }
        }

        public void DownloadSprite(string url, Action<Sprite> OnDownloadComplete)
        {
            StartCoroutine(LoadFromWeb(url, OnDownloadComplete));
        }

        IEnumerator LoadFromWeb(string url, Action<Sprite> OnDownloadComplete)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url);
            DownloadHandlerTexture textureDownloader = new DownloadHandlerTexture(true);
            webRequest.downloadHandler = textureDownloader;
            yield return webRequest.SendWebRequest();
            if (!(webRequest.isNetworkError || webRequest.isHttpError))
            {
                Texture2D texture = textureDownloader.texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);
                OnDownloadComplete(sprite);
            }
            else
            {
                Debug.Log("failed to download image");
            }
        }
        public void DownloadTexture(string url, Action<Texture> OnDownloadComplete)
        {
            StartCoroutine(LoadFromWeb(url, OnDownloadComplete));
        }
        IEnumerator LoadFromWeb(string url, Action<Texture> OnDownloadComplete)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url);
            DownloadHandlerTexture textureDownloader = new DownloadHandlerTexture(true);
            webRequest.downloadHandler = textureDownloader;
            yield return webRequest.SendWebRequest();
            if (!(webRequest.isNetworkError || webRequest.isHttpError))
            {
                OnDownloadComplete(textureDownloader.texture);
            }
            else
            {
                Debug.Log("failed to download image");
            }
        }
        public int GetVersionCode()
        {
            return FetchVersionCode();
        }
        public static int FetchVersionCode()
        {
            try
            {
                AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
                string packageName = context.Call<string>("getPackageName");
                AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
                return packageInfo.Get<int>("versionCode");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 2;
            }
        }
    }
}