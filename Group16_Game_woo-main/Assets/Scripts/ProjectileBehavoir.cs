using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehavoir : MonoBehaviour
{
    public float Damage = 20.0f;
    public float Speed = 1.0f;
    public float LifeTime = 2.0f;

    public GameObject CollisionVFX;
    public GameObject CollisionSound;
    public GameObject TrailSound;

    private Rigidbody _rb;
    string OwnerTag;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    { 
    }

    public void Initialize(float InDamage, float InSpeed, Vector3 Direction, Vector3 RelativeMotion)
    {
        //Destroy(this, LifeTime);
        Invoke("FinishDestroy", LifeTime);
        Damage = InDamage; 
        Speed = InSpeed;
        _rb.velocity = (Direction * Speed) + RelativeMotion;
    }


    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag(other.tag))
        {
            return;
        }

        if(other.tag == "Prop")
        {
            FinishDestroy();
        }

        if (
            (tag == "Player" && other.tag == "Enemy") ||
            (tag == "Enemy" && other.tag == "Player"))
        {
            StatManager SM = other.GetComponentInParent<StatManager>();

            if (SM != null)
            {
                SM.IncrementHealth(-Damage);
                FinishDestroy();
            }
        }
    }

    private void FinishDestroy()
    {
        Destroy(this.gameObject);
    }
}
