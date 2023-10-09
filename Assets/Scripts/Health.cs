using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;

    public bool IsDead { get; protected set; }
    private float _currentHealth;
    
    void Start()
    {
        IsDead = false;
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        IsDead = true;
        Destroy(gameObject);
    }
}
