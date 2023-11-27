/* Contributors: Zakcary Mowbray */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class TrickleSpawner : MonoBehaviour
{
    private float spawnTime;
    [SerializeField] private float spawnRate = 2.0f;
    [SerializeField] private float spawnDistance = 10.0f;
    [SerializeField] private int maxSpawnable = 10;
    public int numSpawned = 0;

    [SerializeField] public GameObject[] enemyPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.Log("No prehabs set for trickle spawner");
            return;
        }

        if (Time.time > spawnTime && numSpawned < maxSpawnable) {
            float theta = Random.Range(0.0f, 2.0f * Mathf.PI);
            if (GameManager.instance.player != null)
            {
                float x = Mathf.Cos(theta) * spawnDistance + GameManager.instance.player.transform.position.x;
                float y = 0;
                float z = Mathf.Sin(theta) * spawnDistance + GameManager.instance.player.transform.position.z;

                GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(x, y, z), transform.rotation);
                enemy.GetComponent<Enemy>().target = GameManager.instance.player;
                enemy.GetComponent<Enemy>().spawner = this;

                spawnTime = Time.time + spawnRate;
                numSpawned++;
            }
        }

    }

    /** 
     * Decrements the number of enemies this spawner has spawned
     **/
    public void EnemyKilled()
    {
        numSpawned--;
    }
}
