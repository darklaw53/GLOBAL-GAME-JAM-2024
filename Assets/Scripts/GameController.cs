using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Metadata;

public class GameController : MonoBehaviour
{
    public GameObject gameOverMenu;
    public CharController characterController;

    public GameObject allPlatforms;
    public LayerMask layerToRemove;

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
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
}