using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(FleeThreats), nameof(FleeThreats.RegistryFixedUpdate))]
    internal class FleeThreatsRegistryFixedUpdatePatch
    {
        public static bool Prefix(FleeThreats __instance)
        {
            if (__instance.FearProfile != null && __instance._threat != null)
            {
                if (__instance.FearProfile._threatsLookup != null && __instance._threat.IdentType != null)
                {
                    if (!__instance.FearProfile._threatsLookup.ContainsKey(__instance._threat.IdentType))
                        return false;
                }
            }
            return true;
        }
    }
}
