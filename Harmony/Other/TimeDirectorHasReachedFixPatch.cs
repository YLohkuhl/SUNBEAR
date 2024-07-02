using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(TimeDirector), nameof(TimeDirector.HasReached))]
    internal class TimeDirectorHasReachedFixPatch
    {
        public static bool Prefix(TimeDirector __instance, double targetWorldTime, ref bool __result)
        {
            __result = __instance._worldModel?.worldTime >= targetWorldTime;
            return false;
        }
    }
}
