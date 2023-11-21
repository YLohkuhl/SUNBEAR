using Il2CppMonomiPark.SlimeRancher.Script.Util;
using Il2CppMonomiPark.SlimeRancher.UI.Localization;
using MelonLoader;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using HarmonyLib;
using SUNBEAR.Data.Slimes;
using System.Collections;
using Il2CppMonomiPark.SlimeRancher.UI;
// using SUNBEAR.Data.Foods;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using static SUNBEAR.Data.Slimes.SunBear;
using SUNBEAR.Data;
using SUNBEAR.Components;
using Il2CppMonomiPark.SlimeRancher.Damage;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.Pedia;

namespace SUNBEAR
{
    internal class HarmonyPatches
    {
        internal static HashSet<PediaEntry> pediasToPatch = new HashSet<PediaEntry>();
        internal static List<MarketUI.PlortEntry> plortsToPatch = new List<MarketUI.PlortEntry>();
        internal static List<EconomyDirector.ValueMap> valueMapsToPatch = new List<EconomyDirector.ValueMap>();

        internal class Save
        {
            [HarmonyPatch(typeof(AutoSaveDirector), "Awake")]
            internal static class PatchAutoSaveDirectorAwake
            {
                public static void Prefix(AutoSaveDirector __instance)
                {
                    LocalInstances.ASDInitialize();
                    SunBearLargos.ASDInitialize();

                    Get<IdentifiableTypeGroup>("BaseSlimeGroup").memberTypes.Add(sunBearSlime);
                    Get<IdentifiableTypeGroup>("EdibleSlimeGroup").memberTypes.Add(sunBearSlime);
                    Get<IdentifiableTypeGroup>("SlimesSinkInShallowWaterGroup").memberTypes.Add(sunBearSlime);
                    Get<IdentifiableTypeGroup>("VaccableBaseSlimeGroup").memberTypes.Add(sunBearSlime);
                    // Get<IdentifiableTypeGroup>("SlimesGroup").memberTypes.Add(sunBearSlime);

                    Get<IdentifiableTypeGroup>("EdiblePlortFoodGroup").memberTypes.Add(sunBearPlort);
                    Get<IdentifiableTypeGroup>("PlortGroup").memberTypes.Add(sunBearPlort);

                    Get<IdentifiableTypeGroup>("BaseSlimeGroup").memberTypes.Add(cubSunBearSlime);
                    Get<IdentifiableTypeGroup>("EdibleSlimeGroup").memberTypes.Add(cubSunBearSlime);
                    Get<IdentifiableTypeGroup>("SlimesSinkInShallowWaterGroup").memberTypes.Add(cubSunBearSlime);
                    // Get<IdentifiableTypeGroup>("SlimesGroup").memberTypes.Add(cubSunBearSlime);

                    Get<IdentifiableTypeGroup>("GordoGroup").memberTypes.Add(sunBearGordo);

                    foreach (SlimeDefinition largoDefinition in SunBearLargos.sunBearLargoDefinitions)
                    {
                        Get<IdentifiableTypeGroup>(largoDefinition.name.Replace("SunBear", "") + "LargoGroup").memberTypes.Add(largoDefinition);
                        LocalInstances.sunBearLargoGroup.memberTypes.Add(largoDefinition);
                    }

                    Get<IdentifiableTypeGroup>("SlimesSinkInShallowWaterGroup").memberGroups.Add(LocalInstances.sunBearLargoGroup);
                    Get<IdentifiableTypeGroup>("EdibleSlimeGroup").memberGroups.Add(LocalInstances.sunBearLargoGroup);
                    Get<IdentifiableTypeGroup>("LargoGroup").memberGroups.Add(LocalInstances.sunBearLargoGroup);

                    Get<IdentifiableTypeGroup>("FoodGroup").memberTypes.Add(Get<IdentifiableType>("WildHoneyCraft"));
                    // Get<IdentifiableTypeGroup>("FoodGroup").memberTypes.Add(Sunberry.sunberryFruit);

                    __instance.identifiableTypes.memberTypes.Add(sunBearSlime);
                    __instance.identifiableTypes.memberTypes.Add(cubSunBearSlime);
                    __instance.identifiableTypes.memberTypes.Add(sunBearPlort);

                    foreach (SlimeDefinition largoDefinition in SunBearLargos.sunBearLargoDefinitions)
                        __instance.identifiableTypes.memberTypes.Add(largoDefinition);

                    // __instance.identifiableTypes.memberTypes.Add(Sunberry.sunberryFruit);
                }
            }
        }

        internal class Market
        {
            [HarmonyPatch(typeof(MarketUI), "Start")]
            internal static class MarketUIStartPatch
            {
                public static void Prefix(MarketUI __instance)
                {
                    __instance.plorts = (from x in __instance.plorts
                                         where !plortsToPatch.Exists((MarketUI.PlortEntry y) => y == x)
                                         select x).ToArray();
                    __instance.plorts = __instance.plorts.ToArray().AddRangeToArray(plortsToPatch.ToArray());
                }
            }

            [HarmonyPatch(typeof(EconomyDirector), "InitModel")]
            internal static class EconomyDirectorInitModelPatch
            {
                public static void Prefix(EconomyDirector __instance)
                {
                    __instance.BaseValueMap = __instance.BaseValueMap.ToArray().AddRangeToArray(valueMapsToPatch.ToArray());
                }
            }
        }

        internal class Pedia
        {
            [HarmonyPatch(typeof(SavedGame), "Push", typeof(GameModel))]
            internal static class SavedGamePushPatch
            {
                public static void Prefix(SavedGame __instance)
                {
                    foreach (var pediaEntry in pediasToPatch)
                        __instance.pediaEntryLookup.TryAdd(pediaEntry.GetPersistenceId(), pediaEntry);
                }
            }

            [HarmonyPatch(typeof(PediaDirector), "Awake")]
            internal static class PediaDirectorAwakePatch
            {
                public static void Prefix(PediaDirector __instance)
                {
                    Grown.LoadSlimepedia();
                    foreach (var pediaEntry in pediasToPatch)
                    {
                        var identPediaEntry = pediaEntry.TryCast<IdentifiablePediaEntry>();
                        if (pediaEntry.TryCast<IdentifiablePediaEntry>())
                            __instance._identDict.TryAdd(identPediaEntry.IdentifiableType, pediaEntry);
                    }
                }
            }
        }

        internal class Localization
        {
            [HarmonyPatch(typeof(LocalizationDirector), "LoadTables")]
            internal static class LocalizationDirectorLoadTablePatch
            {
                public static void Postfix(LocalizationDirector __instance)
                {
                    MelonCoroutines.Start(LoadTable(__instance));
                }

                private static IEnumerator LoadTable(LocalizationDirector director)
                {
                    WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(0.01f);
                    yield return waitForSecondsRealtime;
                    foreach (Il2CppSystem.Collections.Generic.KeyValuePair<string, StringTable> keyValuePair in director.Tables)
                    {
                        if (addedTranslations.TryGetValue(keyValuePair.Key, out var dictionary))
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> keyValuePair2 in dictionary)
                            {
                                keyValuePair.Value.AddEntry(keyValuePair2.Key, keyValuePair2.Value);
                            }
                        }
                    }
                    yield break;
                }

                public static LocalizedString AddTranslation(string table, string key, string localized)
                {
                    System.Collections.Generic.Dictionary<string, string> dictionary;
                    if (!addedTranslations.TryGetValue(table, out dictionary))
                    {
                        dictionary = new System.Collections.Generic.Dictionary<string, string>(); ;
                        addedTranslations.Add(table, dictionary);
                    }
                    dictionary.TryAdd(key, localized);
                    StringTable table2 = LocalizationUtil.GetTable(table);
                    StringTableEntry stringTableEntry = table2.AddEntry(key, localized);
                    return new LocalizedString(table2.SharedData.TableCollectionName, stringTableEntry.SharedEntry.Id);
                }

                public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> addedTranslations = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>();
            }
        }
    }

    internal class OtherHarmonyPatches
    {
        internal class Gordo
        {
            /*[HarmonyPatch(typeof(SnareModel), "SnareGordo", new Type[] { })]
            internal class SnareModelSnareGordoPatch
            {
                public static bool Prefix(SnareModel __instance)
                {
                    if (!__instance.IsBaited() || __instance.HasSnaredGordo())
                        return false;

                    IdentifiableType gordoIdForBait = __instance.GetGordoIdForBait();
                    if (gordoIdForBait == null)
                        return false;

                    __instance.SnareGordo(gordoIdForBait);
                    return true;
                }
            }*/

            [HarmonyPatch(typeof(SnareModel), "GetGordoIdForBait")]
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

            [HarmonyPatch(typeof(GordoRewardsBase), "GiveRewards")]
            internal class GordoRewardsBaseGiveRewardsPatch
            {
                public static Vector3[] sunBearSpawns = new Vector3[16];
                public static Vector3[] defaultGordoSpawns = GordoRewardsBase._spawns;

                public static void Prefix(GordoRewardsBase __instance)
                {
                    if (__instance.GetComponent<GordoIdentifiable>().identType != SunBear.sunBearGordo)
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
                    if (__instance.GetComponent<GordoIdentifiable>().identType != SunBear.sunBearGordo)
                        return;

                    GordoRewardsBase._spawns = defaultGordoSpawns;
                }
            }
        }

        internal class Slime
        {
            [HarmonyPatch(typeof(FleeThreats), "RegistryFixedUpdate")]
            internal class FleeThreatsRegistryFixedUpdatePatch
            {
                public static bool Prefix(FleeThreats __instance)
                {
                    if (__instance.FearProfile != null && __instance._threat != null)
                    {
                        if (__instance.FearProfile._threatsLookup != null && __instance._threat.IdentType != null)
                        {
                            if (!__instance.FearProfile._threatsLookup.ContainsKey(__instance._threat.IdentType))
                                return false;
                        }
                    }
                    return true;
                }
            }

            [HarmonyPatch(typeof(FeralizeOnLargoTransformed), "OnTransformed")]
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

            [HarmonyPatch(typeof(FeralSlimeButtstomp), "Relevancy")]
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

            [HarmonyPatch(typeof(SlimeEat), "EatAndProduce")]
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
    }
}