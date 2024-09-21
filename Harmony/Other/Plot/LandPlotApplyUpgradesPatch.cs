using HarmonyLib;
using SUNBEAR.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(LandPlot), nameof(LandPlot.ApplyUpgrades))]
    internal static class LandPlotApplyUpgradesPatch
    {
        public static void Postfix(LandPlot __instance, Il2CppSystem.Collections.Generic.IEnumerable<LandPlot.Upgrade> upgrades)
        {
            Il2CppSystem.Collections.Generic.List<LandPlot.Upgrade> plotUpgrades = new(upgrades);
            BearFriendlyUpgrader plotUpgrader = __instance.GetComponent<BearFriendlyUpgrader>();

            if (plotUpgrader)
                foreach (var upgrade in plotUpgrades)
                    plotUpgrader.Apply(upgrade);
        }
    }
}
