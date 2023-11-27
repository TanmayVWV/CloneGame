using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop_WeaponButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Cost;
    [SerializeField] Image Icon;
    [SerializeField] Image buttonColor;

    UnityAction<ShopWeapon> OnBeginDragFn;
    UnityAction<ShopWeapon> OnDragFn;
    UnityAction<GameObject, ShopWeapon> OnEndDragFn;
    ShopWeapon weapon;
    GameObject weaponObject;


    public void Bind(ShopWeapon shopWeapon, UnityAction<ShopWeapon> 
        onBeginDragFn, UnityAction<ShopWeapon> onDragFn, UnityAction<GameObject, ShopWeapon> onEndDragFn)
    {
        weapon = shopWeapon;
        Name.text = shopWeapon.Name;
        Cost.text = $"${shopWeapon.Cost}";
        Icon = shopWeapon.Icon;
        weaponObject = shopWeapon.WeaponObject;
        OnBeginDragFn = onBeginDragFn;
        OnDragFn = onDragFn;
        OnEndDragFn = onEndDragFn;
    }

    public void OnBeginDrag()
    {
        OnBeginDragFn.Invoke(weapon);
    }

    public void OnDrag()
    {
        OnDragFn.Invoke(weapon);
    }

    public void OnDragEnd()
    {
        OnEndDragFn.Invoke(gameObject, weapon);
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
