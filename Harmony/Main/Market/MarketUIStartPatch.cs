using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Assist.GeneralizedHelper;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(MarketUI), nameof(MarketUI.Start))]
    internal static class MarketUIStartPatch
    {
        public static void Prefix(MarketUI __instance)
        {
            __instance.plorts = (from x in __instance.plorts
                                 where !plortsToPatch.Exists((MarketUI.PlortEntry y) => y == x)
                                 select x).ToArray();
            __instance.plorts = __instance.plorts.ToArray().AddRangeToArray(plortsToPatch.ToArray());
        }
    }
}
