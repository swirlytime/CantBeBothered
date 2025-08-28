using Interfaces;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")] public float MaxHealth = 5;
    private float _currentHealth;

    public float CurrentHealth => _currentHealth;
    
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        foreach (var receiver in GetComponents<IDamageReceived>())
            receiver.OnDamageTaken();

        if (_currentHealth <= 0)
            OnDeath();
        if (_currentHealth > MaxHealth)
            _currentHealth = MaxHealth;
    }
    
    private void Awake()
    {
        _currentHealth = MaxHealth;
    }

    public void RestoreFullHealth()
    {
        _currentHealth = MaxHealth;
    }

    public void OnDeath()
    {
        var handler = GetComponent<IDeathHandler>();
        handler?.OnDeath();
    }
}
