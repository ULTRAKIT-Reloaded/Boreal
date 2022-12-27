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
            if (__instance.levelNumber < 0)
                __instance.secretObjects = new UnityEngine.GameObject[0];
        }
    }
}
