using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.Damage;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using SUNBEAR.Assist;
using SUNBEAR.Data.Slimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR
{
    internal class LocalInstances
    {
        /*
        // Start
        internal static PediaPage defensePediaPage;
        internal static PediaPage providingPediaPage;
        internal static PediaPage reproductionPediaPage;

        // Middle
        internal static PediaPage cubPediaPage;
        internal static PediaPage savagePediaPage;
        internal static PediaPage harvestingPediaPage;

        // End
        internal static PediaPage factsPediaPage;
        */

        internal static FearProfile sunBearSlimeFearProfile;
        internal static FearProfile fireSlimeSBAFearProfile;
        internal static FearProfile slimeStandardSBAFearProfile;

        // internal static GameIconDefinition gameIconSlimeSunBear;

        internal static IdentifiableTypeGroup sunBearLargoGroup;
        internal static DamageSourceDefinition sunBearAttack;

        public static void PatchInitialize()
        {
            // SUN BEAR LARGO GROUP
            sunBearLargoGroup = ScriptableObject.CreateInstance<IdentifiableTypeGroup>();
            sunBearLargoGroup.hideFlags |= HideFlags.HideAndDontSave;
            sunBearLargoGroup.name = "SunBearLargoGroup";

            sunBearLargoGroup._memberTypes = new();
            sunBearLargoGroup._memberGroups = new();
            sunBearLargoGroup._localizedName = new();

            sunBearLargoGroup.AllowedCategories = new();
        }

        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        /*
                        // PEDIA PAGES
                        // -- START
                        defensePediaPage = PediaHelper.CreatePediaSection("Bearpedia Defensive", "Never sneak up on a bear..", Get<PediaPage>("Slimeology").Icon);
                        defensePediaPage.hideFlags |= HideFlags.HideAndDontSave;

                        providingPediaPage = PediaHelper.CreatePediaSection("Bearpedia Provide", "\"I will provide!\"", Get<PediaPage>("Slimeology").Icon);
                        providingPediaPage.hideFlags |= HideFlags.HideAndDontSave;

                        reproductionPediaPage = PediaHelper.CreatePediaSection("Bearpedia Reproduction", "Where did that come from!?", Get<PediaPage>("Slimeology").Icon);
                        reproductionPediaPage.hideFlags |= HideFlags.HideAndDontSave;

                        // -- MIDDLE
                        cubPediaPage = PediaHelper.CreatePediaSection("Bearpedia Cubs", "Pure cuteness!", Get<PediaPage>("Slimeology").Icon);
                        cubPediaPage.hideFlags |= HideFlags.HideAndDontSave;

                        savagePediaPage = PediaHelper.CreatePediaSection("Bearpedia Savage", "Feeeeed meeeee!", Get<PediaPage>("Slimeology").Icon);
                        savagePediaPage.hideFlags |= HideFlags.HideAndDontSave;

                        harvestingPediaPage = PediaHelper.CreatePediaSection("Bearpedia Harvest", "Delicious Honey! Yum!", Get<PediaPage>("Slimeology").Icon);
                        harvestingPediaPage.hideFlags |= HideFlags.HideAndDontSave;
                        

                        // END
                        factsPediaPage = PediaHelper.CreatePediaSection("Bearpedia Savage", "A few facts to end it off!", Get<PediaPage>("OnTheRanchResource").Icon);
                        factsPediaPage.hideFlags |= HideFlags.HideAndDontSave;
                        */

                        /* GAME ICON SLIME SUN BEAR
                        gameIconSlimeSunBear = ScriptableObject.CreateInstance<GameIconDefinition>();
                        gameIconSlimeSunBear.hideFlags |= HideFlags.HideAndDontSave;
                        gameIconSlimeSunBear.name = "GameIconSlimeSunBear";
                        gameIconSlimeSunBear.gameIcon = LocalAssets.iconSlimeSunBearSpr;
                        gameIconSlimeSunBear.persistenceId = "gameIcon_sunBearSlime";
                        Get<GameIconDefinitionCollection>("GameIconCollection").items.TryAdd(gameIconSlimeSunBear);*/

                        // SUN BEAR DAMAGE SOURCE
                        sunBearAttack = ScriptableObject.CreateInstance<DamageSourceDefinition>();
                        sunBearAttack.hideFlags |= HideFlags.HideAndDontSave;
                        sunBearAttack.name = "SunBearAttack";
                        sunBearAttack._logMessage = "SunBearAttack.Damage";

                        // SUN BEAR FEAR PROFILE
                        sunBearSlimeFearProfile = UnityEngine.Object.Instantiate(Get<FearProfile>("slimeStandardFearProfile"));
                        sunBearSlimeFearProfile.hideFlags |= HideFlags.HideAndDontSave;
                        sunBearSlimeFearProfile.name = "sunBearSlimeFearProfile";

                        Il2CppSystem.Collections.Generic.List<FearProfile.ThreatEntry> threatEntries =
                            new Il2CppSystem.Collections.Generic.List<FearProfile.ThreatEntry>();

                        Il2CppSystem.Collections.Generic.Dictionary<IdentifiableType, FearProfile.ThreatEntry> threatDictionary =
                            new Il2CppSystem.Collections.Generic.Dictionary<IdentifiableType, FearProfile.ThreatEntry>();

                        threatEntries.Add(Get<FearProfile>("slimeStandardFearProfile").Threats[2]);
                        threatDictionary.Add(Get<IdentifiableType>("FireColumn"), Get<FearProfile>("slimeStandardFearProfile").Threats[2]);

                        sunBearSlimeFearProfile.Threats = threatEntries;
                        sunBearSlimeFearProfile._threatsLookup = threatDictionary;

                        // SUN BEAR ATTACK FEAR PROFILE
                        slimeStandardSBAFearProfile = UnityEngine.Object.Instantiate(Get<FearProfile>("slimeStandardFearProfile"));
                        slimeStandardSBAFearProfile.hideFlags |= HideFlags.HideAndDontSave;
                        slimeStandardSBAFearProfile.name = "slimeStandardSBAFearProfile";

                        fireSlimeSBAFearProfile = UnityEngine.Object.Instantiate(Get<FearProfile>("fireSlimeFearProfile"));
                        fireSlimeSBAFearProfile.hideFlags |= HideFlags.HideAndDontSave;
                        fireSlimeSBAFearProfile.name = "fireSlimeSBAFearProfile";

                        if (slimeStandardSBAFearProfile.Threats.Count > 0 && fireSlimeSBAFearProfile.Threats.Count > 0)
                        {
                            FearProfile.ThreatEntry threatEntry = new FearProfile.ThreatEntry()
                            {
                                IdentType = SunBear.sunBearSlime,
                                MaxSearchRadius = slimeStandardSBAFearProfile.Threats[0].MaxSearchRadius,
                                MinSearchRadius = slimeStandardSBAFearProfile.Threats[0].MinSearchRadius,
                                MaxThreatFearPerSec = slimeStandardSBAFearProfile.Threats[0].MaxThreatFearPerSec,
                                MinThreatFearPerSec = slimeStandardSBAFearProfile.Threats[0].MinThreatFearPerSec
                            };

                            // STANDARD
                            if (slimeStandardSBAFearProfile.Threats.Contains(threatEntry))
                                return;
                            else
                                slimeStandardSBAFearProfile.Threats.Add(threatEntry);

                            if (slimeStandardSBAFearProfile._threatsLookup.ContainsKey(SunBear.sunBearSlime) || slimeStandardSBAFearProfile._threatsLookup.ContainsValue(threatEntry))
                                return;
                            else
                                slimeStandardSBAFearProfile._threatsLookup.Add(SunBear.sunBearSlime, threatEntry);

                            // FIRE
                            if (fireSlimeSBAFearProfile.Threats.Contains(threatEntry))
                                return;
                            else
                                fireSlimeSBAFearProfile.Threats.Add(threatEntry);

                            if (fireSlimeSBAFearProfile._threatsLookup.ContainsKey(SunBear.sunBearSlime) || fireSlimeSBAFearProfile._threatsLookup.ContainsValue(threatEntry))
                                return;
                            else
                                fireSlimeSBAFearProfile._threatsLookup.Add(SunBear.sunBearSlime, threatEntry);
                        }
                        break;
                    }
            }
        }
    }
}
