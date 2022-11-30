using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject mainMenuPanel;
    public GameObject gamePanel;

    public void SwitchToGameMenu()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
}
