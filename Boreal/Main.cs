using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULTRAKIT.Extensions;
using UMM;

namespace Boreal
{
    [UKPlugin("petersone1.boreal", "Boreal", "1.0.0", "A custom level loader for ULTRAKILL", true, false)]
    public class Plugin : UKMod
    {
        public override void OnModLoaded()
        {
            base.OnModLoaded();
            Harmony harmony = new Harmony("Boreal");
            harmony.PatchAll();
        }
    }
}
