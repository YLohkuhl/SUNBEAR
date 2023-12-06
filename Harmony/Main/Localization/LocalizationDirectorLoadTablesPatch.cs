using Il2CppMonomiPark.SlimeRancher.Script.Util;
using Il2CppMonomiPark.SlimeRancher.UI.Localization;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using HarmonyLib;
using System.Collections;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(LocalizationDirector), nameof(LocalizationDirector.LoadTables))]
    internal static class LocalizationDirectorLoadTablesPatch
    {
        public static Dictionary<string, Dictionary<string, string>> addedTranslations = new Dictionary<string, Dictionary<string, string>>();

        public static void Postfix(LocalizationDirector __instance) => MelonCoroutines.Start(LoadTable(__instance));

        private static IEnumerator LoadTable(LocalizationDirector director)
        {
            WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(0.01f);
            yield return waitForSecondsRealtime;
            foreach (Il2CppSystem.Collections.Generic.KeyValuePair<string, StringTable> keyValuePair in director.Tables)
            {
                if (addedTranslations.TryGetValue(keyValuePair.Key, out var dictionary))
                {
                    foreach (KeyValuePair<string, string> keyValuePair2 in dictionary)
                        keyValuePair.Value.AddEntry(keyValuePair2.Key, keyValuePair2.Value);
                }
            }
            yield break;
        }

        public static LocalizedString AddTranslation(string table, string key, string localized)
        {
            Dictionary<string, string> dictionary;
            if (!addedTranslations.TryGetValue(table, out dictionary))
            {
                dictionary = new Dictionary<string, string>(); ;
                addedTranslations.Add(table, dictionary);
            }
            dictionary.TryAdd(key, localized);
            StringTable table2 = LocalizationUtil.GetTable(table);
            StringTableEntry stringTableEntry = table2.AddEntry(key, localized);
            return new LocalizedString(table2.SharedData.TableCollectionName, stringTableEntry.SharedEntry.Id);
        }
    }
}
