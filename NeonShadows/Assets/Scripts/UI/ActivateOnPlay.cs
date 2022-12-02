using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivateOnPlay : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().enabled = true;
    }
}