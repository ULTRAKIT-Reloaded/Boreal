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
using BorealEditor.Components;

namespace BorealEditor.Initializers
{
    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class BorealManager : MonoSingleton<BorealManager>
    {
        public string LayerName;
        public string LevelName;

        public int[] TimeRanks = new int[4];
        public int[] KillRanks = new int[4];
        public int[] StyleRanks = new int[4];

        public GameObject[] Secrets = new GameObject[0];
        public string ChallengeDescription;

        [HideInInspector]
        public SpawnableObjectsDatabase database;
       
        public void OnValidate()
        {
            if (TimeRanks.Length != 4)
                Array.Resize(ref TimeRanks, 4);
            if (KillRanks.Length != 4)
                Array.Resize(ref KillRanks, 4);
            if (StyleRanks.Length != 4)
                Array.Resize(ref StyleRanks, 4);
            if (Secrets.Length > 5)
                Array.Resize(ref Secrets, 5);
        }
    }

    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class LevelLoader : MonoSingleton<LevelLoader>
    {
        [HideInInspector]
        public GameObject FirstRoom;

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("Loading custom level");
            FirstRoom = Instantiate(DazeExtensions.PrefabFind(null, "common", "firstroom"), transform.position, Quaternion.identity, transform);
            MapLoader.Instance.isCustomLoaded = true;
            MapLoader.Instance.currentCustomId = "com.boreal." + SceneManager.GetActiveScene().name;
            Instantiate(DazeExtensions.PrefabFind(null, "common", "statsmanager"));

            StatsManager.Instance.levelNumber = -1;
            StatsManager.Instance.timeRanks = BorealManager.Instance.TimeRanks;
            StatsManager.Instance.killRanks = BorealManager.Instance.KillRanks;
            StatsManager.Instance.styleRanks = BorealManager.Instance.StyleRanks;

            LevelNamePopup.Instance.SetPrivate("layerString", BorealManager.Instance.LayerName);
            LevelNamePopup.Instance.SetPrivate("nameString", BorealManager.Instance.LevelName);

            CanvasController.Instance.GetComponentInChildren<SpawnMenu>(true).gameObject.SetActive(true);
            BorealManager.Instance.database = SpawnMenu.Instance.GetPrivate<SpawnableObjectsDatabase>("objects");
        }

        public void Start()
        {
            CanvasController.Instance.GetComponentInChildren<SpawnMenu>().gameObject.SetActive(false);
        }
    }

    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class EndZone : MonoSingleton<EndZone>
    {
        [HideInInspector]
        public GameObject FinalRoom;
        [HideInInspector]
        public FinalDoor FinalDoor;

        private void Start()
        {
            Debug.Log("Loading final pit");
            FinalRoom = Instantiate(DazeExtensions.PrefabFind(null, "common", "FinalRoom 1"), transform.position - new Vector3(0f, 10f, 0f), Quaternion.identity, transform);
            FinalDoor = FinalRoom.GetComponentInChildren<FinalDoor>();
            CameraController.Instance.GetComponentInChildren<LevelNameFinder>(true).textBeforeName = $"{BorealManager.Instance.LayerName}: {BorealManager.Instance.LevelName}";
            FinalRank.Instance.transform.Find("Challenge/Text").GetComponent<Text>().text = BorealManager.Instance.ChallengeDescription.ToUpper();
        }

        public void OpenDoor()
        {
            StatsManager.Instance.StopTimer();
            FinalDoor.Open();
        }

        public void CloseDoor()
        {
            FinalDoor.Close();
        }
    }
}
