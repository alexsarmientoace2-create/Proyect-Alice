using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject playerGO;
    public GameObject UIGO;
    public GameObject cineMachineCamera;
    public GameObject DeathUIGO;
    public GameObject WinUIGO;
    public float totalPoints;
    public float currentPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void ChangeMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        totalPoints = 0;
        currentPoints = 0;
    }

    public void ChangeTutorial()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        totalPoints = currentPoints;
    }

    public void ChangeLevel1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
        totalPoints = currentPoints;
    }

    public void Death()
    {
        Time.timeScale = 0;
        UIGO.SetActive(false);
        DeathUIGO.SetActive(true);
        totalPoints = currentPoints;
    }

    public void Win()
    {
        Time.timeScale = 0;
        UIGO.SetActive(false);
        WinUIGO.SetActive(true);
        currentPoints = totalPoints + currentPoints;
    }
}
