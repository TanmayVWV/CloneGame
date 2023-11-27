using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnePlayer : MonoBehaviour
{
    public static GameObject instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
