using BorealEditor;
using BorealEditor.Boilerplate;
using BorealEditor.Challenges;
using BorealEditor.Initializers;
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
            if (MapLoader.Instance.isCustomLoaded && LevelLoader.Instance != null)
                __instance.secretObjects = BorealManager.Instance.Secrets;
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
                __instance.transform.Find("Title").GetComponent<Text>().text = $"{BorealManager.Instance.LayerName}: {BorealManager.Instance.LevelName}";
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier))]
    public class EnemyIdentiferPatch
    {
        [HarmonyPatch("DeliverDamage")]
        [HarmonyPostfix]
        public static void DamagePostfix(EnemyIdentifier __instance)
        {
            if (UseOnlyChallenge.Instance)
            {
                if (UseOnlyChallenge.Instance.hitterType == HitterType.Hitter)
                {
                    UseOnlyChallenge.Instance.CheckKill(__instance.hitter);
                }
                if (UseOnlyChallenge.Instance.hitterType == HitterType.HitterWeapon)
                {
                    foreach (string weapon in __instance.hitterWeapons)
                    {
                        UseOnlyChallenge.Instance.CheckKill(weapon);
                    }
                }
            }
        }
    }
}
