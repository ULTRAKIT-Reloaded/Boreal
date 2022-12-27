using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULTRAKIT.Extensions;
using UnityEngine.Events;
using UnityEngine;

namespace Boreal
{
    public class LevelLoader : MonoBehaviour
    {
        public static SpawnableObjectsDatabase database;
        public void Awake()
        {
            Debug.Log("Loading custom level");
            GameObject room = Instantiate(DazeExtensions.PrefabFind(null, "common", "firstroom"));
            MapLoader.Instance.isCustomLoaded = true;
            GameObject stats = Instantiate(DazeExtensions.PrefabFind(null, "common", "statsmanager"));

            StatsManager s = StatsManager.Instance;
            s.levelNumber = -1;

            LevelNamePopup.Instance.SetPrivate("layerString", "U-1");
            LevelNamePopup.Instance.SetPrivate("nameString", "Not a copy of the debug room");

            CanvasController.Instance.gameObject.GetComponentInChildren<SpawnMenu>(true).gameObject.SetActive(true);
            database = SpawnMenu.Instance.GetPrivate<SpawnableObjectsDatabase>("objects");
        }

        public void Start()
        {
            CanvasController.Instance.gameObject.GetComponentInChildren<SpawnMenu>().gameObject.SetActive(false);
        }
    }

    public class Tagger : MonoBehaviour
    {
        [SerializeField]
        private string tagstring;

        public void Awake()
        {
            gameObject.tag = tagstring;
        }
    }

    public class EnemySpawner : MonoBehaviour
    {
        public EnemyType _EnemyType;
        public string _commonOverride = string.Empty;
        private GameObject prefab;

        private void Start()
        {
            if (_commonOverride != string.Empty && _commonOverride.Length > 0)
            {
                prefab = DazeExtensions.PrefabFind(null, "common", _commonOverride);
            }
            else
            {
                prefab = LevelLoader.database.enemies.Where(e => e.enemyType == _EnemyType).First().gameObject;
            }
        }

        public void Spawn()
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }

    public class Trigger : MonoBehaviour
    {
        public UnityEvent FunctionToCall;

        private void OnTriggerEnter()
        {
            FunctionToCall.Invoke();
        }
    }
}
