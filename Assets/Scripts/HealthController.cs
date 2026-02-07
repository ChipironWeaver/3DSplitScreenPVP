using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _invicibilityTime;
    [SerializeField] private bool  _invicible;


    public void Start()
    {
        _maxHealth = _currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!_invicible)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Death();
            }
            _invicible = true;
            Invoke(nameof(RemoveInvincibility), _invicibilityTime);
        }
    }

    public void Heal(int heal)
    {
        _currentHealth += heal;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public void Death()
    {
        Debug.Log("Death");
    }

    private void RemoveInvincibility()
    {
        _invicible = false;
    }
}