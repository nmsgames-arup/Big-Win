using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dummyScript : MonoBehaviour
{
    public Button backBtn;

    public void ExitLobby()
    {
        SceneManager.LoadScene("MainScene");
    }
}
