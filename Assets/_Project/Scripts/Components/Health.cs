using UnityEngine;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    
    public int healthMax = 100;
    
    private int _healthCurrent = 10;

    // Health Property
    public int HealthCurrent
    {
        get => _healthCurrent;
        set
        {
            int previousHealth = _healthCurrent;
            _healthCurrent = Mathf.Clamp(value, 0, healthMax);
            OnHealthChanged?.Invoke(_healthCurrent);
            if (_healthCurrent <= 0)
                OnDeath?.Invoke();
        }
    }

    private void Start()
    {
       // _healthCurrent = healthMax;
    }

    public void HealHeath(int amount)
    {
        if (amount <= 0) return;
        HealthCurrent += amount;
        Debug.unityLogger.Log($"{gameObject.name}: Health healed by {amount}");
    }

    public void TakeHealthDamage(int amount)
    {
        if (amount <= 0) return;
        HealthCurrent -= amount;
        Debug.unityLogger.Log($"{gameObject.name}: Health damaged by {amount}");
    }   
}