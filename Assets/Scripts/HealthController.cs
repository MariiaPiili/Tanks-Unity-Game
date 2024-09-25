using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public Action<int> ChangedHealth;

    [SerializeField] private int _maxHealth;

    private int _currentHealth;

    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        private set
        {
            _currentHealth = value;
            ChangedHealth.Invoke(_currentHealth);
        }
    }

    public int MaxHealth => _maxHealth;

    private void Start()
    {
        CurrentHealth = _maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            CurrentHealth--;
            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
