using HarmonyLib;
using SUNBEAR.Data.Slimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(GordoRewardsBase), nameof(GordoRewardsBase.GiveRewards))]
    internal class GordoRewardsBaseGiveRewardsPatch
    {
        public static Vector3[] sunBearSpawns = new Vector3[16];
        public static Vector3[] defaultGordoSpawns = GordoRewardsBase._spawns;

        public static void Prefix(GordoRewardsBase __instance)
        {
            if (__instance?.GetComponent<GordoIdentifiable>()?.identType != SunBear.sunBearGordo)
                return;

            sunBearSpawns[0] = Vector3.zero;
            for (int i = 0; i < 6; i++)
            {
                float f = 6.2831855f * (float)i / 6f;
                sunBearSpawns[i + 1] = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
            }
            for (int j = 0; j < 3; j++)
            {
                float f2 = 6.2831855f * (float)j / 3f + 0.5235988f;
                sunBearSpawns[j + 7] = new Vector3(Mathf.Cos(f2) * 0.5f, 0.866f, Mathf.Sin(f2) * 0.5f);
            }
            for (int k = 0; k < 3; k++)
            {
                float f3 = 6.2831855f * (float)k / 3f - 0.5235988f;
                sunBearSpawns[k + 10] = new Vector3(Mathf.Cos(f3) * 0.5f, -0.866f, Mathf.Sin(f3) * 0.5f);
            }
            for (int n = 0; n < 3; n++)
            {
                float f3 = 6.2831855f * (float)n / 3f - 0.5235988f;
                sunBearSpawns[n + 13] = new Vector3(Mathf.Cos(f3) * 0.5f, -0.866f, Mathf.Sin(f3) * 0.5f);
            }

            GordoRewardsBase._spawns = sunBearSpawns;
        }

        public static void Postfix(GordoRewardsBase __instance)
        {
            if (__instance?.GetComponent<GordoIdentifiable>()?.identType != SunBear.sunBearGordo)
                return;
            GordoRewardsBase._spawns = defaultGordoSpawns;
        }
    }
}
