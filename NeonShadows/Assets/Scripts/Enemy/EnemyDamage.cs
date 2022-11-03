using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public float health;
    public Text textHealth;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        textHealth.text = "Health:" + health + "/100";
        //healthBar.SetMaxHealth(health);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")) {
            health = health - 15;
            textHealth.text = "Health:" + health + "/100";
            //_healthBar.fillAmount = 50 / 100;
            
        }
    }

}
