using Com.BigWin.WebUtils;
using LobbyScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LobbyScripts;
using Newtonsoft.Json;
public class HomeScript : MonoBehaviour
{
    public static HomeScript Instance;
    public GameObject HomePanel;
    public Text NameTxt;
    public Text IDTxt;
    public Image Profile;
    public Text Balance;
    AsyncOperation DragonScene;
    AsyncOperation sevenupScene;
    public Image LobbyAnimpnel;
    public Sprite[] Lobbyframe;
    public Sprite[] WOF_Frames;
    public Image WOF_img;
    public Sprite[] TVE_Frames;
    public Image TVE_img;
    public Sprite[] LuckyDice_Frames;
    public Image LuckyDice_img;
    public Sprite[] LuckyBall_Frames;
    public Image LuckyBall_img;
    public Sprite[] PokerKing_Frames;
    public Image PokerKing_img;
    public Text _loadingTxt;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // StartCoroutine(Loading());
        StartCoroutine(StartWOF_Animation());
        StartCoroutine(StartTVE_Animation());
        StartCoroutine(StartLuckyDice_Animation());
        StartCoroutine(StartLuckyBall_Animation());
        StartCoroutine(StartPokerKing_Animation());
    }

    public IEnumerator StartWOF_Animation()
    {
        WOF_img.gameObject.SetActive(true);
        foreach (var item in WOF_Frames)
        {
            WOF_img.sprite = item;
            yield return new WaitForSeconds(0.015f);
        }
        // StartCoroutine(StartWOF_Animation());
        StartCoroutine(StopWOF_Animation());
    }

    public IEnumerator StopWOF_Animation()
    {
        StopCoroutine(StartWOF_Animation());
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(StartWOF_Animation());
    }

    public IEnumerator StartTVE_Animation()
    {
        TVE_img.gameObject.SetActive(true);
        foreach (var item in TVE_Frames)
        {
            TVE_img.sprite = item;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(StartTVE_Animation());
    }
    public IEnumerator StartLuckyDice_Animation()
    {
        LuckyDice_img.gameObject.SetActive(true);
        foreach (var item in LuckyDice_Frames)
        {
            LuckyDice_img.sprite = item;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(StartLuckyDice_Animation());
    }

    public IEnumerator StartLuckyBall_Animation()
    {
        LuckyBall_img.gameObject.SetActive(true);
        foreach (var item in LuckyBall_Frames)
        {
            LuckyBall_img.sprite = item;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(StartLuckyBall_Animation());
    }

    public IEnumerator StartPokerKing_Animation()
    {
        PokerKing_img.gameObject.SetActive(true);
        foreach (var item in PokerKing_Frames)
        {
            PokerKing_img.sprite = item;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(StartPokerKing_Animation());
    }

    IEnumerator Loading()
    {
        foreach (var item in Lobbyframe)
        {
            LobbyAnimpnel.sprite = item;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Loading());
    }
    public void ShowHomeUI()
    {
        //DragonScene = SceneManager.LoadSceneAsync("DragonScene");
        //DragonScene.allowSceneActivation = false;
        //sevenupScene = SceneManager.LoadSceneAsync("7updown");
        //sevenupScene.allowSceneActivation = false;
        Balance.text = UserDetail.Balance.ToString();
        Profile.sprite = ProfileScript.Instance.ProfileSprite[UserDetail.ProfileId];
        NameTxt.text = UserDetail.Name;
        IDTxt.text = "id:" + UserDetail.ID;
        HomePanel.SetActive(true);

        User user = new User() { 
        user_id = UserDetail.UserId, version_code = 1, language = "en" 
        };
        WebRequestHandler.instance.Post(Constants.ProfileURL,JsonUtility.ToJson(user), (data, status) => {
            if (!status) return;

            Profile profile = JsonConvert.DeserializeObject<Profile>(data);
            //print("balance is " + profile.response.data.chip_balance);
            Balance.text = profile.response.data.chip_balance.ToString();
            UserDetail.Balance = int.Parse(profile.response.data.chip_balance.ToString());
            print("Profile Data " + data);
        });

       
    }

    

    public void DragonBtn()
    {
        _loadingTxt.gameObject.SetActive(true);
        // AndroidToastMsg.ShowAndroidToastMessage("Loading");
        //sevenupScene.allowSceneActivation = false;
        //DragonScene.allowSceneActivation = true;
        SceneManager.LoadScene("DragonScene");
    }
    public void SevenUpBtn()
    {
        _loadingTxt.gameObject.SetActive(true);
        // AndroidToastMsg.ShowAndroidToastMessage("Loading");
        //DragonScene.allowSceneActivation = false;
        //sevenupScene.allowSceneActivation = true;
        SceneManager.LoadScene("7updown");
    }
    public void AndarBaharBtn()
    {
        AndroidToastMsg.ShowAndroidToastMessage("Loading");
        SceneManager.LoadScene("AndarBahar");
    }
    public void WheelOfFortune()
    {
        _loadingTxt.gameObject.SetActive(true);
        // AndroidToastMsg.ShowAndroidToastMessage("Loading");
        SceneManager.LoadScene("WheelOfFortune");
    }
    public void ShopBtn()
    {
        ShopScript.Instance.ShowShopUI();
    }
    public void ProfileBtn()
    {
        ProfileScript.Instance.ShowProfileUI();
    }
    public void settingBtn()
    {
        SettingScript.Instance.ShowSettingUI();
    }
    public void SupportBtn()
    {
        SupportScript.Instance.ShowSupportUI();
    }
    public void NoticeBtn()
    {
        NoticeScript.Instance.ShowNoticeUI();
    }
    public void MailBtn()
    {
        MailScript.Instance.ShowMailUI();
    }
    public void ShareBtn()
    {
        ReferAndEarnScript.Instance.ShowReferAndEarnUI();
    }
    public void SafeBtn()
    {
        SafeScript.Instance.ShowSafeUI();
    }
    public void RankBtn()
    {
        RankScript.Instance.ShowRankUI();
    }
    public void WithdrawBtn()
    {
        WithDrawScript.Instance.ShowWithDrawUI();
    }

    public void DiamondBtn()
    {
        //DiamondScript.Instance.ShowDiamondUI();
        DiamondOffer.Instance.ShowDiamondUI();
    }

    public void AgentBtn()
    {
        AgentScript.Instance.ShowDiamondUI();
    }
    public static string RandomString(int length)
    {
        string element = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        System.Random random = new System.Random();
        return new string((from s in Enumerable.Repeat(element, length)
                           select s[random.Next(s.Length)]).ToArray());
    }
}
public class LobbyData
{
    public string playerId;
}

public class ProfileData
{
    public string username;
                  public int chip_balance ;
                  public int safe_balance ;
                     public int vip_level ;
         public string invite_friend_code ;
             public int team_name_updated ;
                      public string image ;
    public List<object> refered_to_friend ;
                     public string gender ;
              public string referal_bonus ;
             public bool account_verified ;
               public string Signup_bonus ;
             public string referral_bonus ;
             public int refered_by_status ;
               public string version_code ;
                    public string apk_url ;
}

public class ProfileResponse
{
    public bool status;
    public string message;
    public ProfileData data;
}

[Serializable]
public class Profile
{
    public ProfileResponse response;
}

public class User
{
    public int user_id;
    public int version_code;
    public string language;
}
