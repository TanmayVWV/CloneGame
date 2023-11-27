using UnityEngine;

namespace Loot_Box_Scripts
{
    [CreateAssetMenu(fileName = "New Reward", menuName = "Rewards/Reward")]
    public class LootBoxReward : ScriptableObject
    {
        public RewardType rewardType;
        public string rewardName;
        public Sprite icon;
        public Rarity rewardRarity;
        
        // Stat Upgrades
        public string statToMod;
        public float amountToChangeStat;
    }

    public enum RewardType
    {
        StatUpgrade
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare
    }
}