using Il2CppMonomiPark.SlimeRancher.UI.Pedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Localization;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using HarmonyLib;

namespace SUNBEAR.Assist
{
    internal class PediaHelper
    {
        internal static HashSet<PediaEntry> pediasToPatch = new HashSet<PediaEntry>();

        public static string CreatePediaKey(string prefix, string suffix)
        { return "m." + prefix + "." + suffix; }

        public static string CreateIdentifiableKey(IdentifiableType identifiableType, string prefix)
        { return "m." + prefix + "." + identifiableType._pediaPersistenceSuffix; }

        public static void RegisterPediaEntry(PediaEntry pediaEntry)
        {
            if (!pediasToPatch.Contains(pediaEntry))
                pediasToPatch.Add(pediaEntry);
        }

        public static IdentifiablePediaEntry CreateIdentifiableEntry(IdentifiableType identifiableType, PediaHighlightSet highlightSet,
            LocalizedString intro, PediaEntryDetail[] entryDetails, bool isUnlockedInitially = false)
        {
            if (Get<IdentifiablePediaEntry>(identifiableType?.name))
                return null;

            IdentifiablePediaEntry identifiablePediaEntry = ScriptableObject.CreateInstance<IdentifiablePediaEntry>();
            identifiablePediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            identifiablePediaEntry.name = identifiableType.name;

            identifiablePediaEntry._title = identifiableType.localizedName;
            identifiablePediaEntry._description = intro;
            identifiablePediaEntry._identifiableType = identifiableType;

            identifiablePediaEntry._details = entryDetails;
            identifiablePediaEntry._highlightSet = highlightSet;
            // identifiablePediaEntry._unlockInfoProvider = SceneContext.Instance.PediaDirector.Cast<IUnlockInfoProvider>();
            identifiablePediaEntry._isUnlockedInitially = isUnlockedInitially;

            RegisterPediaEntry(identifiablePediaEntry);
            return identifiablePediaEntry;
        }

        public static void AddPediaToCategory(PediaEntry pediaEntry, PediaCategory pediaCategory)
        {
            if (!pediaCategory)
                return;

            LookupDirector director = SRSingleton<GameContext>.Instance.LookupDirector;
            if (!director._categories[director._categories.IndexOf(pediaCategory.GetRuntimeCategory())].Contains(pediaEntry))
                director.AddPediaEntryToCategory(pediaEntry, pediaCategory);
        }

        public static void AddSectionToPedia(PediaEntry pediaEntry, PediaDetailSection pediaDetailSection, LocalizedString textTranslation)
        {
            if (pediaEntry.IsNull())
                return;

            if (pediaDetailSection.IsNull())
                return;

            List<PediaEntryDetail> entryDetails = pediaEntry._details?.ToList();
            if (entryDetails.IsNull())
                entryDetails = [];

            entryDetails.Add(new()
            {
                Section = pediaDetailSection,
                Text = textTranslation,
                TextGamepad = textTranslation,
                TextPS4 = textTranslation
            });

            pediaEntry._details = entryDetails.ToArray();
        }
    }
}
