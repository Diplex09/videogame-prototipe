using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] Knockback _knockback;
    [SerializeField] int _damagePower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out HealthSystem healthSystem))
            {
                if (healthSystem.enabled)
                {
                    healthSystem.ReceiveDamage(_damagePower);
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
                    knockback.ApplyKnockback(this.transform);
                }
            }
            else
            {
                Debug.LogWarning("No Knockback found on " + other.name);
            }
        }
    }
}