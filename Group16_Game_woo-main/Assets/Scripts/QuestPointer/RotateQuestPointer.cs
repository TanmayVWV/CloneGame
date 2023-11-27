/** Contributors: Jordan Planchot **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestPointer
{
    public class RotateQuestPointer : MonoBehaviour
    {
        //
        [SerializeField] GameObject player;
        public GameObject objective;

        [SerializeField] SpriteRenderer objectiveArrow;
        [SerializeField] SpriteRenderer objectiveIcon;

        [SerializeField] Sprite spr_enemyArrow;
        [SerializeField] Sprite spr_islandArrow;
        [SerializeField] Sprite spr_enemyIcon;
        [SerializeField] Sprite spr_islandIcon;


        // Start is called before the first frame update
        void Start()
        {
            objective = GameManager.instance.objectiveShip;
            objectiveArrow.sprite = spr_enemyArrow;
            objectiveIcon.sprite = spr_enemyIcon;
        }

        // Update is called once per frame
        void Update()
        {
            objective = GameManager.instance.objectiveCurrent;
            if (objective == GameManager.instance.objectiveIsland)
            {
                objectiveArrow.sprite = spr_islandArrow;
                objectiveIcon.sprite = spr_islandIcon;
            }
            else
            {
                objectiveArrow.sprite = spr_enemyArrow;
                objectiveIcon.sprite = spr_enemyIcon;
            }
            transform.LookAt(objective.transform.position);
        }
    }
}
