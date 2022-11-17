using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public int health;
    public Text textHealth;
    public HealthBar healthBar;
    [SerializeField] Knockback _knockback;

    // Start is called before the first frame update
    void Start()
    {
        textHealth.text = "Health:" + health + "/100";
        healthBar.SetMaxHealth(health);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            health = health - 15;
            textHealth.text = "Health:" + health + "/100";
            healthBar.SetHealth(health);
            _knockback.ApplyKnockback(other.transform);
            //_healthBar.fillAmount = 50 / 100;

        }
    }

}