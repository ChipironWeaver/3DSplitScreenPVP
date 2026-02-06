using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private float _currentHealth;

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}