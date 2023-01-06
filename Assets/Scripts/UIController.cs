using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject victoryScreen;
    [SerializeField] private GameObject stopButton;
    [SerializeField] private GameObject startbutton;

    private void Awake()
    {
        victoryScreen.SetActive(false);
    }

    public void ShowVictory()
    {
        FindObjectOfType<StopStartController>().StopGame();
        victoryScreen.SetActive(true);
        stopButton.SetActive(false);
        startbutton.SetActive(false);
    }
}
