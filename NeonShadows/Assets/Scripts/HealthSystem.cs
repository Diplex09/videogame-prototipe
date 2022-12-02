using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.Shapes;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] Material _material;
    [SerializeField] private UnityEvent<int> OnReceiveDamage;
    [SerializeField] private UnityEvent OnZeroHealth;
    [SerializeField] private UnityEvent<int> OnReceiveHealth;
    //public HealthBar healthBar;

    public void Start()
    {
        _currentHealth = _maxHealth;
        _material.color = Color.white;
    }

    public int CurrentHealth {
        get => _currentHealth; 
        set => _currentHealth = value;
    }

    public int MaxHealth {
        get => _maxHealth; 
        set => _maxHealth = value;
    }

    public void ReceiveDamage(int damageAmount) {
        
        _currentHealth -= damageAmount;
        StartCoroutine(DamageBlink());
        OnReceiveDamage?.Invoke(CurrentHealth);
        if (CurrentHealth <= 0){
            OnZeroHealth?.Invoke();
        }
    }

    public void GainHealth(int gainAmount) {
        _currentHealth += gainAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        OnReceiveHealth?.Invoke(CurrentHealth);
    }

    IEnumerator DamageBlink()
    {
        for (int i = 0; i < 5; i++)
        {
            _material.color = Color.white;
            yield return new WaitForSeconds(0.15f);
            _material.color = Color.red;
            yield return new WaitForSeconds(0.15f);
        }
        _material.color = Color.white;
    }
}