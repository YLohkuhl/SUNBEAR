using HarmonyLib;
using SUNBEAR.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(FeralizeOnLargoTransformed), nameof(FeralizeOnLargoTransformed.OnTransformed))]
    internal class FeralizeOnLargoTransformedOnTransformedPatch
    {
        public static bool Prefix(FeralizeOnLargoTransformed __instance)
        {
            IdentifiableType ident = __instance.GetComponent<IdentifiableActor>().identType;
            GameObject obj = __instance.gameObject;

            if (!LocalInstances.sunBearLargoGroup.IsMember(ident))
                return true;

            if (ident != null && obj != null)
            {
                if (SunBearLargos.feralSunBearLargoDefinitions.FirstOrDefault(x => x == ident.Cast<SlimeDefinition>()))
                {
                    obj.GetComponent<SlimeEmotions>().Adjust(SlimeEmotions.Emotion.AGITATION, 1);
                    obj.GetComponent<SlimeEmotions>().Adjust(SlimeEmotions.Emotion.HUNGER, 1);
                    // MelonLogger.Msg("Feralized On Largo Transformed -> Savage On Largo Transformed");
                    return false;
                }
            }

            return true;
        }
    }
}
