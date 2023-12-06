using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Assist.GeneralizedHelper;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(EconomyDirector), nameof(EconomyDirector.InitModel))]
    internal static class EconomyDirectorInitModelPatch
    {
        public static void Prefix(EconomyDirector __instance) => 
            __instance.BaseValueMap = __instance.BaseValueMap.ToArray().AddRangeToArray(valueMapsToPatch.ToArray());
    }
}
