using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class GameController : MonoBehaviour
{
    public GameObject gameOverMenu, winMenu, pauseMenu;
    public CharController characterController;

    public GameObject allPlatforms;
    public LayerMask layerToRemove;

    public Image heart1, heart2, heart3, heart4, heart5;
    public Sprite fullHeart, emptyheart;

    bool paused;

    public TextMeshProUGUI score;
    int cash = -1;

    private void Start()
    {
        score.text = "" + cash;
    }

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

    public void LooseHealth()
    {
        if (heart5.sprite == fullHeart) heart5.sprite = emptyheart;
        else if (heart4.sprite == fullHeart) heart4.sprite = emptyheart;
        else if (heart3.sprite == fullHeart) heart3.sprite = emptyheart;
        else if (heart2.sprite == fullHeart) heart2.sprite = emptyheart;
        else if (heart1.sprite == fullHeart) heart1.sprite = emptyheart;
    }

    public void GainCoin()
    {
        cash++;
        score.text = "" + cash;
    }
}