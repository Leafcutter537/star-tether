using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeStage : MonoBehaviour
{
    [SerializeField] private StageSelectPanel stageSelectPanel;

    public void GoToSelectedStage()
    {
        int stageIndex = (stageSelectPanel.GetSelected() as StageSelectChoice).stageIndex;
        SceneManager.LoadScene(stageIndex + 1);
    }
}
