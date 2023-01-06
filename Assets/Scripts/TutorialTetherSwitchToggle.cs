using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialTetherSwitchToggle : TetherSwitchToggle
{

    [SerializeField] private bool isFirst;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (TutorialController.tutorialStep > 5)
        {
            base.OnPointerClick(eventData);
        }
        else if (TutorialController.tutorialStep == 0 & isFirst == true)
        {
            FindObjectOfType<TutorialController>().FirstStep();
            base.OnPointerClick(eventData);
        }
    }

}
