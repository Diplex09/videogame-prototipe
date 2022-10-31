using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MakeDamage : MonoBehaviour
{
    [SerializeField] private int _damagePower = 10;
    [SerializeField] private string _tagToDamage = "Player";
    [SerializeField] private UnityEvent OnTrigger;
    [SerializeField] private PlayerAttack _playerAttack;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(_tagToDamage) && _playerAttack.Attacking) {
            if (other.TryGetComponent(out HealthSystem healthSystem)) {
                    Debug.Log($"Triggered {gameObject.name} on {other.gameObject.name}");
                if (healthSystem.enabled) {
                    healthSystem.ReceiveDamage(_damagePower);
                    OnTrigger?.Invoke();
                }
            }
            else {
                Debug.LogWarning("No HealthSystem found on " + other.name);
            }
        }
    }
}
