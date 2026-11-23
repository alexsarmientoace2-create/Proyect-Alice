using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public float totalPoints;
    public float currentPoints;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void ResetScore()
    {
        totalPoints = 0;
        currentPoints = 0;
    }

    public void EqualScores()
    {
        totalPoints = currentPoints;
    }

    public void AddScores()
    {
        currentPoints = totalPoints + currentPoints;
    }

}

