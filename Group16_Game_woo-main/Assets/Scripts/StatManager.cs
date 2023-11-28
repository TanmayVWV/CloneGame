using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    // object's variables
    // *** THESE ARE THE ADJUSTABLE SPECIFIC VARIABLES LOOK HERE PLS!!! *** TO BE CHANGED IDK WHAT PEOPLE WANT/NEED
    [SerializeField] private float maxHealth = 100.0f;     // max HP stat
    [SerializeField] private float defense = 100.0f;    // damage reduction stat
    [SerializeField] private float attack = 100.0f;     // damage boost stat
    [SerializeField] private float handling = 100.0f;    // handling stat
    [SerializeField] private float speed = 1.0f;      // forward speed stat
    [SerializeField] private int size = 1; // size of ship
    [SerializeField] private int maxSize = 8;
    [SerializeField] private float maxSpeed = 60;
    [SerializeField] private float maxHandling = 1;
    [SerializeField] private float damage = 50.0f;

    // these variables are the ones used by the movement function to control movement stats
    [SerializeField] private float accelerationSpeed = 7.0f;
    [SerializeField] private float decelerationSpeed = 7.0f;
    [SerializeField] private float brakingSpeed = 12.0f;

    // other variables used in script
    private float currentHealth = 0.0f; // not used currently

    public void Start()
    {
        currentHealth = maxHealth;
        if(CompareTag("Player"))
        {
            GameObject DefaultUI = GameObject.Find("DefaultUI");
            if (DefaultUI != null)
            {
                HealthBar HB = DefaultUI.GetComponentInChildren<HealthBar>();
                if (HB != null)
                {
                    HB.UpdateHealth(currentHealth, maxHealth);
                }
            }
        }
        
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public bool IncrementHealth(float amount)
    {
        if (currentHealth + amount > maxHealth)
        {
            amount = maxHealth - currentHealth;
        }
        currentHealth += amount;
        if(CompareTag("Player")) 
        {
            GameObject DefaultUI = GameObject.Find("DefaultUI");
            if (DefaultUI != null)
            {
                HealthBar HB = DefaultUI.GetComponentInChildren<HealthBar>();
                if (HB != null)
                {
                    HB.UpdateHealth(currentHealth, maxHealth);
                }
            }
        }


        if(currentHealth <= 0)
        {
            if(CompareTag("Player"))
            {
                GameManager.instance.HandleDeath();
            }
            if(CompareTag("Enemy"))
            {
                Enemy ES = GetComponent<Enemy>();
                if(ES != null)
                {
                    ES.HandleDeath();
                }

            }
            PlayerDied();
        }
        return true;
    }
    private void PlayerDied()
    {
        LevelManager.instance.GameOver();
        gameObject.SetActive(false);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IncrementMaxHealth(float amount)
    {
        maxHealth += amount;
        if (CompareTag("Player"))
        {
            GameObject DefaultUI = GameObject.Find("DefaultUI");
            if(DefaultUI != null)
            {
                HealthBar HB = DefaultUI.GetComponentInChildren<HealthBar>();
                if (HB != null)
                {
                    HB.UpdateHealth(currentHealth, maxHealth);
                }
            }
            
        }
        return true;
    }

    public float GetHandling()
    {
        return handling;
    }

    public bool IncrementHandling(float amount)
    {
        if (handling + amount <= maxHandling)
        {
            handling += amount;
            return true;
        }
        return false;
    }

    public int GetSize()
    {
        return size;
    }

    public bool IncrementSize(int amount)
    {
        if (size+amount < maxSize)
        {
            size += amount;
            return true;
        }

        return false;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool IncrementSpeed(float amount)
    {
        if (speed + amount <= maxSpeed)
        {
            speed += amount;
            return true;
        }
        return false;
    }

    public float GetAcceleration()
    {
        return accelerationSpeed;
    }

    public bool IncrementAcceleration(float amount)
    {
        accelerationSpeed += amount;
        return true;
    }

    public float GetDeceleration()
    {
        return decelerationSpeed;
    }

    public bool IncrementDeceleration(float amount)
    {
        decelerationSpeed += amount;
        return true;
    }

    public float GetBraking()
    {
        return brakingSpeed;
    }

    public bool IncrementBraking(float amount)
    {
        brakingSpeed += amount;
        return true;
    }

    public float Damage
    {
        get { return damage; }
        set { damage += value; }
    }

}
