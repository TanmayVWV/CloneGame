using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop_StatText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Value;
    
    public void Bind(string name, int value)
    {
        Name.text = name;
        Value.text = value.ToString();
    }

    public void Increment(int amount)
    {
        int current = int.Parse(Value.text);
        current += amount;
        Value.text = current.ToString();
    }
}
