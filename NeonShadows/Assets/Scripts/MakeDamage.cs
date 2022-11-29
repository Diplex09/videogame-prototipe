using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDamage : MonoBehaviour
{
    [SerializeField] private int _damagePower = 10;
    [SerializeField] private string _tagToDamage = "Enemy";
    [SerializeField] private PlayerAttack _playerAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tagToDamage) && _playerAttack.Attacking)
        {
            if (other.TryGetComponent(out HealthSystem healthSystem))
            {
                Debug.Log($"Triggered {gameObject.name} on {other.gameObject.name}");
                if (healthSystem.enabled)
                {
                    healthSystem.ReceiveDamage(_damagePower);
                    Debug.Log("Hola");
                }
            }
            else
            {
                Debug.LogWarning("No HealthSystem found on " + other.name);
            }

            if (other.TryGetComponent(out Knockback knockback))
            {
                if (knockback.enabled)
                {
                    knockback.ApplyKnockback(_playerAttack.transform);
                }
            }
            else
            {
                Debug.LogWarning("No Knockback found on " + other.name);
            }
        }
    }
}