using Player_Core;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Loot_Box_Scripts
{
    public class LootBoxManager : MonoBehaviour
    {

        [SerializeField] private GameObject lootBoxCanvas;
    
        public LootBoxReward[] commonUpgrades;
        public LootBoxReward[] uncommonUpgrades;
        public LootBoxReward[] rareUpgrades;

        // Reward button references
        [SerializeField] private GameObject choice1;
        [SerializeField] private GameObject choice2;
        [SerializeField] private GameObject choice3;
    
        // Loot item choices presented to player
        private LootBoxReward rewardItem1;
        private LootBoxReward rewardItem2;
        private LootBoxReward rewardItem3;
        LootBoxReward[] rewardItems; // Array holding all 3 choices available
    
        // Panels that display rarity of item 
        [SerializeField] private GameObject rarityPanel1;
        [SerializeField] private GameObject rarityPanel2;
        [SerializeField] private GameObject rarityPanel3;
        
        // reference to player
        [SerializeField] private GameObject player;
    
        // Start is called before the first frame update
        void Start()
        {
            lootBoxCanvas.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // Display canvas, get one upgrade per button, present player with 3 choices
        public void PresentLootBox()
        {
            // TODO pause game
            Time.timeScale = 0f;
            GameManager.instance.player.GetComponent<PlayerMovement>().DisableControls();
            // Show canvas
            lootBoxCanvas.gameObject.SetActive(true);
    
            // Randomly select 3 items
            rewardItem1 = SelectLootItem();
            rewardItem2 = SelectLootItem();
            rewardItem3 = SelectLootItem();
            // Add items to array
            rewardItems = new[] { rewardItem1, rewardItem2, rewardItem3 };
            
            Debug.Log("Items chosen");

            // Display item info on buttons
            choice1.GetComponentInChildren<Text>().text = rewardItem1.rewardName;
            choice2.GetComponentInChildren<Text>().text = rewardItem2.rewardName;
            choice3.GetComponentInChildren<Text>().text = rewardItem3.rewardName;

            // Set rarity panel colors
            rarityPanel1.GetComponent<RarityPanel>().ChangeBorderColor(rewardItem1.rewardRarity);
            rarityPanel2.GetComponent<RarityPanel>().ChangeBorderColor(rewardItem2.rewardRarity);
            rarityPanel3.GetComponent<RarityPanel>().ChangeBorderColor(rewardItem3.rewardRarity);
        }

        // Return one of the upgrade arrays based on rarity weight
        private LootBoxReward[] RandomRarity()
        {
            float number = Random.Range(1, 100);
        
            // 10% chance for rare 
            if (number <= 10)
            {
                return rareUpgrades;
            }
            // 30% chance for uncommon
            else if (number <= 40)
            {
                return uncommonUpgrades;
            }
            // 60% chance for common
            else
            {
                return commonUpgrades;
            }
        }

        // Get one item (reward) from a random rarity
        private LootBoxReward SelectLootItem()
        {
            // Select one of the arrays
            LootBoxReward[] upgradesArray = RandomRarity();
        
            // Select a random item from the array
            LootBoxReward selectedUpgrade = upgradesArray[Random.Range(0, upgradesArray.Length)];
        
            return selectedUpgrade;
        }

        // Gets players choice from button click
        public void ChooseReward(int buttonNum)
        {
            LootBoxReward selectedReward = rewardItems[buttonNum];
        
            // Call function to 'execute' reward
            //ExecuteReward(selectedReward);
        
            // TODO unpause game
            lootBoxCanvas.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            GameManager.instance.player.GetComponent<PlayerMovement>().EnableControls();
        }

        /*private void ExecuteReward(LootBoxReward reward)
        {
            // TODO add functions based on upgrade type
            switch (reward.rewardType)
            {
                case RewardType.StatUpgrade:
                    ExecuteStatUpgrade(reward);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExecuteStatUpgrade(LootBoxReward reward)
        {
            switch (reward.statToMod)
            {
                case "maxHealth":
                    player.GetComponent<PlayerMovement>().IncreaseMaxHealth(reward.amountToChangeStat);
                    break;
            }
        }*/
    
    
    
    
    }
}
