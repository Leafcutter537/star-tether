using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

public class SlowAntimatterButton : MonoBehaviour
{
    [SerializeField] private SlowAntimatterEvent slowAntimatterEvent;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string slowedText;
    [SerializeField] private string normalText;
    private bool isSlowed;

    public void Slow()
    {
        slowAntimatterEvent.Raise(this, null);
        isSlowed = !isSlowed;
        if (isSlowed)
            buttonText.text = slowedText;
        else
            buttonText.text = normalText;
    }
}
