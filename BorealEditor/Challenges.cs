using BorealEditor.Boilerplate;
using BorealEditor.Components;
using BorealEditor.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BorealEditor.Challenges
{
    public class ContactChallenge : MonoBehaviour
    {
        public ChallengeType challengeType;
        public bool checkForNoEnemies;
        public bool evenIfPlayerDead;

        private void Awake()
        {
            ChallengeTrigger trigger = gameObject.AddComponent<ChallengeTrigger>();
            trigger.type = challengeType;
            trigger.checkForNoEnemies = checkForNoEnemies;
            trigger.evenIfPlayerDead = evenIfPlayerDead;
        }
    }

    public class KillChallenge : MonoBehaviour
    {
        public int kills;

        private void Awake()
        {
            KillAmountChallenge trigger = gameObject.AddComponent<KillAmountChallenge>();
            trigger.kills = kills;
        }
    }

    public class NoKillChallenge : MonoBehaviour
    {
        private void Awake()
        {
            PacifistChallenge trigger = gameObject.AddComponent<PacifistChallenge>();
        }
    }

    public class SlideChallenge : MonoBehaviour
    {
        public float slideLength;

        private void Awake()
        {
            SlideLengthChallenge trigger = gameObject.AddComponent<SlideLengthChallenge>();
            trigger.slideLength = slideLength;
        }
    }

    public class KillTimeChallenge : MonoBehaviour
    {
        public float timeToBeat;
        public EnemySpawner enemySpawner;

        private void Awake()
        {
            enemySpawner.SetTimeChallenge(timeToBeat);
        }
    }

    public class SpeedChallenge : MonoBehaviour
    {
        public float timeToBeat;

        private TimeChallenge trigger;

        private void Awake()
        {
            trigger = gameObject.AddComponent<TimeChallenge>();
            trigger.time = timeToBeat;
        }

        private void Update()
        {
            if (EndZone.Instance.FinalDoor.aboutToOpen)
                trigger.ReachedGoal();
        }

        public void ReachGoal()
        {
            trigger.ReachedGoal();
        }
    }

    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class UseOnlyChallenge : MonoSingleton<UseOnlyChallenge>
    {
        public string[] weaponsIDs;
        public HitterType hitterType;

        public void Start()
        {
            ChallengeManager.Instance.challengeDone = true;
        }

        public void CheckKill(string ID)
        {
            if (!weaponsIDs.Contains(ID))
                ChallengeManager.Instance.challengeFailed = true;
            else
                ChallengeManager.Instance.challengeDone = true;
        }
    }
}
