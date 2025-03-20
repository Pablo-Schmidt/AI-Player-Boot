using Game;
using Game.Actions;
using Game.GUI;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Pablo_Schmidt
{
    public class Pablo_Schmidt_Controller : HeroController
    {
        [SerializeField]
        private EnemyController enemyTarget;

        public override ControllerAction Think()
        {
            enemyTarget = null;

            // Get the heros current HP
            int currentHP = this.HP;

            // Find a nearby enemy
            enemyTarget = FindNearestEnemy();


            if (currentHP > 3 && enemyTarget != null)
            {
                if (!IsNeighbor(enemyTarget))
                {
                    return new Action_MoveTowards(this, enemyTarget);
                }
                else
                {
                    if (currentHP == 4)
                        return new Action_Kick(this, enemyTarget);
                    else
                        return new Action_Attack(this, enemyTarget);
                }
               
            }

            if (currentHP <= 3)
            {
                Heart heart = Heart.AllHearts.Count > 0 ? Heart.AllHearts[0] : null;
                if (heart != null)
                {
                    return new Action_MoveTowards(this, heart.Node);
                }
            }
            //go to enemy in case it has a lot of XP.


            // Enemies or hearts are found
            return new Action_Wait(this, 1.0f);
        }

        private EnemyController FindNearestEnemy()
        {
            List<EnemyController> enemies = EnemyController.AllEnemies;
            EnemyController nearestEnemy = null;
            float minDistance = float.MaxValue;
            
            foreach(EnemyController enemy in enemies)
            {
                float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
                if (currentDistance < minDistance && enemy.IsAlive)
                {
                    minDistance = currentDistance;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy;
        }
    }
}
