using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    BoxCollider2D boxCollider;

    float currentHealth;
    [SerializeField] float maxHealth = 100;
    [SerializeField] bool canDie = true;

    
    public UnityEvent OnHealthChange;
    public UnityEvent OnDamageTaken;
    public UnityEvent OnHealthIncreased;
    public UnityEvent OnDeath;

    //getters, la flechita "=>" indica que la primera variable sera el getter de la segunda, (no confundir con mayor o igual ">=")
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

   

    private void Start()
    {
        ChangeHealth(maxHealth);
    }

    public void ChangeHealth(float newHealth)
    {
        currentHealth = newHealth;
        OnHealthChange.Invoke();
    }

    public void TakeDamage(float damage)
    {
        ChangeHealth(Mathf.Max(currentHealth - damage, 0));
        OnDamageTaken.Invoke();

        if (currentHealth <= 0 && canDie == true)
        {
            OnDeath.Invoke();
        }
    }

    public void IncreaseHealth(float amoutToHeal)
    {
        ChangeHealth(Mathf.Min(maxHealth, currentHealth + amoutToHeal));
        OnHealthIncreased.Invoke();
    }
}
