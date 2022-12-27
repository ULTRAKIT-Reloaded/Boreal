using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boreal
{
    [HarmonyPatch(typeof(StatsManager))]
    public class StatsPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        public static void AwakePrefix(StatsManager __instance)
        {
            if (MapLoader.Instance.isCustomLoaded)
                __instance.secretObjects = new UnityEngine.GameObject[0];
        }
    }
}
