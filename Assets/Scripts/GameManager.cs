using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GroundPiece[] allGroundPieces;

    AudioSource gameMusic;
    [SerializeField] List<AudioClip> gameMusicClips = new List<AudioClip>();
    int randomGameMusicClip;

    [SerializeField] GameObject levelCompleteUI;
    [SerializeField] Button nextButton;
    Button nextButtonComponent;

    private void Start()
    {
        levelCompleteUI.SetActive(false);
        SetupNewLevel();
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            DisableNextButton();
        } else
        {
            EnableNextButton();
        }
    }

    private void PlayRandomGameMusic()
    {
        if(gameMusic.isPlaying)
        {
            gameMusic.Stop();
        }
        randomGameMusicClip = Random.Range(0, gameMusicClips.Count);
        gameMusic.clip = gameMusicClips[randomGameMusicClip];
        gameMusic.Play();
    }

    private void Awake()
    {
        if(nextButton != null)
        {
            nextButtonComponent = nextButton.GetComponent<Button>();
        }
        gameMusic = GetComponent<AudioSource>();
        PlayRandomGameMusic();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {
            levelCompleteUI.SetActive(true);
        }
    }

    void DisableNextButton()
    {
        if(nextButton != null)
        {
            nextButtonComponent.interactable = false;
        } 
    }

    void EnableNextButton()
    {
        if(!nextButtonComponent.interactable)
        {
            nextButtonComponent.interactable = true;
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
