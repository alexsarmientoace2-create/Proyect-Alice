using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public float totalPoints;
    public float currentPoints;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
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

