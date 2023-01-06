using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StopStartController : MonoBehaviour
{

    public static bool gameIsActive;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject stopButton;

    private void Awake()
    {
        stopButton.SetActive(false);
        gameIsActive = false;
    }

    public void StartGame()
    {
        foreach (Launcher launcher in FindObjectsOfType<Launcher>())
        {
            launcher.Launch();
        }
        startButton.SetActive(false);
        stopButton.SetActive(true);
        gameIsActive = true;
    }

    public void StopGame()
    {
        foreach (Projectile projectile in FindObjectsOfType<Projectile>())
        {
            projectile.RemoveStarTether();
            Destroy(projectile.gameObject);
        }
        foreach (Generator generator in FindObjectsOfType<Generator>())
        {
            generator.RemoveInstance();
            generator.Generate();
        }
        startButton.SetActive(true);
        stopButton.SetActive(false);
        gameIsActive = false;
    }

    public void NextStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
