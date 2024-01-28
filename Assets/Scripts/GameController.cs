using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Metadata;

public class GameController : MonoBehaviour
{
    public GameObject gameOverMenu, winMenu, pauseMenu;
    public CharController characterController;

    public GameObject allPlatforms;
    public LayerMask layerToRemove;

    bool paused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseAndUnpause();
        }
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void FallThrough()
    {
        foreach (PlatformEffector2D child in allPlatforms.GetComponentsInChildren<PlatformEffector2D>(true))
        {
            child.colliderMask &= ~layerToRemove.value;
            Invoke("FallDone", .5f);
        }
    }

    void FallDone()
    {
        foreach (PlatformEffector2D child in allPlatforms.GetComponentsInChildren<PlatformEffector2D>(true))
        {
            child.colliderMask |= layerToRemove.value;
        }
    }

    public void PauseAndUnpause()
    {
        paused = !paused;

        if (paused)
        {
            characterController.inputDisabled = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            characterController.inputDisabled = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void FinishLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            characterController.inputDisabled = true;
            winMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}