using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;
    [SerializeField] Image staminaFill;
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI pointsUI;
    [SerializeField] GameObject playerGO;
    PlayerController playerStamina;
    HealthSystem playerHealth;
    private GameManager gameManager;
    private GameObject gameManagerGO;


    private void Awake()
    {
        gameManagerGO = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gameManagerGO.GetComponent<GameManager>();
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        playerStamina = playerGO.GetComponent<PlayerController>();
        playerHealth = playerGO.GetComponent<HealthSystem>();

        ChangePoints();
    
    }

    public void ChangeHealthUI()
    {
        healthFill.fillAmount = playerHealth.CurrentHealth / playerHealth.MaxHealth;
    }

    public void ChangeStaminaUI()
    {
        staminaFill.fillAmount = playerStamina.CurrentStamina / playerStamina.maxStamina;
    }

    public void ChangePoints() 
    {
        pointsUI.text = new string("Points : " + (gameManager.totalPoints));
    }
}
