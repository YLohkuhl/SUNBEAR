using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(LoadingScreenView), nameof(LoadingScreenView.Awake))]
    internal class LoadingScreenViewAwakePatch
    {
        public static void Prefix(LoadingScreenView __instance)
        {
            if (__instance._animationFrames.ToArray().FirstOrDefault(x => x.Frame1.name.Contains("tut_death")).IsNotNull())
                return;

            __instance._animationFrames.TryAdd(new LoadingScreenView.AnimationEntry()
            {
                Frame1 = LocalAssets.loadingCharsSBASpr,
                Frame2 = LocalAssets.loadingCharsSBBSpr
            });
        }
    }
}
