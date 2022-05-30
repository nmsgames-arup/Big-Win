using Com.BigWin.WebUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScript : MonoBehaviour
{
    public static RegisterScript Instance;
    public GameObject RegisterPanel;
    public InputField EmailId;
    public InputField MobileNo;
    public InputField Password;
    public InputField UserName;
    public InputField ReEnterPassword;
    public InputField OTP;
    public Button OTPBtn;
    //public Button SubmitButton;
    public Text ShowMessageText;
    // string RegisterURL = "https://jeetogame.in/jeeto_game_new/WebServices/SignUp";//"https://jeetogame.in/jeeto_game/WebServices/SignUp";
    string RegisterURL = "http://52.7.28.180:5000/auth/verifyemail";
    string OTPURL = "http://52.7.28.180:5000/auth/signUp";
    private void Awake()
    {
        Instance = this;
       
        ShowMessageText.gameObject.SetActive(false);
       // MobileNo.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
       // MobileNo.onEndEdit.AddListener(delegate { ValueLengthCheck(); });
    }

    void OnEnable()
    {
        OTPBtn.interactable = true;
    }

    // Invoked when the value of the text field changes.
    public void ValueChangeCheck()
    {
        int number;
        if (!int.TryParse(MobileNo.text, out number))
        {
            ShowMessage("Please Enter Number Only");
            Debug.Log("String is the number: " + number);
        }
       
    }

    public void ValueLengthCheck()
    {
        int number;
        if (MobileNo.text.Length!=10)
        {
            ShowMessage("Please Enter Correct Mobile Number Only");
           
        }
       
    }


    public void ShowRegisterUI()
    {
        RegisterPanel.SetActive(true);
    }
    public void CloseRegisterUI()
    {
        RegisterPanel.SetActive(false);
    }
    public void RegisterBtn()
    {
        SessionXMLScript _sessionScript = FindObjectOfType<SessionXMLScript>();
        // _sessionScript.CreateXMLFile_data();
        // RegisterPanel.SetActive(false);
        if (!String.IsNullOrEmpty(EmailId.text) || !String.IsNullOrEmpty(OTP.text))
        {
                string device_id = SystemInfo.deviceUniqueIdentifier;
                ShowMessageText.gameObject.SetActive(false);
                // RegisterForm form = new RegisterForm(MobileNo.text, Password.text, ReEnterPassword.text,UserDetail.UserId, "en");
                RegisterForm form = new RegisterForm("monica22patel@gmail.com", Password.text, Password.text, "monica", "5874879531", "en");
                StartCoroutine(WebRequestHandler.instance.RegisterAPI(RegisterURL, EmailId.text, OTP.text));
                // WebRequestHandler.instance.Post(RegisterURL, JsonUtility.ToJson(form), OnRegisterRequestProcessed);
            // }
        }
    }

    void ShowMessage(string MSG)
    {
        ShowMessageText.text = MSG;
       
    }
    private void OnRegisterRequestProcessed(string json, bool success)
    {
        RegisterFormRoot responce = JsonUtility.FromJson<RegisterFormRoot>(json);
        if (responce.response.status)
        {
            RegisterPanel.SetActive(false);
            // ProfileScript.Instance.ShowProfileUI();
        }
    
    }
   public void OTPVerifyBtn()
    {
        if (String.IsNullOrEmpty(EmailId.text))
        {
            ShowMessage("Enter Email ID");
        }
        else if(String.IsNullOrEmpty(Password.text))
        {
            ShowMessage("Enter Password");
        }
        // else if(String.IsNullOrEmpty(UserName.text) )
        // {
        //     ShowMessage("Enter UserName");
        // }
        // else if(String.IsNullOrEmpty(MobileNo.text))
        // {
        //     ShowMessage("Enter Mobile No");
        // }
        else
        {

            Debug.Log("data entered  ");
            string device_id = SystemInfo.deviceUniqueIdentifier;
            // RegisterForm form = new RegisterForm(MobileNo.text, device_id,Password.text, ReEnterPassword.text,"en");
            RegisterForm form = new RegisterForm("5874598745", Password.text, ReEnterPassword.text,UserDetail.UserId, "en");
            // WebRequestHandler.instance.Post(OTPURL, JsonUtility.ToJson(form), OnOtpVerifyRequestProcessed);
            StartCoroutine(WebRequestHandler.instance.GetOTP(OTPURL, EmailId.text, Password.text, "dummy", "5874987512"));
        }
    }

    public void GetOTPBtn()
    {
        // WebRequestHandler.instance.GetOTP(OTPURL, EmailId.text, Password.text, "dummy", "5874896325");
        StartCoroutine(WebRequestHandler.instance.GetOTP(OTPURL, EmailId.text, Password.text, "dummy", "5874987512"));
    }


    private void OnOtpVerifyRequestProcessed(string json, bool success)
    {
        LoginFormRoot responce = JsonUtility.FromJson<LoginFormRoot>(json);
        Debug.Log(responce.response.message);
        AndroidToastMsg.ShowAndroidToastMessage(responce.response.message);
    }

}
[Serializable]
public class RegisterForm
{
    public string emailId;
    public string mobile_number;
    public string password;
    public string c_password;
    public string userName;
    public string language;
   // public string otp;
    public int user_id;
    public string device_id;

public RegisterForm(string mobile_number, string password,string c_password,  int user_id, string language)
    {
        this.mobile_number = mobile_number;
        this.password = password;
        this.c_password = c_password;
        this.language = language;
       // this.otp = otp;
        this.user_id = user_id;
    } 
    public RegisterForm(string emailId, string password,string c_password, string userName, string mobile_number, string language)
    {
        this.emailId = emailId;
        this.password = password;
        this.c_password = c_password;
        this.userName = userName;
        this.mobile_number = mobile_number;
        this.language = language;
    }
  /*  public RegisterForm(string mobile_number, string password,string c_password, string user_id, string language)
    {
        this.mobile_number = mobile_number;

        this.password = password;
        this.c_password = c_password;
        this.language = language;
        this.user_id = user_id;
    }*/
}
[Serializable]
public class RegisterFormData
{
    public int id;
    public string username;
    public object first_name;
    public object last_name;
    public int role_id;
    public object email;
    public string phone;
    public string password;
    public string c_password;
    public object team_name;
    public object date_of_bith;
    public int gender;
    public object country;
    public object state;
    public object city;
    public object postal_code;
    public object address;
    public string image;
    public int guest_id;
    public int profile_id;
    public object fb_id;
    public object google_id;
    public string refer_id;
    //public string otp;
   // public DateTime otp_time;
    public int is_login;
    //public int otp_verified;
    public object last_login;
    public string device_id;
    public object device_type;
    public object module_access;
    public object current_password;
    public string language;
    public string cash_balance;
    public string safe_balance;
    public string winning_balance;
    public string bonus_amount;
    public object coin_balance;
    public int vip_level;
    public int status;
    public int is_updated;
    public object email_verified;
    public object verify_string;
    public int sms_notify;
    public object createdby;
    public DateTime created;
    public string modified;
    public object bot_type;
    public int user_id;
    public object full_name;
}
[Serializable]
public class RegisterFormResponse
{
    public bool status;
    public string message;
    public RegisterFormData data;
}
[Serializable]
public class RegisterFormRoot
{
    public RegisterFormResponse response;
}