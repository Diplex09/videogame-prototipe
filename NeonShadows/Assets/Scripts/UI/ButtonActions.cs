using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickSingleplayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("FirstLevel");
    }

    public void OnClickMultiplayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MultiplayerMinigame");
    }
}
