using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Script.Util.Extensions;
using Il2CppInterop.Runtime;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppMonomiPark.SlimeRancher.World;
using Il2CppSystem.Dynamic.Utils;
using MelonLoader;
using SUNBEAR.Assist;
using SUNBEAR.Assist.Helpers;
using SUNBEAR.Components;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;
using UnityEngine.Playables;

namespace SUNBEAR.Data.Slimes
{
    internal class SunBear
    {
        internal static List<Il2CppSystem.Type> casualExcludedComponents = new List<Il2CppSystem.Type>();
        internal static List<Il2CppSystem.Type> grownExcludedComponents = new List<Il2CppSystem.Type>();
        internal static List<Il2CppSystem.Type> largoExcludedComponents = new List<Il2CppSystem.Type>();
        internal static List<Il2CppSystem.Type> cubExcludedComponents = new List<Il2CppSystem.Type>();

        internal static SlimeDefinition sunBearSlime;
        internal static SlimeDefinition cubSunBearSlime;
        internal static IdentifiableType sunBearGordo;
        internal static IdentifiableType sunBearPlort;

        internal static GameObject moduleSlimeSunBear;
        internal static GameObject sunBearTriggers;
        internal static Color[] sunBearPalette =
        {
            LoadHex("#1a1718"),
            LoadHex("#201d1e"),
            LoadHex("#fed8b1"),
            LoadHex("#edad6c"),
            LoadHex("#85abce"),
            LoadHex("#8a3034")
        };

        public static void Initialize()
        {
            // -- MODULE
            moduleSlimeSunBear = new GameObject("moduleSlimeSunBear");
            moduleSlimeSunBear.Prefabitize();
            moduleSlimeSunBear.hideFlags |= HideFlags.HideAndDontSave;
            // moduleSlimeSunBear.tag = "Sun Bear Slime";

            moduleSlimeSunBear.AddComponent<SlimeEat>();
            moduleSlimeSunBear.AddComponent<ReactToToyNearby>();
            moduleSlimeSunBear.AddComponent<AttackPlayer>();
            moduleSlimeSunBear.AddComponent<GotoPlayer>();

            moduleSlimeSunBear.AddComponent<SunBearReproduction>();
            // moduleSlimeSunBear.AddComponent<SunBearIsolation>();
            moduleSlimeSunBear.AddComponent<SunBearProvide>();
            moduleSlimeSunBear.AddComponent<SunBearHarvest>();
            moduleSlimeSunBear.AddComponent<SunBearAttack>();
            moduleSlimeSunBear.AddComponent<SunBearSavage>();

            moduleSlimeSunBear.AddComponent<SunBearGrowth>();
            moduleSlimeSunBear.AddComponent<SunBearFollow>();
            moduleSlimeSunBear.AddComponent<SunBearPlayful>();

            moduleSlimeSunBear.AddComponent<SunBearCache>();
            moduleSlimeSunBear.AddComponent<SBGeneralizedBehaviour>();

            cubExcludedComponents = new List<Il2CppSystem.Type>
            {
                // Il2CppType.Of<SunBearEnvironmental>(),
                Il2CppType.Of<SunBearReproduction>(),
                // Il2CppType.Of<SunBearIsolation>(),
                // Il2CppType.Of<SunBearTentacleBite>(),
                Il2CppType.Of<SunBearProvide>(),
                Il2CppType.Of<SunBearHarvest>(),
                Il2CppType.Of<SunBearAttack>(),
                Il2CppType.Of<SunBearSavage>()
            };

            largoExcludedComponents = new List<Il2CppSystem.Type>()
            {
                Il2CppType.Of<SunBearReproduction>(),
                Il2CppType.Of<SunBearProvide>(),
                /*Il2CppType.Of<SunBearAttack>(),
                Il2CppType.Of<SunBearSavage>()*/
            };

            grownExcludedComponents = new List<Il2CppSystem.Type>()
            {
                Il2CppType.Of<SunBearGrowth>(),
                Il2CppType.Of<SunBearFollow>(),
                Il2CppType.Of<SunBearPlayful>()
            };

            casualExcludedComponents = new List<Il2CppSystem.Type>()
            {
                Il2CppType.Of<SunBearAttack>(),
                Il2CppType.Of<SunBearSavage>(),
            };

            // -- TRIGGERS
            sunBearTriggers = new GameObject("SunBearTriggers");
            sunBearTriggers.Prefabitize();
            sunBearTriggers.hideFlags |= HideFlags.HideAndDontSave;
            sunBearTriggers.transform.parent = moduleSlimeSunBear.transform;

            // - PROTECTION TRIGGER
            GameObject sunBearProtectionTrigger = new GameObject("SunBearProtectionTrigger");
            sunBearProtectionTrigger.transform.parent = sunBearTriggers.transform;
            sunBearProtectionTrigger.AddComponent<SphereCollider>().radius = 10;
            sunBearProtectionTrigger.AddComponent<SunBearProtection>();
            sunBearProtectionTrigger.GetComponent<SphereCollider>().isTrigger = true;

            // -- CACHE TRIGGER
            GameObject sunBearCacheTrigger = new GameObject("SunBearCacheTriggers");
            sunBearCacheTrigger.transform.parent = sunBearTriggers.transform;

            // --- CUB TRIGGER
            GameObject cubTrigger = new GameObject("CubTrigger");
            cubTrigger.transform.parent = sunBearCacheTrigger.transform;
            cubTrigger.AddComponent<SphereCollider>().radius = 10;
            cubTrigger.AddComponent<SunBearCacheTrigger>();
            cubTrigger.GetComponent<SphereCollider>().isTrigger = true;

            // --- BEAR TRIGGER
            GameObject bearTrigger = new GameObject("BearTrigger");
            bearTrigger.transform.parent = sunBearCacheTrigger.transform;
            bearTrigger.AddComponent<SphereCollider>().radius = 15;
            bearTrigger.AddComponent<SunBearCacheTrigger>();
            bearTrigger.GetComponent<SphereCollider>().isTrigger = true;

            // --- HIVE TRIGGER
            GameObject hiveTrigger = new GameObject("HiveTrigger");
            hiveTrigger.transform.parent = sunBearCacheTrigger.transform;
            hiveTrigger.AddComponent<SphereCollider>().radius = 18;
            hiveTrigger.AddComponent<SunBearCacheTrigger>();
            hiveTrigger.GetComponent<SphereCollider>().isTrigger = true;

            // --- SLIME LARGO TRIGGER
            GameObject slimeLargoTrigger = new GameObject("SlimeLargoTrigger");
            slimeLargoTrigger.transform.parent = sunBearCacheTrigger.transform;
            slimeLargoTrigger.AddComponent<SphereCollider>().radius = 20;
            slimeLargoTrigger.AddComponent<SunBearCacheTrigger>();
            slimeLargoTrigger.GetComponent<SphereCollider>().isTrigger = true;
        }

        internal class Grown
        {
            public static void Initialize()
            {
                sunBearSlime = ScriptableObject.CreateInstance<SlimeDefinition>();
                sunBearSlime.hideFlags |= HideFlags.HideAndDontSave;
                sunBearSlime.name = "SunBear";
                sunBearSlime.Name = sunBearSlime.name;

                sunBearSlime.CanLargofy = true;
                sunBearSlime.color = sunBearPalette[0];
                sunBearSlime.localizationSuffix = "sun_bear_slime";

                sunBearPlort = ScriptableObject.CreateInstance<IdentifiableType>();
                sunBearPlort.hideFlags |= HideFlags.HideAndDontSave;
                sunBearPlort.name = "SunBearPlort";

                sunBearPlort.IsPlort = true;
                sunBearPlort.color = sunBearPalette[1];
                sunBearPlort.localizationSuffix = "sun_bear_plort";
            }

            public static void LoadSlimepedia()
            {
                IdentifiablePediaEntry sunBearEntry = PediaHelper.AddSlimepedia(sunBearSlime, "SunBear", "While not the most popular, the unique patch on their fur will remind you of who they are!")?.Cast<IdentifiablePediaEntry>();
                PediaHelper.AddSlimepediaSection(sunBearEntry,
                    "The Sun Bear slime, distinct for its fur patch and texture, is a unique and speedy species. " +
                    "They are defensive, preferring isolation, especially when cubs are present. " +
                    "Reproduction is rare but only requires one bear, taking 3-7 days for 1-2 cubs, which can grow aggressive once fully grown. " +
                    "Once Sun Bears become largos, they lose the ability to reproduce. " +
                    "Cubs need a caretaker Sun Bear for proper care and feeding, making it impossible to take them home. " +
                    "Sun Bears harvest Wild Honey, and hungry ones may turn savage, requiring quick feeding. " +
                    "Cubs are playful and grow into ranchable bears after 3-5 days. " +
                    "Key facts include their resistance to vacpack in attack mode, increased attack chances based on hunger, agitation, or cub presence. " +
                    "Largos, more domesticated post-merging, can go savage, adding complexity to ranching."
                );
                PediaHelper.AddSlimepediaSection(sunBearEntry,
                    "Due to the Sun Bear slimes preferring isolation, they may perceive you as a threat simply for approaching them. " +
                    "In such situations, it's best to run, as they will eventually cease pursuit once you exit their proximity—considerably more amiable than other bears. " +
                    "When they turn savage, you can deter them by splashing water or offering food. " +
                    "This is considerably more dangerous than a feral state, as they become fixated solely on one thing: their target. " +
                    "This could be considered the same for their usual attacking.", true
                );
                PediaHelper.AddSlimepediaSection(sunBearEntry,
                    "Sun Bear plorts were long shrouded in mystery, with unconfirmed claims on slime forums about their potential effects. " +
                    "Recently, scientists revealed confidential research indicating that these plorts show extraordinary abilities when near garden crops. " +
                    "However, the catch is that this ability can only be utilized every 12 hours in a ranching setting.", false, true
                    /*"Sun Bear plorts are a rather mysterious type of plort. " +
                    "On slime forums, individuals have claimed that these plorts can enhance food growth and reduce decay. " +
                    "Scientists have yet to validate these claims and are keeping their research on the plort confidential. " +
                    "It is hoped that more information about the plort will be made available soon.", false, true*/
                );

                /*string generalSlimeology =
                    "<i>This is gonna be a long slimeology..</i>\n\n" +
                    "Sun Bear slimes may not always look like the other slimes. " +
                    "Their unique patch is what separates them from the others, including their unique body texture. " +
                    "They generally jump lower than the other slimes as well but are very fast! Their cubs however are not as fast and jump even lower. " +
                    "When a cub grows, they're no longer under the safe care of their caretaker.. but then they can defend themselves. " +
                    "Whoever it is against, they've gotten themselves in some trouble! Possibly the Sun Bear themselves as well.\n\n" +
                    "Sun Bear slimes are quite the unique slime, getting into everything they can do can be a handful! So that's why <b>everything will be separated into sections</b>.\n" +
                    "Learn more about the Sun Bear slimes and everything they're capable of in the next pages!\n\n" +
                    "<b> Guide to Bear Defense -> </b>";

                string defenseSlimeology =
                    "<b>Never sneak up on a bear..</b>\n\n" +
                    "While Sun Bear slimes may be cute, they do tend to be.. rather isolated to themselves and very defensive, <b>especially if cubs are around</b>.\n" +
                    "It's a way of protecting themselves from unwanted threats and preferring to be alone rather than with a bunch of others. " +
                    "Does this make them incapable of being ranched? No! They will not always attack but it's best to know how you should handle them. " +
                    "If in some case they do attack, they've noticed you and will always run after you. It's best to just run until they're uninterested which can be quite easy a lot of the time! " +
                    "Once uninterested, you can try to go near them again and hope that they won't attack you again, repeat this process till they're happy around you and take one home!\n\n" +
                    "Just to clarify, cubs are harmless! Though normally to be seen around a caretaker, it can be very dangerous to go near them. Unfortunately though, you cannot take them home <i>yourself</i>. This will be explained more later.\n" +
                    "Here's another fun fact! When a Sun Bear becomes a largo, they become more domesticated by merging with a more less-wild slime. This can make them easier to handle and they no longer attack.. unless they're savage however.\n\n" +
                    "<b> <- General | How're cubs made? -> </b>";

                string reproductionSlimeology =
                    "<b>Did.. a cub just appear out of nowhere?</b>\n\n" +
                    "Like any bear, they can have cubs! There is also <i>luckily</i> no need for another bear for them to reproduce.\n" +
                    "This is not a very common process though, it can take up to 3 - 7 days for a Sun Bear to reproduce a number of 1 - 2 cubs. " +
                    "What could you get out of this? Well you get to have cute cubs around! Though once the cubs grow up, you could ranch them. " +
                    "It can be difficult though in the first few seconds of growing up, once the cubs are grown.. they <i>could</i> fight their caretaker. This is all part of the isolation process. " +
                    "Figure out your own techniques/strategies to prevent this from happening if you'd like. It'll be really difficult to try and separate them once the attacking part has began.\n\n" +
                    "Another thing about the Sun Bear largos is that they also lose the ability to reproduce. Sad really. So it's best to not expect to get cubs from a Sun Bear that has been transformed into a largo.\n\n" +
                    "<b> <- Guide to Bear Defense | Providing for Cubs -> </b>";

                string providingSlimeology =
                    "<b>Naturally providing for cubs nearby. :3</b>\n\n" +
                    "Cub Sun Bear slimes are very cute creatures, all they wanna do is play! Though without their caretaker, they wouldn't be able to keep their life together.\n" +
                    "Cubs <b>require</b> a caretaker, one of who is a grown Sun Bear. They provide for them and keep them fed, regardless of if the cub is following them or not. " +
                    "Without them, the cubs would not go through their growing process due to not being fed and sometimes could result to more.. fatal consequences. " +
                    "This is one of the primary reasons that you cannot take cubs home with you, it would be impossible to provide for them unless a grown Sun Bear was doing it for you.\n\n" +
                    "Every few hours, a grown Sun Bear will attempt to provide for cubs nearby. This should keep them in good shape to grow up without any delays! " +
                    "Of course if the Sun Bear is in not good shape to feed cubs, they cannot and this could affect the cub badly.\n\n" +
                    "<b> <- How're cubs made? | Wild Honey Harvesting -> </b>";

                string harvestingSlimeology =
                    "<b>Delicious Honey! Yum!</b>\n\n" +
                    "Sun Bear slimes are known for their unique fur patch but they're also known for their love for honey!\n" +
                    "With this, they have the unique ability of finding and harvesting their own <b>Wild Honey</b>. " +
                    "Sometimes while on your own journey to harvest resources, you may see a Sun Bear slime harvesting some honey of their own. Who will get to it first? " +
                    "Just try not to get attacked in the process, it is their favorite food after all! While they're harvesting though, they may have some difficulties of their own so maybe even help them out if possible? Hmm..\n\n" +
                    "<b> <- Providing for Cubs | Bears went Savage -> </b>";

                string savageSlimeology =
                    "<b>Feeeeed meeeee! >:[</b>\n\n" +
                    "While you may have a Sun Bear on your ranch or out in the wild that may not be attacking anyone.. it can all go down hill if they're hungry.\n" +
                    "When Sun Bear slimes are hungry enough, excluding cubs but includes largos.. they tend to get very aggressive. They'll go savage, it's like being feral but could be much more deadly. " +
                    "A pure Sun Bear slime will attempt to attack other slimes nearby and perhaps have a snack. Simple as that. " +
                    "Largos on the other hand will attack you and you only. Wild or not, only you. They do not want to chomp on other slimes. " +
                    "This can also be resolved once they've eaten though. Either it's a slime or something you feed them! Just make it quick before they decide to go after you..\n\n" +
                    "While it may seem like something you should be afraid of <i>which you probably should</i>, think about them. They're probably starving! :[\n\n" +
                    "<b> <- Wild Honey Harvesting | Guide to Bear Cubs -> </b>";

                string cubSlimeology =
                    "<b>Pure cuteness! :D</b>\n\n" +
                    "Cub Sun Bear slimes are one of the cutest things you may see! While it is unfortunate that you cannot take them with you, they're not completely useless! Why would you consider them useless..\n" +
                    "Cubs are very playful, they just love to play till they finally hit that big growth spurt! You'll see them jump around, roll and maybe do the occasional bite! Completely harmless. " +
                    "They'll also follow whichever grown up bear they choose to follow, this bear will <i>most likely</i> not harm them. They must follow this bear for protection, care, etc. It's nature. " +
                    "When it's finally time for their growth, which usually happens within 3 - 5 days, their size will transition to be bigger and they'll be all grown up. This is a bear that you could ranch once grown!\n\n" +
                    "<b> <- Bears went Savage | Sun Bear Facts -> </b>";

                string factsSlimeology =
                    "<b>A few facts to end it off!</b>\n\n" +
                    "While a lot of information about the Sun Bear slimes has been listed, here are some facts that you could read to get just a bit more information on the slime!\n\n" +
                    "<b>1.</b> The vacpack has no effect on them while in attack mode.\n" +
                    "<b>2.</b> Attack chances are increased based on hunger, agitation or if cubs are around. Tarrs are always attacked if found within their proximity. This does not mean the attack is always successful.\n" +
                    "<b>3.</b> Largos become more domesticated when merging with a less-wild slime. Due to this, the only things largos are capable of doing is harvesting honey and going savage. " +
                    "While this could be good for ranching in a more safer way with Sun Bear slimes, when they go savage.. it can get very complicated. Any kind of feral is also replaced with them going savage.\n\n" +
                    "And that's all! Thanks for reading up on the Sun Bear slimes and have fun having your own little adventure with them.\n\n" +
                    "<b> <- Guide to Bear Cubs | Rest of the Slimepedia -> </b>";

                string bearRisks =
                    "Read <b>\"Guide to Bear Defense\"</b>\n" +
                    "Read <b>\"Bears went Savage\"</b>\n" +
                    "Anything else could be hidden within the text.";

                string bearPlortonomics =
                    "Sun Bear plorts have the ability to help the environment, at least that's what the forums say. " +
                    "Some have rumoured that these plorts could help the plants around them and help them grow faster and rot slower. " +
                    "Nobody has been sure though and scientist are keeping their work concealed for the Sun Bear plorts <i>as of now</i>.";

                MelonLogger.Msg(sunBearEntry.name);
                PediaHelper.AddSlimepediaSection(sunBearEntry, generalSlimeology);

                PediaHelper.AddPediaSection(sunBearEntry, LocalInstances.defensePediaPage, defenseSlimeology);
                PediaHelper.AddPediaSection(sunBearEntry, LocalInstances.reproductionPediaPage, reproductionSlimeology);
                PediaHelper.AddPediaSection(sunBearEntry, LocalInstances.providingPediaPage, providingSlimeology);
                PediaHelper.AddPediaSection(sunBearEntry, LocalInstances.harvestingPediaPage, harvestingSlimeology);
                PediaHelper.AddPediaSection(sunBearEntry, LocalInstances.savagePediaPage, savageSlimeology);
                PediaHelper.AddPediaSection(sunBearEntry, LocalInstances.cubPediaPage, cubSlimeology);
                PediaHelper.AddPediaSection(sunBearEntry, LocalInstances.factsPediaPage, factsSlimeology);

                PediaHelper.AddSlimepediaSection(sunBearEntry, bearRisks);
                PediaHelper.AddSlimepediaSection(sunBearEntry, bearPlortonomics);*/
            }

            public static void Load(string sceneName)
            {
                switch (sceneName)
                {
                    case "GameCore":
                        {
                            sunBearSlime.localizedName = GeneralizedHelper.CreateTranslation("Actor", "l." + sunBearSlime.localizationSuffix, "Sun Bear Slime");
                            sunBearPlort.localizedName = GeneralizedHelper.CreateTranslation("Actor", "l." + sunBearPlort.localizationSuffix, "Sun Bear Plort");

                            #region SUN_BEAR_PLORT
                            sunBearPlort.icon = LocalAssets.iconPlortSunBearSpr;

                            // MATERIAL
                            Material plortMaterial = UnityEngine.Object.Instantiate(Get<GameObject>("plortPink").GetComponent<MeshRenderer>().sharedMaterial);
                            plortMaterial.hideFlags |= HideFlags.HideAndDontSave;
                            plortMaterial.name = "plortSunBearBase";
                            plortMaterial.SetColor("_TopColor", sunBearPalette[2]);
                            plortMaterial.SetColor("_MiddleColor", sunBearPalette[1]);
                            plortMaterial.SetColor("_BottomColor", sunBearPalette[0]);
                            plortMaterial.SetTexture("_StripeTexture", LocalAssets.stripesSunBearPlort);

                            // PREFAB
                            sunBearPlort.prefab = PrefabUtils.CopyPrefab(Get<IdentifiableType>("PinkPlort").prefab);
                            sunBearPlort.prefab.hideFlags |= HideFlags.HideAndDontSave;
                            sunBearPlort.prefab.name = "plortSunBear";
                            sunBearPlort.prefab.AddComponent<SunBearPlortonomics>();
                            sunBearPlort.prefab.GetComponent<Identifiable>().identType = sunBearPlort;
                            sunBearPlort.prefab.GetComponent<MeshRenderer>().sharedMaterial = plortMaterial;

                            /*SECTR_PointSource pointSource = sunBearPlort.prefab.AddComponent<SECTR_PointSource>();
                            pointSource.pitch = 1;
                            pointSource.Loop = false;
                            pointSource.PlayOnStart = false;
                            pointSource.RestartLoopsOnEnabled = false;
                            pointSource.Cue = Get<SECTR_AudioCue>("SiloReward");*/

                            // REGISTRY
                            GeneralizedHelper.RegisterPlortEntry(new MarketUI.PlortEntry() { identType = sunBearPlort });
                            GeneralizedHelper.RegisterValueMap(new EconomyDirector.ValueMap
                            {
                                Accept = sunBearPlort.prefab.GetComponent<Identifiable>(),
                                FullSaturation = 20,
                                Value = 42
                            });

                            // EATMAPS
                            foreach (IdentifiableTypeGroup largoGroup in Get<IdentifiableTypeGroup>("LargoGroup").memberGroups)
                            {
                                foreach (IdentifiableType largoType in largoGroup.memberTypes)
                                {
                                    SlimeDefinition largoDef = largoType.Cast<SlimeDefinition>();
                                    if (largoDef == null || largoDef.Diet == null || !largoDef.IsLargo)
                                        continue;
                                    largoDef.Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, largoDef);
                                }
                            }
                            #endregion

                            #region SUN_BEAR_SLIME
                            // DEFINITION
                            sunBearSlime.BaseModule = Get<GameObject>("moduleSlimeStandard");
                            sunBearSlime.BaseSlimes = new SlimeDefinition[0];
                            sunBearSlime.FavoriteToyIdents = new ToyDefinition[0];
                            sunBearSlime.SlimeModules = new GameObject[] { moduleSlimeSunBear };
                            sunBearSlime.Sounds = Get<SlimeSounds>("Standard");
                            sunBearSlime.NativeZones = new ZoneDefinition[] { Get<ZoneDefinition>("Luminous Strand") };
                            sunBearSlime.showForZones = sunBearSlime.NativeZones;

                            // PREFAB
                            sunBearSlime.prefab = PrefabUtils.CopyPrefab(Get<GameObject>("slimePink"));
                            sunBearSlime.prefab.name = "slimeSunBear";
                            sunBearSlime.PrefabScale = 1.5f;
                            sunBearSlime.prefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                            sunBearSlime.prefab.AddComponent<SBGeneralizedBehaviour>();
                            // sunBearSlime.prefab.AddComponent<SunBearEnvironmental>();
                            sunBearSlime.prefab.AddComponent<SunBearReproduction>();
                            // sunBearSlime.prefab.AddComponent<SunBearIsolation>();
                            // sunBearSlime.prefab.AddComponent<SunBearTentacleBite>();
                            sunBearSlime.prefab.AddComponent<SunBearProvide>();
                            sunBearSlime.prefab.AddComponent<SunBearHarvest>();
                            sunBearSlime.prefab.AddComponent<SunBearAttack>();
                            sunBearSlime.prefab.AddComponent<SunBearSavage>();
                            sunBearSlime.prefab.AddComponent<SunBearCache>();
                            sunBearSlime.prefab.AddComponent<SunBearGoto>();

                            sunBearSlime.prefab.AddComponent<GotoPlayer>().PlayerIdentifiableType = Get<IdentifiableType>("Player");
                            sunBearSlime.prefab.AddComponent<AttackPlayer>()._playerIdentifiableType = Get<IdentifiableType>("Player");
                            sunBearSlime.prefab.GetComponent<AttackPlayer>()._damageSource = LocalInstances.sunBearAttack;
                            sunBearSlime.prefab.GetComponent<AttackPlayer>().DamagePerAttack = 30;
                            /*sunBearSlime.prefab.GetComponent<SlimeFeral>().feralLifetimeHours = float.PositiveInfinity;
                            sunBearSlime.prefab.GetComponent<SlimeFeral>().dynamicToFeral = false;*/

                            sunBearSlime.prefab.GetComponent<Identifiable>().identType = sunBearSlime;
                            sunBearSlime.prefab.GetComponent<SlimeEat>().SlimeDefinition = sunBearSlime;
                            sunBearSlime.prefab.GetComponent<PlayWithToys>().SlimeDefinition = sunBearSlime;
                            sunBearSlime.prefab.GetComponent<ReactToToyNearby>().SlimeDefinition = sunBearSlime;
                            sunBearSlime.prefab.GetComponent<SlimeVarietyModules>().BaseModule = sunBearSlime.BaseModule;
                            sunBearSlime.prefab.GetComponent<SlimeVarietyModules>().SlimeModules = sunBearSlime.SlimeModules;
                            sunBearSlime.prefab.GetComponent<Vacuumable>().size = Vacuumable.Size.LARGE;

                            sunBearSlime.prefab.GetComponent<SlimeHealth>().MaxHealth = 40;
                            sunBearSlime.prefab.GetComponent<FleeThreats>().FearProfile = LocalInstances.sunBearSlimeFearProfile;
                            sunBearSlime.prefab.GetComponent<SlimeRandomMove>()._maxJump = 4;

                            if (SunBearPreferences.IsRealisticMode())
                            {
                                sunBearSlime.prefab.GetComponent<SlimeRandomMove>().ScootSpeedFactor = 2.5f;
                                sunBearSlime.prefab.GetComponent<GotoConsumable>().PursuitSpeedFactor = 2.5f;
                            }
                            else
                            {
                                sunBearSlime.prefab.GetComponent<SlimeRandomMove>().ScootSpeedFactor = 2;
                                sunBearSlime.prefab.GetComponent<GotoConsumable>().PursuitSpeedFactor = 2;
                            }

                            GameObject instantiatedTriggers = UnityEngine.Object.Instantiate(sunBearTriggers);
                            instantiatedTriggers.transform.parent = sunBearSlime.prefab.transform;

                            if (SunBearPreferences.IsCasualMode())
                                UnityEngine.Object.Destroy(sunBearSlime.prefab.transform.Find("SunBearTriggers(Clone)/SunBearProtectionTrigger").gameObject);

                            foreach (Il2CppSystem.Type excludedComponent in grownExcludedComponents)
                            {
                                if (excludedComponent == null)
                                    continue;

                                if (sunBearSlime.prefab.GetComponent(excludedComponent))
                                    UnityEngine.Object.Destroy(sunBearSlime.prefab.GetComponent(excludedComponent));
                            }

                            if (SunBearPreferences.IsCasualMode())
                            {
                                foreach (Il2CppSystem.Type excludedComponent in casualExcludedComponents)
                                {
                                    if (excludedComponent == null)
                                        continue;

                                    if (SunBearPreferences.IsCasualWSavageMode() && excludedComponent == Il2CppType.Of<SunBearSavage>())
                                        continue;

                                    if (sunBearSlime.prefab.GetComponent(excludedComponent))
                                        UnityEngine.Object.Destroy(sunBearSlime.prefab.GetComponent(excludedComponent));
                                }
                            }

                            if (SunBearPreferences.IsRealisticMode() && SunBearPreferences.IsRealisticWOSavageMode())
                                UnityEngine.Object.Destroy(sunBearSlime.prefab.GetComponent<SunBearSavage>());

                            UnityEngine.Object.Destroy(sunBearSlime.prefab.GetComponent<AweTowardsLargos>());
                            UnityEngine.Object.Destroy(sunBearSlime.prefab.GetComponent<PinkSlimeFoodTypeTracker>());
                            UnityEngine.Object.Destroy(sunBearSlime.prefab.GetComponent<ColliderTotemLinkerHelper>());
                            UnityEngine.Object.Destroy(sunBearSlime.prefab.transform.FindChild("TotemLinker(Clone)").gameObject);

                            // DIET
                            sunBearSlime.Diet = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink")).Diet;
                            sunBearSlime.Diet.MajorFoodGroups = new SlimeEat.FoodGroup[] { SlimeEat.FoodGroup.FRUIT };
                            sunBearSlime.Diet.MajorFoodIdentifiableTypeGroups = new IdentifiableTypeGroup[] { Get<IdentifiableTypeGroup>("FruitGroup") };
                            sunBearSlime.Diet.ProduceIdents = new IdentifiableType[] { sunBearPlort };
                            sunBearSlime.Diet.AdditionalFoodIdents = new IdentifiableType[]
                            {
                                Get<IdentifiableType>("Chick"),
                                Get<IdentifiableType>("StonyChick"),
                                Get<IdentifiableType>("BriarChick"),
                                Get<IdentifiableType>("SeaChick"),
                                Get<IdentifiableType>("ThunderChick"),
                                Get<IdentifiableType>("PaintedChick"),
                                Get<IdentifiableType>("WildHoneyCraft"),
                                Get<IdentifiableType>("SunSapCraft")
                            };
                            sunBearSlime.Diet.FavoriteIdents = new IdentifiableType[] { Get<IdentifiableType>("WildHoneyCraft") };
                            sunBearSlime.Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, sunBearSlime);

                            sunBearSlime.icon = LocalAssets.iconSlimeSunBearSpr;
                            sunBearSlime.properties = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink").properties);
                            sunBearSlime.defaultPropertyValues = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink")).defaultPropertyValues;

                            // APPEARANCE
                            SlimeAppearance slimeAppearance = UnityEngine.Object.Instantiate(Get<SlimeAppearance>("PinkDefault"));
                            SlimeAppearanceApplicator slimeAppearanceApplicator = sunBearSlime.prefab.GetComponent<SlimeAppearanceApplicator>();
                            slimeAppearance.name = "SunBearDefault";
                            slimeAppearanceApplicator.Appearance = slimeAppearance;
                            slimeAppearanceApplicator.SlimeDefinition = sunBearSlime;

                            // APPEARANCE STRUCTURES
                            // EARS
                            GameObject slimeAppearanceObject = new GameObject("sunbear_ears");
                            slimeAppearanceObject.Prefabitize();
                            slimeAppearanceObject.hideFlags |= HideFlags.HideAndDontSave;

                            slimeAppearanceObject.AddComponent<SkinnedMeshRenderer>().sharedMesh = LocalAssets.sunBearEars;

                            slimeAppearanceObject.AddComponent<SlimeAppearanceObject>().hideFlags |= HideFlags.HideAndDontSave;
                            slimeAppearanceObject.GetComponent<SlimeAppearanceObject>().RootBone = SlimeAppearance.SlimeBone.JIGGLE_TOP;
                            slimeAppearanceObject.GetComponent<SlimeAppearanceObject>().ParentBone = SlimeAppearance.SlimeBone.JIGGLE_BACK;
                            slimeAppearanceObject.GetComponent<SlimeAppearanceObject>().AttachedBones = new SlimeAppearance.SlimeBone[0].AddDefaultBones();
                            slimeAppearanceObject.GetComponent<SlimeAppearanceObject>().IgnoreLODIndex = true;
                            // UnityEngine.Object.DontDestroyOnLoad(slimeAppearanceObject.GetComponent<SlimeAppearanceObject>());

                            slimeAppearance.Structures = slimeAppearance.Structures.ToArray().AddToArray(new SlimeAppearanceStructure(slimeAppearance.Structures[0]));

                            slimeAppearance.Structures[2].Element = ScriptableObject.CreateInstance<SlimeAppearanceElement>();
                            slimeAppearance.Structures[2].Element.name = "SunBearEars";
                            slimeAppearance.Structures[2].Element.Name = "Sun Bear Ears";
                            slimeAppearance.Structures[2].Element.Prefabs = new SlimeAppearanceObject[] { slimeAppearanceObject.GetComponent<SlimeAppearanceObject>() };
                            slimeAppearance.Structures[2].Element.Type = SlimeAppearanceElement.ElementType.EARS;
                            slimeAppearance.Structures[2].SupportsFaces = false;

                            // REST OF APPEARANCE
                            Material slimeMaterial = UnityEngine.Object.Instantiate(slimeAppearance.Structures[0].DefaultMaterials[0]);
                            slimeMaterial.hideFlags |= HideFlags.HideAndDontSave;
                            slimeMaterial.name = "slimeSunBearBase";
                            slimeMaterial.SetColor("_TopColor", sunBearPalette[0]);
                            slimeMaterial.SetColor("_MiddleColor", sunBearPalette[1]);
                            slimeMaterial.SetColor("_BottomColor", sunBearPalette[0]);
                            slimeMaterial.SetColor("_SpecColor", sunBearPalette[1]);

                            slimeMaterial.SetTexture("_ColorMask", LocalAssets.maskSunBearMulticolor);
                            slimeMaterial.SetColor("_RedTopColor", sunBearPalette[0]);
                            slimeMaterial.SetColor("_RedMiddleColor", sunBearPalette[1]);
                            slimeMaterial.SetColor("_RedBottomColor", sunBearPalette[0]);
                            slimeMaterial.SetColor("_GreenTopColor", sunBearPalette[2]);
                            slimeMaterial.SetColor("_GreenMiddleColor", sunBearPalette[3]);
                            slimeMaterial.SetColor("_GreenBottomColor", sunBearPalette[2]);

                            var slimeKeywords = slimeMaterial.GetShaderKeywords().ToList();
                            slimeKeywords.Remove("_BODYCOLORING_DEFAULT");
                            slimeKeywords.Add("_BODYCOLORING_MULTI");
                            slimeMaterial.SetShaderKeywords(slimeKeywords.ToArray());

                            Material earsMaterial = UnityEngine.Object.Instantiate(slimeMaterial);
                            earsMaterial.hideFlags |= HideFlags.HideAndDontSave;
                            earsMaterial.name = "slimeSunBearEarsBase";
                            earsMaterial.SetTexture("_ColorMask", LocalAssets.maskSunBearEarsMulticolor);
                            earsMaterial.SetColor("_RedTopColor", sunBearPalette[0]);
                            earsMaterial.SetColor("_RedMiddleColor", sunBearPalette[1]);
                            earsMaterial.SetColor("_RedBottomColor", sunBearPalette[0]);

                            slimeAppearance.Structures[0].DefaultMaterials[0] = slimeMaterial;
                            slimeAppearance.Structures[2].DefaultMaterials[0] = earsMaterial;

                            slimeAppearance._face = UnityEngine.Object.Instantiate(Get<SlimeAppearance>("TabbyDefault").Face);
                            slimeAppearance.Face.name = "faceSlimeSunBear";

                            SlimeExpressionFace[] expressionFaces = new SlimeExpressionFace[0];
                            foreach (SlimeExpressionFace slimeExpressionFace in slimeAppearance.Face.ExpressionFaces)
                            {
                                Material slimeEyes = null;
                                Material slimeMouth = null;

                                if (slimeExpressionFace.Eyes)
                                    slimeEyes = UnityEngine.Object.Instantiate(slimeExpressionFace.Eyes);
                                if (slimeExpressionFace.Mouth)
                                    slimeMouth = UnityEngine.Object.Instantiate(slimeExpressionFace.Mouth);

                                if (slimeEyes)
                                {
                                    slimeEyes.SetColor("_EyeRed", sunBearPalette[4]);
                                    slimeEyes.SetColor("_EyeGreen", Color.gray);
                                    slimeEyes.SetColor("_EyeBlue", sunBearPalette[4]);
                                }
                                if (slimeMouth)
                                {
                                    slimeMouth.SetColor("_MouthTop", sunBearPalette[1]);
                                    slimeMouth.SetColor("_MouthMid", Color.gray);
                                    slimeMouth.SetColor("_MouthBot", sunBearPalette[1]);
                                }
                                slimeExpressionFace.Eyes = slimeEyes;
                                slimeExpressionFace.Mouth = slimeMouth;
                                expressionFaces = expressionFaces.AddToArray(slimeExpressionFace);
                            }
                            slimeAppearance.Face.ExpressionFaces = expressionFaces;
                            slimeAppearance.Face.OnEnable();

                            slimeAppearance._icon = sunBearSlime.icon;
                            slimeAppearance._splatColor = sunBearPalette[0];
                            slimeAppearance._colorPalette = new SlimeAppearance.Palette
                            {
                                Ammo = sunBearPalette[0],
                                Top = sunBearPalette[0],
                                Middle = sunBearPalette[1],
                                Bottom = sunBearPalette[0]
                            };
                            sunBearSlime.AppearancesDefault = new SlimeAppearance[] { slimeAppearance };
                            sunBearSlime.prefab.hideFlags |= HideFlags.HideAndDontSave;
                            #endregion

                            if (!SRSingleton<GameContext>.Instance.SlimeDefinitions.Slimes.FirstOrDefault(x => x == sunBearSlime))
                                SRSingleton<GameContext>.Instance.SlimeDefinitions.Slimes = SRSingleton<GameContext>.Instance.SlimeDefinitions.Slimes.AddItem(sunBearSlime).ToArray();
                            SRSingleton<GameContext>.Instance.SlimeDefinitions._slimeDefinitionsByIdentifiable.TryAdd(sunBearSlime, sunBearSlime);
                            break;
                        }
                    case "zoneStrand_Area2":
                        {
                            #region SPLIT_TREE_TOP
                            var splitTreeTop = GameObject.Find("zoneStrand_Area2/cellSplitTreeTop/Sector/Slimes").transform;
                            DirectedActorSpawner.SpawnConstraint splitTreeTopBearConstraint = new DirectedActorSpawner.SpawnConstraint()
                            {
                                Weight = 1,
                                Window = new DirectedActorSpawner.TimeWindow() { TimeMode = DirectedActorSpawner.TimeMode.NIGHT },
                                Slimeset = new SlimeSet()
                                {
                                    Members = new SlimeSet.Member[]
                                    {
                                        new SlimeSet.Member()
                                        {
                                            _prefab = sunBearSlime.prefab,
                                            IdentType = sunBearSlime,
                                            Weight = 1
                                        }
                                    }
                                }
                            };

                            DirectedActorSpawner.SpawnConstraint splitTreeTopDayBearConstraint = new DirectedActorSpawner.SpawnConstraint()
                            {
                                Weight = 0.5f,
                                Window = new DirectedActorSpawner.TimeWindow() { TimeMode = DirectedActorSpawner.TimeMode.DAY },
                                Slimeset = new SlimeSet()
                                {
                                    Members = new SlimeSet.Member[]
                                    {
                                        new SlimeSet.Member()
                                        {
                                            _prefab = sunBearSlime.prefab,
                                            IdentType = sunBearSlime,
                                            Weight = 0.05f
                                        }
                                    }
                                }
                            };

                            var treeConstraints = SpawnHelper.AddConstraintToLocation(splitTreeTop, splitTreeTopBearConstraint);
                            SpawnHelper.RemoveFromLocation("Pink", treeConstraints);
                            SpawnHelper.AddConstraintToLocation(splitTreeTop, splitTreeTopDayBearConstraint);
                            #endregion

                            #region WATERFALL_CLIFF
                            var waterfallCliff = GameObject.Find("zoneStrand_Area2/cellWaterfallCliff/Sector/Slimes").transform;
                            DirectedActorSpawner.SpawnConstraint waterfallCliffBearConstraint = new DirectedActorSpawner.SpawnConstraint()
                            {
                                Weight = 0.5f,
                                Window = new DirectedActorSpawner.TimeWindow() { TimeMode = DirectedActorSpawner.TimeMode.NIGHT },
                                Slimeset = new SlimeSet()
                                {
                                    Members = new SlimeSet.Member[]
                                    {
                                        new SlimeSet.Member()
                                        {
                                            _prefab = sunBearSlime.prefab,
                                            IdentType = sunBearSlime,
                                            Weight = 0.05f
                                        }
                                    }
                                }
                            };

                            DirectedActorSpawner.SpawnConstraint waterfallCliffDayBearConstraint = new DirectedActorSpawner.SpawnConstraint()
                            {
                                Weight = 0.1f,
                                Window = new DirectedActorSpawner.TimeWindow() { TimeMode = DirectedActorSpawner.TimeMode.DAY },
                                Slimeset = new SlimeSet()
                                {
                                    Members = new SlimeSet.Member[]
                                    {
                                        new SlimeSet.Member()
                                        {
                                            _prefab = sunBearSlime.prefab,
                                            IdentType = sunBearSlime,
                                            Weight = 0.01f
                                        }
                                    }
                                }
                            };

                            var waterfallConstraints = SpawnHelper.AddConstraintToLocation(waterfallCliff, waterfallCliffBearConstraint);
                            SpawnHelper.RemoveFromLocation("Honey", waterfallConstraints);
                            SpawnHelper.AddConstraintToLocation(waterfallCliff, waterfallCliffDayBearConstraint);
                            #endregion
                            break;
                        }
                    case "zoneStrand_Area3":
                        {
                            #region FLUTTER_CLIFFS
                            var flutterCliffs = GameObject.Find("zoneStrand_Area3/cellFlutterCliffs/Sector/Slimes").transform;
                            DirectedActorSpawner.SpawnConstraint flutterCliffsBearConstraint = new DirectedActorSpawner.SpawnConstraint()
                            {
                                Weight = 0.1f,
                                Window = new DirectedActorSpawner.TimeWindow() { TimeMode = DirectedActorSpawner.TimeMode.NIGHT },
                                Slimeset = new SlimeSet()
                                {
                                    Members = new SlimeSet.Member[]
                                    {
                                        new SlimeSet.Member()
                                        {
                                            _prefab = sunBearSlime.prefab,
                                            IdentType = sunBearSlime,
                                            Weight = 0.01f
                                        }
                                    }
                                }
                            };

                            DirectedActorSpawner.SpawnConstraint flutterCliffsDayBearConstraint = new DirectedActorSpawner.SpawnConstraint()
                            {
                                Weight = 0.05f,
                                Window = new DirectedActorSpawner.TimeWindow() { TimeMode = DirectedActorSpawner.TimeMode.DAY },
                                Slimeset = new SlimeSet()
                                {
                                    Members = new SlimeSet.Member[]
                                    {
                                        new SlimeSet.Member()
                                        {
                                            _prefab = sunBearSlime.prefab,
                                            IdentType = sunBearSlime,
                                            Weight = 0.005f
                                        }
                                    }
                                }
                            };

                            var cliffsConstraints = SpawnHelper.AddConstraintToLocation(flutterCliffs, flutterCliffsBearConstraint);
                            SpawnHelper.RemoveFromLocation("Honey", cliffsConstraints);
                            SpawnHelper.AddConstraintToLocation(flutterCliffs, flutterCliffsDayBearConstraint);
                            #endregion
                            break;
                        }
                }

                /*switch (sceneName.Contains("zoneStrand"))
                {
                    case true:
                        {
                            #region OVERALL_STRAND
                            DirectedActorSpawner.SpawnConstraint bearConstraint = new DirectedActorSpawner.SpawnConstraint()
                            {
                                Weight = 0.005f,
                                Window = new DirectedActorSpawner.TimeWindow() { TimeMode = DirectedActorSpawner.TimeMode.NIGHT },
                                Slimeset = new SlimeSet()
                                {
                                    Members = new SlimeSet.Member[]
                                    {
                                        new SlimeSet.Member()
                                        {
                                            _prefab = sunBearSlime.prefab,
                                            IdentType = sunBearSlime,
                                            Weight = 0.0005f
                                        }
                                    }
                                }
                            };

                            /*DirectedActorSpawner.SpawnConstraint dayBearConstraint = new DirectedActorSpawner.SpawnConstraint()
                            {
                                weight = 0.0005f,
                                window = new DirectedActorSpawner.TimeWindow() { timeMode = DirectedActorSpawner.TimeMode.DAY },
                                slimeset = new SlimeSet()
                                {
                                    members = new SlimeSet.Member[]
                                    {
                                        new SlimeSet.Member()
                                        {
                                            identType = sunBearSlime,
                                            prefab = sunBearSlime.prefab,
                                            weight = 0.00005f
                                        }
                                    }
                                }
                            };*/
                            // Practically impossible, just don't spawn them in the day anywhere else lol

                            /*foreach (DirectedSlimeSpawner nodeSlime in UnityEngine.Object.FindObjectsOfType<DirectedSlimeSpawner>())
                            {
                                if (nodeSlime == null)
                                    continue;

                                if (nodeSlime.gameObject.name.Contains("Puddle") || nodeSlime.gameObject.name.Contains("Gold"))
                                    continue;

                                if (nodeSlime.Constraints.FirstOrDefault(x => x == bearConstraint, null) == null && nodeSlime.Constraints.FirstOrDefault(x => x.Slimeset.Members[0].IdentType == sunBearSlime, null) == null)
                                    nodeSlime.Constraints = nodeSlime.Constraints.ToArray().AddToArray(bearConstraint);
                            }
                            #endregion
                            break;
                        }
                }*/
            }
        }

        internal class Young
        {
            public static void Initialize()
            {
                cubSunBearSlime = ScriptableObject.CreateInstance<SlimeDefinition>();
                cubSunBearSlime.hideFlags |= HideFlags.HideAndDontSave;
                cubSunBearSlime.name = "CubSunBear";
                cubSunBearSlime.Name = cubSunBearSlime.name;

                cubSunBearSlime.CanLargofy = false;
                cubSunBearSlime.color = sunBearPalette[0];
                cubSunBearSlime.localizationSuffix = "cub_sun_bear_slime";
            }

            public static void Load(string sceneName)
            {
                switch (sceneName)
                {
                    case "GameCore":
                        {
                            cubSunBearSlime.localizedName = GeneralizedHelper.CreateTranslation("Actor", "l." + cubSunBearSlime.localizationSuffix, "Cub Sun Bear Slime");

                            #region CUB_SUN_BEAR_SLIME
                            // DEFINITION
                            cubSunBearSlime.BaseModule = Get<GameObject>("moduleSlimeStandard");
                            cubSunBearSlime.BaseSlimes = new SlimeDefinition[0];
                            cubSunBearSlime.FavoriteToyIdents = new ToyDefinition[0];
                            cubSunBearSlime.SlimeModules = new GameObject[] { moduleSlimeSunBear };
                            cubSunBearSlime.Sounds = Get<SlimeSounds>("Standard");
                            cubSunBearSlime.NativeZones = new ZoneDefinition[] { Get<ZoneDefinition>("Luminous Strand") };
                            cubSunBearSlime.showForZones = cubSunBearSlime.NativeZones;

                            // PREFAB
                            cubSunBearSlime.prefab = PrefabUtils.CopyPrefab(sunBearSlime.prefab);
                            cubSunBearSlime.prefab.name = "slimeCubSunBear";
                            cubSunBearSlime.PrefabScale = 0.9f;
                            cubSunBearSlime.prefab.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

                            cubSunBearSlime.prefab.AddComponent<SunBearGrowth>();
                            cubSunBearSlime.prefab.AddComponent<SunBearFollow>();
                            cubSunBearSlime.prefab.AddComponent<SunBearPlayful>();

                            cubSunBearSlime.prefab.AddComponent<AweTowardsLargos>().GetCopyOf(Get<GameObject>("slimePink").GetComponent<AweTowardsLargos>());

                            cubSunBearSlime.prefab.GetComponent<Identifiable>().identType = cubSunBearSlime;
                            cubSunBearSlime.prefab.GetComponent<SlimeEat>().SlimeDefinition = cubSunBearSlime;
                            cubSunBearSlime.prefab.GetComponent<PlayWithToys>().SlimeDefinition = cubSunBearSlime;
                            cubSunBearSlime.prefab.GetComponent<ReactToToyNearby>().SlimeDefinition = cubSunBearSlime;
                            cubSunBearSlime.prefab.GetComponent<Vacuumable>().size = Vacuumable.Size.NORMAL;

                            cubSunBearSlime.prefab.GetComponent<SlimeHealth>().MaxHealth = 20;
                            cubSunBearSlime.prefab.GetComponent<FleeThreats>().FearProfile = Get<FearProfile>("slimeStandardFearProfile");
                            cubSunBearSlime.prefab.GetComponent<SlimeRandomMove>()._maxJump = 3.3f;

                            if (SunBearPreferences.IsRealisticMode())
                            {
                                cubSunBearSlime.prefab.GetComponent<SlimeRandomMove>().ScootSpeedFactor = 1;
                                cubSunBearSlime.prefab.GetComponent<GotoConsumable>().PursuitSpeedFactor = 1;
                            }
                            else
                            {
                                cubSunBearSlime.prefab.GetComponent<SlimeRandomMove>().ScootSpeedFactor = 0.8f;
                                cubSunBearSlime.prefab.GetComponent<GotoConsumable>().PursuitSpeedFactor = 0.8f;
                            }

                            foreach (Il2CppSystem.Type excludedComponent in cubExcludedComponents)
                            {
                                if (excludedComponent == null)
                                    continue;

                                if (cubSunBearSlime.prefab.GetComponent(excludedComponent))
                                    UnityEngine.Object.Destroy(cubSunBearSlime.prefab.GetComponent(excludedComponent));
                            }

                            // UnityEngine.Object.Destroy(cubSunBearSlime.prefab.GetComponent<SlimeFeral>());
                            UnityEngine.Object.Destroy(cubSunBearSlime.prefab.GetComponent<GotoPlayer>());
                            UnityEngine.Object.Destroy(cubSunBearSlime.prefab.GetComponent<AttackPlayer>());
                            UnityEngine.Object.Destroy(cubSunBearSlime.prefab.GetComponent<ColliderTotemLinkerHelper>());
                            UnityEngine.Object.Destroy(cubSunBearSlime.prefab.transform.FindChild("TotemLinker(Clone)").gameObject);
                            UnityEngine.Object.Destroy(cubSunBearSlime.prefab.transform.FindChild("SunBearTriggers(Clone)/SunBearProtectionTrigger").gameObject);

                            // DIET
                            if (SunBearPreferences.IsCasualMode() && SunBearPreferences.IsCasualCubs())
                            {
                                cubSunBearSlime.Diet = UnityEngine.Object.Instantiate(sunBearSlime).Diet;
                                cubSunBearSlime.Diet.ProduceIdents = Array.Empty<IdentifiableType>();
                            }
                            else
                            {
                                cubSunBearSlime.Diet = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink")).Diet;
                                cubSunBearSlime.Diet.MajorFoodGroups = Array.Empty<SlimeEat.FoodGroup>();
                                cubSunBearSlime.Diet.MajorFoodIdentifiableTypeGroups = Array.Empty<IdentifiableTypeGroup>();
                                cubSunBearSlime.Diet.ProduceIdents = Array.Empty<IdentifiableType>();
                                cubSunBearSlime.Diet.AdditionalFoodIdents = Array.Empty<IdentifiableType>();
                                cubSunBearSlime.Diet.FavoriteIdents = Array.Empty<IdentifiableType>();
                            }
                            cubSunBearSlime.Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, cubSunBearSlime);

                            cubSunBearSlime.icon = sunBearSlime.icon;
                            cubSunBearSlime.properties = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink").properties);
                            cubSunBearSlime.defaultPropertyValues = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink")).defaultPropertyValues;

                            cubSunBearSlime.AppearancesDefault = new SlimeAppearance[] { Get<SlimeAppearance>("SunBearDefault") };
                            cubSunBearSlime.prefab.hideFlags |= HideFlags.HideAndDontSave;
                            #endregion

                            SRSingleton<GameContext>.Instance.SlimeDefinitions.Slimes = SRSingleton<GameContext>.Instance.SlimeDefinitions.Slimes.AddItem(cubSunBearSlime).ToArray();
                            SRSingleton<GameContext>.Instance.SlimeDefinitions._slimeDefinitionsByIdentifiable.TryAdd(cubSunBearSlime, cubSunBearSlime);
                            break;
                        }
                }
            }
        }

        internal class Gordo
        {
            public static void Initialize()
            {
                sunBearGordo = ScriptableObject.CreateInstance<IdentifiableType>();
                sunBearGordo.hideFlags |= HideFlags.HideAndDontSave;
                sunBearGordo.name = "SunBearGordo";
                sunBearGordo.color = sunBearPalette[0];
                sunBearGordo.localizationSuffix = "sun_bear_gordo";
            }

            public static void Load(string sceneName)
            {
                switch (sceneName)
                {
                    case "GameCore":
                        {
                            #region SUN_BEAR_GORDO
                            List<GameObject> rewards = new List<GameObject>()
                            {
                                Get<IdentifiableType>("ContainerStrand01").prefab,
                                Get<IdentifiableType>("ContainerStrand01").prefab,
                                Get<IdentifiableType>("WildHoneyCraft").prefab,
                                Get<IdentifiableType>("WildHoneyCraft").prefab,
                                Get<IdentifiableType>("WildHoneyCraft").prefab,
                                Get<IdentifiableType>("SunSapCraft").prefab
                            };

                            for (int i = 0; i < 9; i++)
                                rewards.Add(Randoms.SHARED.Pick(Get<IdentifiableTypeGroup>("FruitGroup").memberTypes).prefab);

                            GordoBuilder.CreateGordo(Get<IdentifiableType>("PinkGordo"), sunBearSlime, sunBearGordo, LocalAssets.iconGordoSunBearSpr, "Sun Bear Gordo", 70, rewards.ToArray());
                            sunBearGordo.prefab.transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);

                            /*sunBearGordo.prefab.GetComponent<GordoFaceComponents>().StrainEyes =
                                sunBearSlime.AppearancesDefault[0].Face.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.ALARM).Eyes;
                            sunBearGordo.prefab.GetComponent<GordoFaceComponents>().StrainMouth =
                                sunBearSlime.AppearancesDefault[0].Face.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.ALARM).Mouth;*/

                            GameObject gordoSunBearEars = new GameObject("gordo_sunbear_ears");
                            gordoSunBearEars.Prefabitize();
                            gordoSunBearEars.AddComponent<SkinnedMeshRenderer>().sharedMesh = LocalAssets.gordoSunBearEars;
                            gordoSunBearEars.GetComponent<SkinnedMeshRenderer>().sharedMaterial = sunBearSlime.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                            gordoSunBearEars.transform.parent = sunBearGordo.prefab.transform.Find("Vibrating/bone_root/bone_slime/bone_core/bone_jiggle_bac/bone_skin_bac").transform;
                            gordoSunBearEars.transform.localScale = Vector3.one;
                            #endregion
                            break;
                        }
                    case "zoneStrand_Area2":
                        {
                            if (!GameObject.Find("zoneStrand_Area2/cellSplitTreeTop/Sector/Slimes/gordoSunBear"))
                                GordoBuilder.PlaceGordo(sunBearGordo, GameObject.Find("zoneStrand_Area2/cellSplitTreeTop/Sector/Slimes").transform, new Vector3(194.8873f, 33.72f, -365.0594f), -100);
                            break;
                        }
                }
            }
        }
    }
}
