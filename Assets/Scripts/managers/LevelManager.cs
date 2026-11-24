using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] TextMeshProUGUI deathPointsUI;
    [SerializeField] TextMeshProUGUI winPointsUI;
    public GameObject playerGO;
    public GameObject UIGO;
    public GameObject cineMachineCamera;
    public GameObject DeathUIGO;
    public GameObject WinUIGO;
    private GameManager gameManager;
    private GameObject gameManagerGO;

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

        gameManagerGO = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gameManagerGO.GetComponent<GameManager>();
    }

    public void ChangeMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        gameManager.ResetScore();
    }

    public void ChangeTutorial()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        gameManager.EqualScores();
    }

    public void ChangeLevel1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
        gameManager.EqualScores();
    }

    public void Death()
    {
        deathPointsUI.text = new string("Points : " + gameManager.totalPoints);
        Time.timeScale = 0;
        UIGO.SetActive(false);
        DeathUIGO.SetActive(true);
        gameManager.EqualScores();
    }

    public void Win()
    {
        Time.timeScale = 0;
        UIGO.SetActive(false);
        WinUIGO.SetActive(true);
        gameManager.AddScores();
        winPointsUI.text = new string("Points : " + gameManager.currentPoints);
    }
}
