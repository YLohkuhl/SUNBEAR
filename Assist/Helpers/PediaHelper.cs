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
        public static string CreatePediaKey(string prefix, string localizationSuffix)
        { return "m." + prefix + "." + localizationSuffix; }

        public static string CreateIdentifiableKey(string prefix, IdentifiableType identifiableType)
        { return "m." + prefix + "." + identifiableType._pediaPersistenceSuffix; }

        /*public static PediaPage CreatePediaSection(string pediaSectionName, string sectionTitle, Sprite sectionIcon)
        {
            PediaPage pediaPage = ScriptableObject.CreateInstance<PediaPage>();
            pediaPage.name = pediaSectionName;
            pediaPage._title = LocalizationDirectorLoadTablePatch.AddTranslation("UI", "l." + pediaSectionName.ToLower().Replace(" ", "_"), sectionTitle);
            pediaPage._icon = sectionIcon;
            return pediaPage;
        }*/

        public static IdentifiablePediaEntry CreateIdentifiableEntry(IdentifiableType identifiableType, string pediaEntryName, PediaHighlightSet pediaHighlightSet,
            LocalizedString pediaTitle, LocalizedString pediaIntro, PediaEntryDetail[] pediaEntryDetails, bool unlockedInitially = false)
        {
            if (Get<IdentifiablePediaEntry>(pediaEntryName))
                return null;

            IdentifiablePediaEntry identifiablePediaEntry = ScriptableObject.CreateInstance<IdentifiablePediaEntry>();

            identifiablePediaEntry.hideFlags |= HideFlags.HideAndDontSave;
            identifiablePediaEntry.name = pediaEntryName;
            identifiablePediaEntry._title = pediaTitle;
            identifiablePediaEntry._description = pediaIntro;
            identifiablePediaEntry._identifiableType = identifiableType;

            identifiablePediaEntry._details = pediaEntryDetails;
            identifiablePediaEntry._highlightSet = pediaHighlightSet;
            identifiablePediaEntry._unlockInfoProvider = SceneContext.Instance.PediaDirector.Cast<IUnlockInfoProvider>();
            identifiablePediaEntry._isUnlockedInitially = unlockedInitially;

            return identifiablePediaEntry;
        }

        // Don't need this anymore
        /*public static void AddPediaSection(PediaEntry pediaEntry, PediaDetailSection pediaSection, string pediaText)
        {
            if (pediaEntry.IsNull())
                return;

            string localizationSuffix;

            if (pediaEntry.TryCast<FixedPediaEntry>())
                localizationSuffix = pediaEntry.Cast<FixedPediaEntry>()._persistenceSuffix;
            else if (pediaEntry.TryCast<IdentifiablePediaEntry>())
                localizationSuffix = pediaEntry.Cast<IdentifiablePediaEntry>().IdentifiableType._pediaPersistenceSuffix;
            else
                return;

            List<PediaEntryDetail> pediaPagesEntries = pediaEntry._details?.ToList();

            if (pediaPagesEntries.IsNull())
                pediaPagesEntries = new List<PediaEntryDetail>();

            LocalizedString pediaTranslation = GeneralizedHelper.CreateTranslation("PediaPage", CreatePediaKey(pediaSection.name.ToLower().Replace(" ", "_"), localizationSuffix), pediaText);
            pediaPagesEntries.Add(new PediaEntryDetail()
            {
                SE = pediaSection,
                Text = pediaTranslation,
                TextGamepad = pediaTranslation,
                TextPS4 = pediaTranslation
            });

            pediaEntry._pageEntries = pediaPagesEntries.ToArray();
        }*/

        public static PediaEntry AddSlimepedia(IdentifiableType identifiableType, string pediaEntryName, string pediaIntro, bool unlockedInitially = false)
        {
            if (Get<IdentifiablePediaEntry>(pediaEntryName))
                return null;

            PediaCategory basePediaEntryCategory = SRSingleton<SceneContext>.Instance.PediaDirector._pediaConfiguration.Categories.ToArray().First(x => x.name == "Slimes");
            PediaEntry pediaEntry = basePediaEntryCategory._items.First();

            LocalizedString intro = GeneralizedHelper.CreateTranslation("Pedia", CreateIdentifiableKey("intro", identifiableType), pediaIntro);
            IdentifiablePediaEntry identifiablePediaEntry = CreateIdentifiableEntry(identifiableType, pediaEntryName, pediaEntry._highlightSet,
                identifiableType.localizedName, intro, null, unlockedInitially);

            if (!basePediaEntryCategory._items.FirstOrDefault(x => x == identifiablePediaEntry))
                basePediaEntryCategory._items = basePediaEntryCategory._items.ToArray().AddToArray(identifiablePediaEntry);
            GeneralizedHelper.RegisterPediaEntry(identifiablePediaEntry);

            return identifiablePediaEntry;
        }

        public static void AddSlimepediaSection(IdentifiablePediaEntry identifiablePediaEntry, string pediaText, bool isRisks = false, bool isPlortonomics = false)
        {
            if (identifiablePediaEntry.IsNull())
                return;

            List<PediaEntryDetail> pediaPagesEntries = identifiablePediaEntry._details?.ToList();

            if (pediaPagesEntries.IsNull())
                pediaPagesEntries = new List<PediaEntryDetail>();

            LocalizedString pediaTranslation;
            if (isRisks && !isPlortonomics)
            {
                pediaTranslation = GeneralizedHelper.CreateTranslation("PediaPage", CreateIdentifiableKey("risks", identifiablePediaEntry.IdentifiableType), pediaText);
                pediaPagesEntries.Add(new PediaEntryDetail()
                {
                    Section = Get<PediaDetailSection>("Rancher Risks"),
                    Text = pediaTranslation,
                    TextGamepad = pediaTranslation,
                    TextPS4 = pediaTranslation
                });
            }
            else if (!isRisks && isPlortonomics)
            {
                pediaTranslation = GeneralizedHelper.CreateTranslation("PediaPage", CreateIdentifiableKey("plortonomics", identifiablePediaEntry.IdentifiableType), pediaText);
                pediaPagesEntries.Add(new PediaEntryDetail()
                {
                    Section = Get<PediaDetailSection>("Plortonomics"),
                    Text = pediaTranslation,
                    TextGamepad = pediaTranslation,
                    TextPS4 = pediaTranslation
                });
            }
            else
            {
                pediaTranslation = GeneralizedHelper.CreateTranslation("PediaPage", CreateIdentifiableKey("slimeology", identifiablePediaEntry.IdentifiableType), pediaText);
                pediaPagesEntries.Add(new PediaEntryDetail()
                {
                    Section = Get<PediaDetailSection>("Slimeology"),
                    Text = pediaTranslation,
                    TextGamepad = pediaTranslation,
                    TextPS4 = pediaTranslation
                });
            }

            identifiablePediaEntry._details = pediaPagesEntries.ToArray();
        }
    }
}
