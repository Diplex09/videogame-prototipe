using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    // On singleplayer button click
    public void OnSingleplayerButtonClick()
    {
        // Load singleplayer scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Demo");
    }

    // On multiplayer button click
    public void OnMultiplayerButtonClick()
    {
        // Load multiplayer scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MultiplayerMinigame");
    }
}