using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Data.Slimes.SunBear;

namespace SUNBEAR.Harmony.Other
{
    [HarmonyPatch(typeof(SlimeEat), nameof(SlimeEat.EatAndProduce))]
    internal class SlimeEatEatAndProducePatch
    {
        private static int defaultDamagePerAttack;

        public static bool Prefix(SlimeEat __instance, ref GameObject target)
        {
            SlimeDefinition definition = __instance.SlimeDefinition;
            IdentifiableType targetIdent = target.GetComponent<IdentifiableActor>().identType;

            // SUN BEAR
            if (definition != null && targetIdent != null)
            {
                if (definition == Get<SlimeDefinition>("Tarr"))
                {
                    if (defaultDamagePerAttack != __instance.DamagePerAttack)
                        defaultDamagePerAttack = __instance.DamagePerAttack;
                    if (targetIdent.Cast<SlimeDefinition>() == sunBearSlime)
                    {
                        __instance.DamagePerAttack = sunBearSlime.prefab.GetComponent<SlimeHealth>().MaxHealth;
                        return true;
                    }
                }
            }

            // SUN BEAR LARGOS
            if (definition != null && targetIdent != null)
            {
                if (definition == Get<SlimeDefinition>("Tarr"))
                {
                    SlimeDefinition targetLargoIdent = null;
                    if (LocalInstances.sunBearLargoGroup.IsMember(targetIdent.Cast<SlimeDefinition>()))
                        targetLargoIdent = targetIdent.Cast<SlimeDefinition>();

                    if (targetLargoIdent != null && !(target.GetComponent<SlimeHealth>()._currHealth <= __instance.DamagePerAttack))
                    {
                        AttemptSpinAndBite(__instance.gameObject, target);
                        CompleteBite(__instance.gameObject, target, __instance.DamagePerAttack);
                        return false;
                    }
                }
            }

            return true;
        }

        public static void Postfix(SlimeEat __instance, ref GameObject target)
        {
            SlimeDefinition definition = __instance.SlimeDefinition;
            IdentifiableType targetIdent = target.GetComponent<IdentifiableActor>().identType;

            if (definition != null)
            {
                if (definition == Get<SlimeDefinition>("Tarr"))
                {
                    if (targetIdent.Cast<SlimeDefinition>() == sunBearSlime)
                    {
                        if (__instance.DamagePerAttack != defaultDamagePerAttack)
                            __instance.DamagePerAttack = defaultDamagePerAttack;
                    }
                }
            }
        }

        private static bool AttemptSpinAndBite(GameObject original, GameObject target)
        {
            if (original == null)
                return false;

            if (target == null)
                return false;

            var chomper = original.GetComponent<Chomper>();
            var targetIdent = target.GetComponent<IdentifiableActor>().identType;

            if (chomper.CanChomp())
            {
                original.transform.LookAt(target.transform);
                chomper.StartChomp(target, targetIdent, false, false, null, null);
                return true;
            }
            return false;
        }

        private static void CompleteBite(GameObject original, GameObject target, int damageAmount)
        {
            if (original == null)
                return;

            if (target == null)
                return;

            var slimeAudio = original.GetComponent<SlimeAudio>();
            var targetIdent = target.GetComponent<IdentifiableActor>().identType;

            slimeAudio.Play(slimeAudio.SlimeSounds.AttackClipDefinition);
            target.GetComponent<SlimeAudio>().Play(slimeAudio.SlimeSounds.VoiceAlarmClipDefinition);

            if (targetIdent != sunBearSlime)
            {
                Vector3 vector = target.transform.position - original.transform.position;
                float calculation = Mathf.Min(1, Mathf.Sqrt(vector.sqrMagnitude) / 30);
                target.GetComponent<Rigidbody>().AddForce((vector.normalized * calculation + Vector3.up).normalized * 12 * target.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                target.GetComponent<SlimeAudio>().Play(slimeAudio.SlimeSounds.JumpClipDefinition);
                target.GetComponent<SlimeEmotions>().Adjust(SlimeEmotions.Emotion.FEAR, 0.2f);
            }

            target.GetComponent<SlimeHealth>().Damage(new Damage() { Amount = damageAmount, SourceObject = original, DamageSource = Get<DamageSourceDefinition>("SlimeAttack") });
            // MelonLogger.Msg("Completed bite");
        }
    }
}
