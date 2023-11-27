using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public struct AbilityData
{
    public string Name;
    public int Level;
    public float Damage;
    public int ProjectilesPerShot;
    public float ShotSpread; //How much spread between projectiles on a single shot if there are many projectiles
    public float CoolDown;
    public float ProjectileSpeed;
    public float ProjectileSize;
    public int Repititions; //This is to deal with things like 3 round burst
    public float RepititionsDelay; //How much time between shots if repeating
    public float RelativeMotionScale; //How much does the ships motion impact the projectile speed between 0 and 1
    public List<string> Tags;
}

public class GameplayAbility : MonoBehaviour
{
    public AbilityData AbilityData = new AbilityData();

    public virtual bool CanActivateAbility()
    {
        Debug.Log("GameplayAbility:: Base Implementation of CanActivateAbility");
        return false;
    }
    public virtual bool ActivateAbility()
    {
        Debug.Log("GameplayAbility:: Base Implementation of ActivateAbility");
        return false;
    }

    public virtual bool AbilityInProgress()
    {
        Debug.Log("GameplayAbility:: Base Implementation of AbilityInProgress");
        return false;
    }
}
