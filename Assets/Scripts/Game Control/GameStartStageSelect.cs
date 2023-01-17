using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartStageSelect : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(StageSelectPanel.GetHighestLevelIndex());
    }
}
