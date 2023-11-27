using Player_Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonAbility : GameplayAbility
{
    public GameObject Emitter;
    public GameObject ProjectilePrefab;
    public GameObject OnShootParticleEffect;
    public GameObject OnShootSoundEffect;

    float LastShotTime = 0.0f;
    int RepitionsLeft = 0;
    
    public int Level = 1;
    public float Damage = 20;
    public int ProjectilesPerShot = 1;
    public float ShotSpread; //How much spread between projectiles on a single shot if there are many projectiles
    public float ShotSpreadVariance = 0.0f; //How much variation between shot spread per shot
    public float CoolDown = 1;
    public float ProjectileSpeed = 20;
    public float ProjectileSize = 1;
    public int Repititions = 1; //This is to deal with things like 3 round burst
    public float RepititionsDelay = 0.0f; //How much time between shots if repeating
    public float RelativeMotionScale = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        AbilityData.Level = Level;
        AbilityData.Damage = Damage;
        AbilityData.ProjectilesPerShot = ProjectilesPerShot;
        AbilityData.ShotSpread = ShotSpread;
        AbilityData.CoolDown = CoolDown;
        AbilityData.ProjectileSpeed = ProjectileSpeed;
        AbilityData.ProjectileSize = ProjectileSize;
        AbilityData.Repititions = Repititions;
        AbilityData.RepititionsDelay = RepititionsDelay;
        AbilityData.RelativeMotionScale = RelativeMotionScale;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool AbilityInProgress()
    {
        return RepitionsLeft > 0;
    }

    public override bool CanActivateAbility()
    {
        if(LastShotTime == 0.0f)
            return true;
        return (Time.time - LastShotTime) >= AbilityData.CoolDown
            && RepitionsLeft <= 0;
    }

    public override bool ActivateAbility()
    {
        if(!CanActivateAbility())
        {
            //Debug.Log("Could not activate ability");
            return false;
        }
        RepitionsLeft = AbilityData.Repititions;

        for (int i = 0; i < AbilityData.Repititions; i++)
        {
            Invoke("Shoot", AbilityData.RepititionsDelay * i);
        }

        return true;
    }

    protected void Shoot()
    {
        Vector3 ParticleSpawnPosition = Emitter ? Emitter.transform.position : transform.position;

        float SpreadVariance = Random.Range(-ShotSpreadVariance, ShotSpreadVariance);
        float CurrentShotSpread = Mathf.Max(0.0f, AbilityData.ShotSpread + SpreadVariance);

        float DeltaSpread = CurrentShotSpread / AbilityData.ProjectilesPerShot;
        int AngleDeviations = AbilityData.ProjectilesPerShot / 2;
        float InitialAngleOffset = -1 * DeltaSpread * AngleDeviations;

        for (int i = 0; i < AbilityData.ProjectilesPerShot; i++)
        {
            Vector3 Position;
            Quaternion Rotation;
            transform.GetLocalPositionAndRotation(out Position, out Rotation);
            Vector3 Forward = transform.forward;
            Forward = Quaternion.AngleAxis(InitialAngleOffset + DeltaSpread * i, Vector3.up) * Forward;
            SpawnProjectile(Forward);
        }
        RepitionsLeft--;
        if(RepitionsLeft <= 0)
        {
            LastShotTime = Time.time;
        }
    }

    protected void SpawnProjectile(Vector3 Direction)
    {
        Vector3 Positon = new Vector3(Emitter.transform.position.x, 0.0f, Emitter.transform.position.z);
        GameObject Projectile = Instantiate(ProjectilePrefab, Positon , Emitter.transform.rotation);
        Projectile.transform.localScale = Vector3.one * AbilityData.ProjectileSize;
        Physics.IgnoreCollision(Projectile.GetComponent<SphereCollider>(), GetComponentInParent<BoxCollider>(),true);
        Projectile.tag = tag;
        ProjectileBehavoir PB = Projectile.GetComponent<ProjectileBehavoir>();
        if(PB != null)
        {
            Vector3 RelativeMotion = new Vector3();
            Boat BoatScript = GetComponentInParent<Boat>();

            if( BoatScript != null )
            {
                RelativeMotion = BoatScript.GetVelocity() * AbilityData.RelativeMotionScale;
            }
            else
            {
                Debug.Log("No Boat Script Found");
            }

            PB.Initialize(Damage, ProjectileSpeed, Direction.normalized, RelativeMotion);
        }
    }
        

}
