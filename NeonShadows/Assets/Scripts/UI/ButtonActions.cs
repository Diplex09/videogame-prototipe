using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    public void OnClickSingleplayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("FirstLevel");
    }

    public void OnClickMultiplayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MultiplayerMinigame");
    }
}
