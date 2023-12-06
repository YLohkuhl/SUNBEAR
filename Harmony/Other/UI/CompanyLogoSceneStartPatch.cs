using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(CompanyLogoScene), nameof(CompanyLogoScene.Start))]
    internal class CompanyLogoSceneStartPatch
    {
        public static void Prefix(CompanyLogoScene __instance) => 
            __instance.bouncyIcons = __instance.bouncyIcons.ToArray().TryAdd(LocalAssets.iconSlimeSunBearSpr);
    }
}
