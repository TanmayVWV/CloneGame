using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop_UpgradeButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Cost;
    [SerializeField] Image Icon;
    [SerializeField] Image buttonColor;

    UnityAction<ShopUpgrade> OnSelectedFn;
    ShopUpgrade upgrade;

    private int Increment;

    public void Bind(ShopUpgrade shopUpgrade, UnityAction<ShopUpgrade> onSelectedFn)
    {
        upgrade = shopUpgrade;
        Name.text = shopUpgrade.Name;
        Cost.text = $"${shopUpgrade.Cost}";
        Icon = shopUpgrade.Icon;
        OnSelectedFn = onSelectedFn;
        Increment = shopUpgrade.Increment;
    }

    public void OnClick()
    {
        OnSelectedFn.Invoke(upgrade);
    }

    public void OnHover()
    {
        Color c = buttonColor.color;
        c.a = 0.229f;
        buttonColor.color = c;
    }

    public void OnEndHover()
    {
        Color c = buttonColor.color;
        c.a = 0.329f;
        buttonColor.color = c;
    }
}
