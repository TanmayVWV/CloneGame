using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Shop/Upgrade", fileName = "ShopUpgrade_")]
public class ShopUpgrade : ScriptableObject
{
    public string Name;
    public int Cost;
    public Image Icon;
    public int Increment;
    public int index;
}
