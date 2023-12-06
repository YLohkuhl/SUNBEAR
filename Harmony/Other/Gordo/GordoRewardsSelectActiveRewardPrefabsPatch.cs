using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Regions;
using Il2CppMonomiPark.SlimeRancher.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(GordoRewards), nameof(GordoRewards.SelectActiveRewardPrefabs))]
    internal class GordoRewardsSelectActiveRewardPrefabsPatch
    {
        public static bool Prefix(GordoRewards __instance, ref Il2CppSystem.Collections.Generic.IEnumerable<GameObject> __result)
        {
            GameObject path = GameObject.Find("zoneStrand_Area2/cellSplitTreeTop/Sector/Slimes/gordoSunBear");

            if (__instance.gameObject != path)
            {
                Il2CppSystem.Collections.Generic.List<GameObject> modifiedRewards = new Il2CppSystem.Collections.Generic.List<GameObject>();
                foreach (var reward in __instance.RewardPrefabs)
                    modifiedRewards.Add(reward);

                modifiedRewards.Remove(Get<IdentifiableType>("SunSapCraft").prefab);
                modifiedRewards.Add(Get<IdentifiableType>("ContainerStrand01").prefab);
                __result = modifiedRewards.Cast<Il2CppSystem.Collections.Generic.IEnumerable<GameObject>>();
                return false;
            }

            return true;
        }
    }
}
