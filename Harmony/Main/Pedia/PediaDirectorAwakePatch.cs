using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Data.Slimes.SunBear;
using static SUNBEAR.Assist.GeneralizedHelper;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(PediaDirector), nameof(PediaDirector.Awake))]
    internal static class PediaDirectorAwakePatch
    {
        public static void Prefix(PediaDirector __instance)
        {
            Grown.LoadSlimepedia();
            foreach (var pediaEntry in pediasToPatch)
            {
                var identPediaEntry = pediaEntry.TryCast<IdentifiablePediaEntry>();
                if (identPediaEntry)
                {
                    __instance._identDict.TryAdd(identPediaEntry.IdentifiableType, identPediaEntry);
                    if (SunBearPreferences.IsCasualMode() && SunBearPreferences.IsCasualCubs())
                        if (identPediaEntry.IdentifiableType == sunBearSlime)
                            __instance._identDict.TryAdd(cubSunBearSlime, identPediaEntry);
                }
            }
        }
    }
}
