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
        public static MelonPreferences_Category Preferences { get; protected set; }

        public static MelonPreferences_Entry<bool> IsCasualMode { get; protected set; }
        public static MelonPreferences_Entry<bool> IsCasualCubs { get; protected set; }
        public static MelonPreferences_Entry<bool> IsCasualSavage { get; protected set; }

        public static MelonPreferences_Entry<bool> IsRealisticMode { get; protected set; }
        public static MelonPreferences_Entry<bool> IsRealisticNoSavage { get; protected set; }

        public static void Initialize()
        {
            Preferences = MelonPreferences.CreateCategory("SUNBEAR");

            // CASUAL
            IsCasualMode = Preferences.CreateEntry("IsCasualMode", false, "Is Casual Mode?",
                "Casual Mode disables various behaviours in order to make the Sun Bear's more friendly!\n\n" +
                "They could be considered to be most similar to the way the largos behave, although they still have their reproduction behaviours, etc."
            );

            IsCasualCubs = Preferences.CreateEntry("IsCasualCubs", false, "Is Casual Cubs?",
                "This requires for \"Is Casual Mode\" to also be enabled, if not it will be done automatically.\n\n" +
                "With this configuration enabled, the cubs will become less-restricted and you could even suck them into your VacPack or feed them!\n\n" +
                "(Note! They will not produce anything when you decide to feed them, it will simply get rid of their hunger.)"
            );

            IsCasualSavage = Preferences.CreateEntry("IsCasualSavage", false, "Is Casual With Savage?",
                "This requires for \"Is Casual Mode\" to also be enabled, if not it will be done automatically.\n\n" +
                "With this configuration enabled, you will still have 'Casual Mode' although it will add the savage behaviours that the Sun Bear's originally possess.\n\n" +
                "Therefore adding additional difficulty while still removing some difficulty?"
            );

            // REALISTIC
            IsRealisticMode = Preferences.CreateEntry("IsRealisticMode", false, "Is Realistic Mode?",
                "Realistic Mode enhances various behaviours in order to make the Sun Bear's more dangerous!\n\n" +
                "They may become faster, deadlier, etc. An interesting additional is if they aren't fed, they will disappear due to starvation."
            );

            IsRealisticNoSavage = Preferences.CreateEntry("IsRealisticNoSavage", false, "Is Realistic With No Savage?",
                "This requires for \"Realistic Mode\" to also be enabled, if not it will be done automatically.\n\n" +
                "With this configuration enabled, you will still have the 'Realistic Mode' although it will remove the savage behaviours that the Sun Bear's originally possess."
            );

            Preferences.SetFilePath(Path.Combine(MelonEnvironment.UserDataDirectory, "SunBearPreferences.cfg"));
            EnableModesIfOtherIsEnabled();
            PreventMultiModeEnabled();
        }

        public static void EnableModesIfOtherIsEnabled()
        {
            if (!IsCasualMode.Value)
            {
                if (IsCasualCubs.Value)
                    IsCasualMode.Value = true;

                if (IsCasualSavage.Value)
                    IsCasualMode.Value = true;
            }

            if (!IsRealisticMode.Value)
                if (IsRealisticNoSavage.Value)
                    IsRealisticMode.Value = true;

            Preferences.SaveToFile();
        }

        public static void PreventMultiModeEnabled()
        {
            if (IsCasualMode.Value)
            {
                if (IsRealisticMode.Value)
                {
                    IsRealisticMode.Value = false;
                    IsRealisticNoSavage.Value = false;
                }
            }
            else if (IsRealisticMode.Value) // Just in case . . .
            {
                if (IsCasualMode.Value)
                {
                    IsCasualMode.Value = false;
                    IsCasualCubs.Value = false;
                    IsCasualSavage.Value = false;
                }
            }

            Preferences.SaveToFile();
        }
    }
}
