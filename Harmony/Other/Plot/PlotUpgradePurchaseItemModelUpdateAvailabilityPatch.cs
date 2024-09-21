using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI.Plot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Enums.LandPlotUpgrade;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(PlotUpgradePurchaseItemModel), nameof(PlotUpgradePurchaseItemModel.UpdateAvailability))]
    internal static class PlotUpgradePurchaseItemModelUpdateAvailabilityPatch
    {
        public static bool Prefix(PlotUpgradePurchaseItemModel __instance, IPlotInfoProvider plotInfoProvider)
        {
            if (__instance.Upgrade != BEAR_FRIENDLY)
                return true;
            Func<bool> func = () => !plotInfoProvider.HasUpgrade(__instance.Upgrade);
            __instance.IsAvailable = func;
            return false;
        }
    }
}
