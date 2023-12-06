using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using SUNBEAR.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Data.Slimes.SunBear;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(SnareModel), nameof(SnareModel.GetGordoIdForBait))]
    internal class SnareModelGetGordoIdForBaitPatch
    {
        public static bool Prefix(SnareModel __instance, ref IdentifiableType __result)
        {
            IdentifiableType baitId = __instance.baitTypeId;
            IdentifiableType gadgetId = __instance.ident;
            float rand = UnityEngine.Random.Range(0f, 1f);

            if (baitId == Get<IdentifiableType>("WildHoneyCraft"))
            {
                if (gadgetId == Get<IdentifiableType>("GordoSnareNovice"))
                {
                    if (rand <= 0.1f)
                    {
                        __result = sunBearGordo;
                        // MelonLogger.Msg(rand);
                        return false;
                    }
                }

                if (gadgetId == Get<IdentifiableType>("GordoSnareAdvanced"))
                {
                    if (rand <= 0.3f)
                    {
                        __result = sunBearGordo;
                        // MelonLogger.Msg(rand);
                        return false;
                    }
                }

                // If this existed, add
                /*
                if (gadgetId == Get<IdentifiableType>("GordoSnareMaster"))
                {
                    if (rand <= 0.5f)
                    {
                        __result = sunBearGordo;
                        // MelonLogger.Msg(rand);
                        return false;
                    }
                }*/
            }

            return true;
        }

        /*public static void Postfix(ref IdentifiableType __result)
        {
            if (__result)
                MelonLogger.Msg(__result.ToString());
        }*/
    }
}
