using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{

    public static int tutorialStep;
    [SerializeField] private GameObject pointerFirstDialogue;
    [SerializeField] private GameObject pointerFifthDialogue;
    [SerializeField] private GameObject firstDialoguePanel;
    [SerializeField] private GameObject secondDialoguePanel;
    [SerializeField] private GameObject thirdDialoguePanel;
    [SerializeField] private GameObject fourthDialoguePanel;
    [SerializeField] private GameObject fifthDialoguePanel;
    [SerializeField] private GameObject skipTutorialButton;

    private List<GameObject> allGameObjects;


    void Start()
    {
        tutorialStep = 0;

        allGameObjects = new List<GameObject>() { pointerFirstDialogue, firstDialoguePanel,
            secondDialoguePanel,
            thirdDialoguePanel,
            fourthDialoguePanel,
            pointerFifthDialogue, fifthDialoguePanel,
            skipTutorialButton};

        HideAll();

        pointerFirstDialogue.SetActive(true);
        firstDialoguePanel.SetActive(true);
        skipTutorialButton.SetActive(true);
    }

    public void FirstStep()
    {
        pointerFirstDialogue.SetActive(false);
        firstDialoguePanel.SetActive(false);
        secondDialoguePanel.SetActive(true);
        tutorialStep++;
    }

    public void SecondStep()
    {
        secondDialoguePanel.SetActive(false);
        thirdDialoguePanel.SetActive(true);
        tutorialStep++;
    }
    
    public void ThirdStep()
    {

        thirdDialoguePanel.SetActive(false);
        fourthDialoguePanel.SetActive(true);
        tutorialStep++;
    }

    public void FourthStep()
    {
        fourthDialoguePanel.SetActive(false) ;
        fifthDialoguePanel.SetActive(true);
        pointerFifthDialogue.SetActive(true);
        tutorialStep++;
    }

    private void HideAll()
    {
        foreach (GameObject obj in allGameObjects)
        {
            obj.SetActive(false);
        }
    }


    public void EndTutorial()
    {
        HideAll();
        tutorialStep = 9999;
    }



}
