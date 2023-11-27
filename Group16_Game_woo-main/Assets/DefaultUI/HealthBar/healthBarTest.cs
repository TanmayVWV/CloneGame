using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBarTest : MonoBehaviour
{

    [SerializeField] HealthBar healthBar;

    [SerializeField] int maxHealth;

    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //healthBar.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentHealth -= 10;
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentHealth += 10;
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
    }
}
