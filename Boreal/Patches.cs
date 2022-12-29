using BorealEditor;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Boreal
{
    [HarmonyPatch(typeof(StatsManager))]
    public class StatsManagerPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        public static void AwakePrefix(StatsManager __instance)
        {
            if (MapLoader.Instance.isCustomLoaded)
                __instance.secretObjects = new UnityEngine.GameObject[0];
        }
    }

    [HarmonyPatch(typeof(LevelStats))]
    public class LevelStatsPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void StartPostfix(LevelStats __instance)
        {
            if ((MapLoader.Instance?.isCustomLoaded ?? false) && LevelLoader.Instance != null)
                __instance.transform.Find("Title").GetComponent<Text>().text = $"{LevelLoader.Instance.LayerName}: {LevelLoader.Instance.LevelName}";
        }
    }
}
