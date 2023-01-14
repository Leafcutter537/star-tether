using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStartStopController : StopStartController
{
    
    public void TutorialStartGame()
    {
        if (TutorialController.tutorialStep == 1)
        {
            FindObjectOfType<TutorialController>().SecondStep();
            StartGame();
        }
        if (TutorialController.tutorialStep > 10)
        {
            StartGame();
        }
    }

    public void TutorialStopGame()
    {
        if (TutorialController.tutorialStep > 10)
            StopGame();
    }

}
