using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Script.Util.Extensions;
using Il2CppInterop.Runtime;
using Il2CppMonomiPark.SlimeRancher;
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
            // so I don't freaking forget how to visualize this in-game
            // it scales based on transform which I guess is initially expected due to the default value of the radius being 0.5
            // but changing this will of course cause it to scale more but the default transform scale is 1 (1,1,1). Change it to 0.5 (0.5,0.5,0.5) to make it more accurate.
            // It's sort of like reversing their positions in a way. For visualizing in-game, basically reverse it again and add MeshFilter, MeshRenderer, etc in order to visualize the radius you want.
            // Boom, that works please look at in future when needed thank youuuuu
            // (and no I don't know the math that it does in the background for this but whatever it works)

            // maybe ill change how this works a bit

            // uhhhh big code update when

            GameObject cubTrigger = new GameObject("CubTrigger");
            cubTrigger.transform.parent = sunBearCacheTrigger.transform;
            cubTrigger.transform.localScale *= 0.5f;
            cubTrigger.AddComponent<SphereCollider>().radius = 10;
            cubTrigger.AddComponent<SunBearCacheTrigger>();
            cubTrigger.GetComponent<SphereCollider>().isTrigger = true;

            // --- BEAR TRIGGER
            GameObject bearTrigger = new GameObject("BearTrigger");
            bearTrigger.transform.parent = sunBearCacheTrigger.transform;
            bearTrigger.transform.localScale *= 0.5f;
            bearTrigger.AddComponent<SphereCollider>().radius = 15;
            bearTrigger.AddComponent<SunBearCacheTrigger>();
            bearTrigger.GetComponent<SphereCollider>().isTrigger = true;

            // --- HIVE TRIGGER
            GameObject hiveTrigger = new GameObject("HiveTrigger");
            hiveTrigger.transform.parent = sunBearCacheTrigger.transform;
            hiveTrigger.transform.localScale *= 0.5f;
            hiveTrigger.AddComponent<SphereCollider>().radius = 18;
            hiveTrigger.AddComponent<SunBearCacheTrigger>();
            hiveTrigger.GetComponent<SphereCollider>().isTrigger = true;

            // --- SLIME LARGO TRIGGER
            GameObject slimeLargoTrigger = new GameObject("SlimeLargoTrigger");
            slimeLargoTrigger.transform.parent = sunBearCacheTrigger.transform;
            slimeLargoTrigger.transform.localScale *= 0.5f;
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
                sunBearSlime._pediaPersistenceSuffix = "sun_bear_slime";

                sunBearPlort = ScriptableObject.CreateInstance<IdentifiableType>();
                sunBearPlort.hideFlags |= HideFlags.HideAndDontSave;
                sunBearPlort.name = "SunBearPlort";

                sunBearPlort.IsPlort = true;
                sunBearPlort.color = sunBearPalette[1];
                sunBearPlort._pediaPersistenceSuffix = "sun_bear_plort";
            }

            public static void LoadPedia()
            {
                sunBearSlime.localizedName = GeneralizedHelper.CreateTranslation("Actor", "l." + sunBearSlime._pediaPersistenceSuffix, "Sun Bear Slime");
                sunBearPlort.localizedName = GeneralizedHelper.CreateTranslation("Actor", "l." + sunBearPlort._pediaPersistenceSuffix, "Sun Bear Plort");

                PediaEntry sunBearEntry = PediaHelper.CreateIdentifiableEntry(sunBearSlime, Get<PediaEntry>("Pink")._highlightSet,
                    GeneralizedHelper.CreateTranslation(
                        "Pedia",
                        $"m.intro.{sunBearSlime._pediaPersistenceSuffix}",
                        "While not the most popular, the unique patch on their fur will remind you of who they are!"
                    ),
                    []
                );
                
                // SLIMEOLOGY
                PediaHelper.AddSectionToPedia(sunBearEntry, Get<PediaDetailSection>("Slimeology"),
                   GeneralizedHelper.CreateTranslation("PediaPage", $"m.slimeology.{sunBearSlime._pediaPersistenceSuffix}",
                        "The Sun Bear slime, distinct for its fur patch and texture, is a unique and speedy species. " +
                        "They are defensive, preferring isolation, especially when cubs are present. " +
                        "Reproduction is rare but only requires one bear, taking 3-7 days for 1-2 cubs, which can grow aggressive once fully grown. " +
                        "Once Sun Bears become largos, they lose the ability to reproduce. " +
                        "Cubs need a caretaker Sun Bear for proper care and feeding, making it impossible to take them home. " +
                        "Sun Bears harvest Wild Honey, and hungry ones may turn savage, requiring quick feeding. " +
                        "Cubs are playful and grow into ranchable bears after 3-5 days. " +
                        "Key facts include their resistance to vacpack in attack mode, increased attack chances based on hunger, agitation, or cub presence. " +
                        "Largos, more domesticated post-merging, can go savage, adding complexity to ranching."
                   )
                );

                // RISKS
                PediaHelper.AddSectionToPedia(sunBearEntry, Get<PediaDetailSection>("Rancher Risks"),
                    GeneralizedHelper.CreateTranslation("PediaPage", $"m.risks.{sunBearSlime._pediaPersistenceSuffix}",
                        "Due to the Sun Bear slimes preferring isolation, they may perceive you as a threat simply for approaching them. " +
                        "In such situations, it's best to run, as they will eventually cease pursuit once you exit their proximity—considerably more amiable than other bears. " +
                        "When they turn savage, you can deter them by splashing water or offering food. " +
                        "This is considerably more dangerous than a feral state, as they become fixated solely on one thing: their target. " +
                        "This could be considered the same for their usual attacking."
                    )
                );

                // PLORTONOMICS
                PediaHelper.AddSectionToPedia(sunBearEntry, Get<PediaDetailSection>("Plortonomics"),
                    GeneralizedHelper.CreateTranslation("PediaPage", $"m.plortonomics.{sunBearSlime._pediaPersistenceSuffix}",
                        "Sun Bear plorts were long shrouded in mystery, with unconfirmed claims on slime forums about their potential effects. " +
                        "Recently, scientists revealed confidential research indicating that these plorts show extraordinary abilities when near garden crops. " +
                        "However, the catch is that this ability can only be utilized every 12 hours in a ranching setting."
                    )
                    /*"Sun Bear plorts are a rather mysterious type of plort. " +
                    "On slime forums, individuals have claimed that these plorts can enhance food growth and reduce decay. " +
                    "Scientists have yet to validate these claims and are keeping their research on the plort confidential. " +
                    "It is hoped that more information about the plort will be made available soon.", false, true*/
                );

                PediaHelper.AddPediaToCategory(sunBearEntry, Get<PediaCategory>("Slimes"));
            }

            public static void Load(string sceneName)
            {
                switch (sceneName)
                {
                    case "GameCore":
                        {
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
                            foreach (IdentifiableTypeGroup largoGroup in Get<IdentifiableTypeGroup>("LargoGroup")._memberGroups)
                            {
                                foreach (IdentifiableType largoType in largoGroup._memberTypes)
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
                            sunBearSlime.prefab.GetComponent<Vacuumable>().size = VacuumableSize.LARGE;

                            sunBearSlime.prefab.GetComponent<SlimeHealth>().MaxHealth = 40;
                            sunBearSlime.prefab.GetComponent<FleeThreats>().FearProfile = LocalInstances.sunBearSlimeFearProfile;
                            sunBearSlime.prefab.GetComponent<SlimeRandomMove>()._maxJump = 4;

                            if (SunBearPreferences.IsRealisticMode.Value)
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

                            if (SunBearPreferences.IsCasualMode.Value)
                                UnityEngine.Object.Destroy(sunBearSlime.prefab.transform.Find("SunBearTriggers(Clone)/SunBearProtectionTrigger").gameObject);

                            foreach (Il2CppSystem.Type excludedComponent in grownExcludedComponents)
                            {
                                if (excludedComponent == null)
                                    continue;

                                if (sunBearSlime.prefab.GetComponent(excludedComponent))
                                    UnityEngine.Object.Destroy(sunBearSlime.prefab.GetComponent(excludedComponent));
                            }

                            if (SunBearPreferences.IsCasualMode.Value)
                            {
                                foreach (Il2CppSystem.Type excludedComponent in casualExcludedComponents)
                                {
                                    if (excludedComponent == null)
                                        continue;

                                    if (SunBearPreferences.IsCasualSavage.Value && excludedComponent == Il2CppType.Of<SunBearSavage>())
                                        continue;

                                    if (sunBearSlime.prefab.GetComponent(excludedComponent))
                                        UnityEngine.Object.Destroy(sunBearSlime.prefab.GetComponent(excludedComponent));
                                }
                            }

                            if (SunBearPreferences.IsRealisticMode.Value && SunBearPreferences.IsRealisticNoSavage.Value)
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
                cubSunBearSlime._pediaPersistenceSuffix = "cub_sun_bear_slime";
            }

            public static void Load(string sceneName)
            {
                switch (sceneName)
                {
                    case "GameCore":
                        {
                            cubSunBearSlime.localizedName = GeneralizedHelper.CreateTranslation("Actor", "l." + cubSunBearSlime._pediaPersistenceSuffix, "Cub Sun Bear Slime");

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
                            cubSunBearSlime.prefab.GetComponent<Vacuumable>().size = VacuumableSize.NORMAL;

                            cubSunBearSlime.prefab.GetComponent<SlimeHealth>().MaxHealth = 20;
                            cubSunBearSlime.prefab.GetComponent<FleeThreats>().FearProfile = Get<FearProfile>("slimeStandardFearProfile");
                            cubSunBearSlime.prefab.GetComponent<SlimeRandomMove>()._maxJump = 3.3f;

                            if (SunBearPreferences.IsRealisticMode.Value)
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
                            if (SunBearPreferences.IsCasualMode.Value && SunBearPreferences.IsCasualCubs.Value)
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
                sunBearGordo._pediaPersistenceSuffix = "sun_bear_gordo";
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
                                Get<IdentifiableType>("ContainerStrand01").prefab,
                                Get<IdentifiableType>("WildHoneyCraft").prefab,
                                Get<IdentifiableType>("WildHoneyCraft").prefab,
                                Get<IdentifiableType>("WildHoneyCraft").prefab
                            };

                            for (int i = 0; i < 9; i++)
                                rewards.Add(Randoms.SHARED.Pick(Get<IdentifiableTypeGroup>("FruitGroup")._memberTypes).prefab);

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
