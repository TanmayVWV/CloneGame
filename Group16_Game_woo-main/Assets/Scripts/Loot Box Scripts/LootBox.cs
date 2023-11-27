using UnityEngine;

namespace Loot_Box_Scripts
{
    public class LootBox : MonoBehaviour
    {
        [SerializeField] private LootBoxManager lootBoxManager;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                lootBoxManager.PresentLootBox();
                Debug.Log("lootbox collected");
                Destroy(gameObject);
            }
        }
    }
}
