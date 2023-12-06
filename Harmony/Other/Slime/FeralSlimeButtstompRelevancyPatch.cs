using HarmonyLib;
using SUNBEAR.Components;
using SUNBEAR.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(FeralSlimeButtstomp), nameof(FeralSlimeButtstomp.Relevancy))]
    internal class FeralSlimeButtstompRelevanyPatch
    {
        public static bool Prefix(FeralSlimeButtstomp __instance, ref float __result, ref bool isGrounded)
        {
            IdentifiableType ident = __instance._slimeAudio._slimeModel.ident;
            GameObject obj = __instance.gameObject;

            if (!LocalInstances.sunBearLargoGroup.IsMember(ident))
                return true;

            if (ident != null && obj != null)
            {
                if (SunBearLargos.feralSunBearLargoDefinitions.FirstOrDefault(x => x == ident.Cast<SlimeDefinition>()))
                {
                    var isSavage = obj.GetComponent<SunBearSavage>().IsSavage();
                    if (isGrounded && isSavage && Time.time >= __instance._nextStompTime)
                    {
                        float sqrMagnitude = (SceneContext.Instance.Player.transform.position + Vector3.up - obj.transform.position).sqrMagnitude;
                        if (sqrMagnitude <= 400f && sqrMagnitude >= 25f)
                        {
                            __result = Randoms.SHARED.GetInRange(0.3f, 1f);
                            // MelonLogger.Msg("Feral Stomping");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
