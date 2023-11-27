using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UpdateAmount(GameManager.instance.PlayerMoney);
    }

    public void UpdateAmount(int amount)
    {
        string amountText = amount.ToString("C0", CultureInfo.GetCultureInfo("en-US"));

        text.text = amountText;
    }
}
