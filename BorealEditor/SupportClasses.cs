using BorealEditor.Challenges;
using BorealEditor.Components;
using BorealEditor.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BorealEditor.Boilerplate
{
    public class BorealEnemy : MonoBehaviour
    {
        public EnemyIdentifier eid;
        public EnemySpawner spawner;

        private void Awake()
        {
            eid = GetComponentInChildren<EnemyIdentifier>();
        }

        private void Update()
        {
            if (eid && eid.dead)
            {
                spawner.enemyDead = true;
                this.enabled = false;
            }
        }
    }

    // Enums

    public enum HitterType
    {
        Hitter,
        HitterWeapon
    }

    public enum BonusType
    {
        Default,
        Supercharge,
        DualWield
    }
}
