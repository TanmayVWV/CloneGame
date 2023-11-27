using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WantedLevel : MonoBehaviour
{
    [SerializeField] RectTransform visualTransform;

    private float yPos;
    private float minX;
    private float maxX;

    // Start is called before the first frame update
    void Start()
    {
        yPos = visualTransform.anchoredPosition.y;
        maxX = visualTransform.anchoredPosition.x;
        minX = (visualTransform.anchoredPosition.x - visualTransform.rect.width);
        UpdateWanted(0, 1);
    }

    public void UpdateWanted(float currentWanted, float maxWanted)
    {
        if (currentWanted >= 0 && currentWanted <= maxWanted)
        {
            float currentX = mapValues(currentWanted, 0, maxWanted, minX, maxX);
            visualTransform.anchoredPosition = new Vector3(currentX, yPos);
        }

    }

    private float mapValues(float x, float inMin, float inMax, float outMin, float outMax) 
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
