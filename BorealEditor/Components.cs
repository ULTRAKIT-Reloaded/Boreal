using BorealEditor.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULTRAKIT.Extensions;
using UnityEngine.Events;
using UnityEngine;
using BorealEditor.Boilerplate;

namespace BorealEditor.Components
{
    public class Tagger : MonoBehaviour
    {
        [SerializeField]
        private string tagstring;

        public void Awake()
        {
            gameObject.tag = tagstring;
        }
    }

    public class EnemyGroup : MonoBehaviour
    {
        public EnemySpawner[] SpawnerGroup;
        public UnityEvent OnEnemiesKilled;

        private bool activated = false;

        private void Update()
        {
            if (activated)
                return;
            if (!SpawnerGroup.Where(s => s.enemyDead == false).Any())
            {
                activated = true;
                OnEnemiesKilled.Invoke();
                base.enabled = false;
            }
        }
    }

    public class EnemySpawner : MonoBehaviour
    {
        public EnemyType _EnemyType;
        public string _commonOverride = string.Empty;

        private GameObject prefab;
        private bool timeKillChallenge;
        private float time;

        [HideInInspector]
        public bool enemyDead = false;

        private void Start()
        {
            if (_commonOverride != string.Empty && _commonOverride.Length > 0)
            {
                prefab = DazeExtensions.PrefabFind(null, "common", _commonOverride);
            }
            else
            {
                prefab = BorealManager.Instance.database.enemies.Where(e => e.enemyType == _EnemyType).First().gameObject;
            }
        }

        public void Spawn()
        {
            GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
            enemy.AddComponent<BorealEnemy>().spawner = this;
            if (timeKillChallenge)
                enemy.AddComponent<SpeedKillChallenge>().timeLeft = time;
        }

        public void SetTimeChallenge(float time)
        {
            this.time = time;
            timeKillChallenge = true;
        }
    }

    public class Trigger : MonoBehaviour
    {
        public bool OneTimeOnly;
        public UnityEvent FunctionToCall;
        private bool _active;

        private void OnTriggerEnter(Collider other)
        {
            if (!_active && other.tag == "Player")
            {
                _active = true;
                FunctionToCall.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!OneTimeOnly && _active && other.tag == "Player")
            {
                _active = false;
            }
        }
    }

    public class RenderFixer : MonoBehaviour
    {
        public string LayerName;

        public void Start()
        {
            PeterExtensions.RenderObject(gameObject, LayerMask.NameToLayer(LayerName));
        }
    }

    public class DefaultBonus : MonoBehaviour
    {
        public BonusType type;
        public int secretNumber;

        private void Awake()
        {
            if (type == BonusType.Default)
            {
                Bonus bonus = Instantiate(BorealManager.Instance._bonus, transform.position, transform.rotation, transform).GetComponent<Bonus>();
                bonus.secretNumber = secretNumber;
            }
            if (type == BonusType.Supercharge)
            {
                Bonus bonus = Instantiate(BorealManager.Instance._bonusSupercharge, transform.position, transform.rotation, transform).GetComponent<Bonus>();
                bonus.secretNumber = secretNumber;
            }
            if (type == BonusType.DualWield)
            {
                Bonus bonus = Instantiate(BorealManager.Instance._bonusDualWield, transform.position, transform.rotation, transform).GetComponent<Bonus>();
                bonus.secretNumber = secretNumber;
            }
        }
    }

    public class BorealBonus : MonoBehaviour
    {
        public BonusType type;
        public int secretNumber;
        public bool dontReplaceWithGhost;

        public GameObject ghostVersion;
        public GameObject breakEffect;
        public GameObject ghostBreakEffect;

        private void Awake()
        {
            Bonus bonus = gameObject.AddComponent<Bonus>();
            bonus.dontReplaceWithGhost = dontReplaceWithGhost;
            switch (type)
            {
                case BonusType.Supercharge: bonus.superCharge = true; break;
                case BonusType.DualWield: gameObject.AddComponent<DualWieldPickup>(); break;
            }
            if (!bonus.ghost || bonus.dontReplaceWithGhost)
            {
                bonus.breakEffect = breakEffect;
            }
            if (bonus.ghost && !bonus.dontReplaceWithGhost)
            {
                Bonus obj = Instantiate(ghostVersion, transform.position, transform.rotation).AddComponent<Bonus>();
                bonus.ghost = true;
                switch (type)
                {
                    case BonusType.Supercharge: obj.superCharge = true; break;
                    case BonusType.DualWield: obj.gameObject.AddComponent<DualWieldPickup>(); break;
                }
                obj.breakEffect = ghostBreakEffect;

                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
