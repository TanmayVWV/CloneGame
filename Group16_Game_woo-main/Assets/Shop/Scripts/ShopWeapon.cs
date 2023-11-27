using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Shop/Weapon", fileName = "ShopWeapon_")]
public class ShopWeapon : ScriptableObject
{
    public string Name;
    public int Cost;
    public Image Icon;
    public GameObject WeaponObject;
}
