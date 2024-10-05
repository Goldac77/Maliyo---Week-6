using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GroundPiece[] allGroundPieces;

    AudioSource gameMusic;
    [SerializeField] List<AudioClip> gameMusicClips = new List<AudioClip>();
    int randomGameMusicClip;

    [SerializeField] GameObject levelCompleteUI;

    private void Start()
    {
        SetupNewLevel();
        if (levelCompleteUI.activeInHierarchy)
        {
            levelCompleteUI.SetActive(false);
        }
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void PlayRandomGameMusic()
    {
        if(gameMusic.isPlaying)
        {
            gameMusic.Stop();
        }
        randomGameMusicClip = Random.Range(0, gameMusicClips.Count);
        gameMusic.PlayOneShot(gameMusicClips[randomGameMusicClip], 0.7f);
    }

    private void Awake()
    {
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

    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene("MainMenu");
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
