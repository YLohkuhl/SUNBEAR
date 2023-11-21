using Il2CppMonomiPark.SlimeRancher.Slime;
using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR
{
    internal class SunBearPreferences
    {
        private static readonly string CategoryIdentifier = "SunBearPreferences";
        private static readonly string CasualIdentifier = "Casual Mode";
        private static readonly string CasualWSavageIdentifier = "Casual Mode (With Savage)";
        // private static readonly string MixedIdentifier = "Mixed Mode";
        private static readonly string RealisticIdentifier = "Realistic Mode";
        private static readonly string RealisticWOSavageIdentifier = "Realistic Mode (Without Savage)";

        private static MelonPreferences_Category SunBearPrefs;
        private static MelonPreferences_Entry<bool> casualMode;
        private static MelonPreferences_Entry<bool> casualWSavageMode;
        // private static MelonPreferences_Entry<bool> mixedMode;
        private static MelonPreferences_Entry<bool> realisticMode;
        private static MelonPreferences_Entry<bool> realisticWOSavageMode;

        public static void Initialize()
        {
            SunBearPrefs = MelonPreferences.CreateCategory(CategoryIdentifier);

            casualMode = SunBearPrefs.CreateEntry(CasualIdentifier, false, null, 
                "Just for the cuteness and nobody is attacking anybody." +
                "\nLike the largos, they can still harvest honey but have the ability to reproduce and provide for cubs." +
                "\n\"Casual Mode (With Savage)\" is optional to enable being savage with agitation while in casual mode."
            );
            casualWSavageMode = SunBearPrefs.CreateEntry(CasualWSavageIdentifier, false);
            // mixedMode = SunBearPrefs.CreateEntry(MixedIdentifier, true);
            realisticMode = SunBearPrefs.CreateEntry(RealisticIdentifier, false, null,
                "Things just get more dangerous and you overall just get damaged a lot more." +
                "\nChances for attacking can be made higher as well including the bears being slightly faster while attacking." +
                "\nThere is also the chance of death if a bear has not been fed for too long." +
                "\n\"Realistic Mode (Without Savage)\" is optional to disable being savage with agitation while in realistic mode."
            );
            realisticWOSavageMode = SunBearPrefs.CreateEntry(RealisticWOSavageIdentifier, false);

            SunBearPrefs.SetFilePath(Path.Combine(MelonEnvironment.UserDataDirectory, CategoryIdentifier + ".cfg"));
        }

        public static bool IsCasualMode() => casualMode.Value;

        public static bool IsCasualWSavageMode() => casualWSavageMode.Value;

        // public static bool IsMixedMode() => mixedMode.Value;

        public static bool IsRealisticMode() => realisticMode.Value;

        public static bool IsRealisticWOSavageMode() => realisticWOSavageMode.Value;

        public static void EnableModesIfSavage(bool saveToFile = true, bool printmsg = false)
        {
            if (IsCasualWSavageMode())
            {
                if (!IsCasualMode())
                    MelonPreferences.SetEntryValue(CategoryIdentifier, CasualIdentifier, true);
            }

            if (IsRealisticWOSavageMode())
            {
                if (!IsRealisticMode())
                    MelonPreferences.SetEntryValue(CategoryIdentifier, RealisticIdentifier, true);
            }

            if (saveToFile)
                SunBearPrefs.SaveToFile(printmsg);
        }

        public static void DisableMultipleModes(bool saveToFile = true, bool printmsg = true)
        {
            // No you may not have multiple modes and I will make sure of that lol
            if (IsCasualMode())
            {
                if (IsRealisticWOSavageMode())
                    MelonPreferences.SetEntryValue(CategoryIdentifier, RealisticWOSavageIdentifier, false);
                if (IsRealisticMode())
                    MelonPreferences.SetEntryValue(CategoryIdentifier, RealisticIdentifier, false);
            }

            if (IsRealisticMode())
            {
                if (IsCasualWSavageMode())
                    MelonPreferences.SetEntryValue(CategoryIdentifier, CasualWSavageIdentifier, false);
                if (IsCasualMode())
                    MelonPreferences.SetEntryValue(CategoryIdentifier, CasualIdentifier, false);
            }

            if (saveToFile)
                SunBearPrefs.SaveToFile(printmsg);
        }
    }
}
