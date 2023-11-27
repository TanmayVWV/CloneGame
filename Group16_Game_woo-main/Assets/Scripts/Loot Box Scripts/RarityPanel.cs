using System;
using UnityEngine;
using UnityEngine.UI;

namespace Loot_Box_Scripts
{
    public class RarityPanel : MonoBehaviour
    {
        public Image image;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        public void ChangeBorderColor(Rarity rewardRarity)
        {
            switch (rewardRarity)
            {
                case Rarity.Common:
                    image.color = Color.gray;
                    break;
                case Rarity.Uncommon:
                    image.color = Color.green;
                    break;
                case Rarity.Rare:
                    image.color = Color.cyan;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
