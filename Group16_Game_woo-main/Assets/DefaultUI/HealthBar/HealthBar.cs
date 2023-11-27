using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] RectTransform healthTransform;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthImage;
    [SerializeField] Canvas canvas;

    private float yPos;
    private float minX;
    private float maxX;

    // Start is called before the first frame update
    void Start()
    {
        yPos = healthTransform.anchoredPosition.y;
        maxX = healthTransform.anchoredPosition.x;
        minX = (healthTransform.anchoredPosition.x - healthTransform.rect.width);

        GameObject player = GameManager.instance.player;
        if (player != null )
        {
            StatManager statManager = player.GetComponent<StatManager>();
            UpdateHealth(statManager.GetHealth(), statManager.GetMaxHealth());
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (currentHealth >= 0 &&  currentHealth <= maxHealth) 
        {
            healthText.text = "Health: " + currentHealth + " / " + maxHealth;

            float currentX = mapValues(currentHealth, 0, maxHealth, minX, maxX);
            healthTransform.anchoredPosition = new Vector3(currentX, yPos);

            if (currentHealth > maxHealth / 2)
            {
                healthImage.color = new Color32((byte)mapValues(currentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);
            }
            else
            {
                healthImage.color = new Color32(255, (byte)mapValues(currentHealth, 0, maxHealth / 2, 0, 255), 0, 255);
            }
        }
    }

    private float mapValues(float x, float inMin, float inMax, float outMin, float outMax) 
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
