using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartStageSelect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(StageSelectPanel.GetHighestLevelIndex());
    }
}
