using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;

    private void Awake()
    {
        stageText.text = "Stage " + SceneManager.GetActiveScene().buildIndex;
    }
}
