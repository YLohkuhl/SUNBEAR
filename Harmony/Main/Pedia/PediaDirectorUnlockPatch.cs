using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Data.Slimes.SunBear;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(PediaDirector), nameof(PediaDirector.Unlock), [ typeof(IdentifiableType) ])]
    internal static class PediaDirectorUnlockPatch
    {
        public static bool Prefix(PediaDirector __instance, ref IdentifiableType identifiableType)
        {
            if (identifiableType == cubSunBearSlime)
            {
                __instance.Unlock(sunBearSlime);
                return false;
            }
            return true;
        }
    }
}
