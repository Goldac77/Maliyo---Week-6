using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public void CasualGameMode()
    {
        gameManager.NextLevel();
    }

    public void HardGameMode()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
