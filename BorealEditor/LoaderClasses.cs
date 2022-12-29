using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULTRAKIT.Extensions;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BorealEditor
{
    // Initializers

    [ConfigureSingleton(SingletonFlags.DestroyDuplicates)]
    public class LevelLoader : MonoSingleton<LevelLoader>
    {
        public string LayerName;
        public string LevelName;

        public int[] TimeRanks = new int[4];
        public int[] KillRanks = new int[4];
        public int[] StyleRanks = new int[4];

        [HideInInspector]
        public static SpawnableObjectsDatabase database;
        [HideInInspector]
        public GameObject FirstRoom;
        
        public void OnValidate()
        {
            if (TimeRanks.Length != 4)
                Array.Resize(ref TimeRanks, 4);
            if (KillRanks.Length != 4)
                Array.Resize(ref KillRanks, 4);
            if (StyleRanks.Length != 4)
                Array.Resize(ref StyleRanks, 4);
        }

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("Loading custom level");
            FirstRoom = Instantiate(DazeExtensions.PrefabFind(null, "common", "firstroom"), transform.position, Quaternion.identity, transform);
            MapLoader.Instance.isCustomLoaded = true;
            MapLoader.Instance.currentCustomId = "com.boreal." + SceneManager.GetActiveScene().name;
            Instantiate(DazeExtensions.PrefabFind(null, "common", "statsmanager"));

            StatsManager.Instance.levelNumber = -1;
            StatsManager.Instance.timeRanks = TimeRanks;
            StatsManager.Instance.killRanks = KillRanks;
            StatsManager.Instance.styleRanks = StyleRanks;

            LevelNamePopup.Instance.SetPrivate("layerString", LayerName);
            LevelNamePopup.Instance.SetPrivate("nameString", LevelName);

            CanvasController.Instance.GetComponentInChildren<SpawnMenu>(true).gameObject.SetActive(true);
            database = SpawnMenu.Instance.GetPrivate<SpawnableObjectsDatabase>("objects");
        }

        public void Start()
        {
            CanvasController.Instance.GetComponentInChildren<SpawnMenu>().gameObject.SetActive(false);
        }
    }

    [ConfigureSingleton(SingletonFlags.DestroyDuplicates)]
    public class CreateEndZone : MonoSingleton<CreateEndZone>
    {
        [HideInInspector]
        public GameObject FinalRoom;
        [HideInInspector]
        public FinalDoor FinalDoor;

        protected void Start()
        {
            Debug.Log("Loading final pit");
            FinalRoom = Instantiate(DazeExtensions.PrefabFind(null, "common", "FinalRoom 1"), transform.position - new Vector3(0f, 10f, 0f), Quaternion.identity, transform);
            FinalDoor = FinalRoom.GetComponentInChildren<FinalDoor>();
            CameraController.Instance.GetComponentInChildren<LevelNameFinder>(true).textBeforeName = $"{LevelLoader.Instance.LayerName}: {LevelLoader.Instance.LevelName}";
        }

        public void OpenDoor()
        {
            FinalDoor.Open();
        }

        public void CloseDoor()
        {
            FinalDoor.Close();
        }
    }

    // Components

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
            if (_active && other.tag == "Player")
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
}
