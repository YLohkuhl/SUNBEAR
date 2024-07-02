using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppInterop.Runtime;
using SUNBEAR.Data.Slimes;
using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.World;
using MelonLoader;
using Il2CppMonomiPark.SlimeRancher;

namespace SUNBEAR.Assist
{
    internal class LargoHelper
    {
        public static SlimeDefinition CreateIdentifiable(SlimeDefinition primaryDefinition, SlimeDefinition secondaryDefinition, Color identifiableColor)
        {
            SlimeDefinition largoDefinition = ScriptableObject.CreateInstance<SlimeDefinition>();

            largoDefinition.hideFlags |= HideFlags.HideAndDontSave;
            largoDefinition.name = primaryDefinition.name + secondaryDefinition.name;
            largoDefinition.Name = largoDefinition.name;
            largoDefinition.IsLargo = true;
            largoDefinition.CanLargofy = false;
            largoDefinition.color = identifiableColor;
            largoDefinition._pediaPersistenceSuffix = 
                primaryDefinition._pediaPersistenceSuffix.Replace("_slime", "_") + secondaryDefinition._pediaPersistenceSuffix.Replace("_slime", "_largo");
            largoDefinition.BaseSlimes = new SlimeDefinition[] { primaryDefinition, secondaryDefinition };

            return largoDefinition;
        }

        public static SlimeDiet.EatMapEntry CreateTarrEatMap(IdentifiableType identifiableType)
        {
            SlimeDiet.EatMapEntry plortEntry =
                new SlimeDiet.EatMapEntry()
                {
                    Driver = SlimeEmotions.Emotion.AGITATION,
                    EatsIdent = identifiableType,
                    BecomesIdent = Get<SlimeDefinition>("Tarr"),
                    MinDrive = 0.5f
                };

            return plortEntry;
        }

        /*public static void ApplyBaseEatMaps(SlimeDefinition largoDefinition)
        {
            SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
            SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];

            var primaryEntry =
                new SlimeDiet.EatMapEntry()
                {
                    driver = SlimeEmotions.Emotion.AGITATION,
                    eatsIdent = secondaryDef.Diet.ProduceIdents[0],
                    becomesIdent = largoDefinition,
                    minDrive = 0.5f
                };

            var secondaryEntry =
                new SlimeDiet.EatMapEntry()
                {
                    driver = SlimeEmotions.Emotion.AGITATION,
                    eatsIdent = primaryDef.Diet.ProduceIdents[0],
                    becomesIdent = largoDefinition,
                    minDrive = 0.5f
                };

            if (!primaryDef.Diet.EatMap.Contains(primaryEntry))
                primaryDef.Diet.EatMap.Add(primaryEntry);

            if (!secondaryDef.Diet.EatMap.Contains(secondaryEntry))
                secondaryDef.Diet.EatMap.Add(secondaryEntry);

            primaryDef.Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, primaryDef);
            secondaryDef.Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, secondaryDef);
        }*/

        public static void RegisterDefinitionAndAppearance(SlimeDefinition largoDefinition)
        {
            SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
            SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
            SlimeDefinitions slimeDefinitions = SRSingleton<GameContext>.Instance.SlimeDefinitions;
            SlimeAppearanceDirector slimeAppearanceDirector = Get<SlimeAppearanceDirector>("MainSlimeAppearanceDirector");

            slimeDefinitions.Slimes = slimeDefinitions.Slimes.ToArray().TryAdd(largoDefinition);
            slimeDefinitions._slimeDefinitionsByIdentifiable.TryAdd(largoDefinition, largoDefinition);

            slimeDefinitions._largoDefinitionByBaseDefinitions.TryAdd(new SlimeDefinitions.SlimeDefinitionPair()
            {
                SlimeDefinition1 = primaryDef,
                SlimeDefinition2 = secondaryDef
            }, largoDefinition);

            slimeDefinitions._largoDefinitionByBasePlorts.TryAdd(new SlimeDefinitions.PlortPair()
            {
                Plort1 = primaryDef.Diet.ProduceIdents[0],
                Plort2 = secondaryDef.Diet.ProduceIdents[0]
            }, largoDefinition);

            slimeAppearanceDirector.RegisterDependentAppearances(largoDefinition, largoDefinition.AppearancesDefault[0]);
            slimeAppearanceDirector.UpdateChosenSlimeAppearance(largoDefinition, largoDefinition.AppearancesDefault[0]);
        }

        public static void PresetLargo(string largoName, SlimeDefinition largoDefinition)
        {
            // THIS IS ONLY MADE FOR SUN BEAR LARGOS \\
            SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
            SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
            largoDefinition.localizedName = GeneralizedHelper.CreateTranslation("Actor", largoDefinition._pediaPersistenceSuffix, largoName);

            #region SUN_BEAR_LARGO
            // DEFINITION
            largoDefinition.BaseModule = Get<GameObject>("moduleSlimeLargo");
            largoDefinition.SlimeModules = new GameObject[0];
            largoDefinition.SlimeModules = primaryDef.SlimeModules.Union(secondaryDef.SlimeModules).ToArray();
            largoDefinition.FavoriteToyIdents = new ToyDefinition[0];
            largoDefinition.FavoriteToyIdents = primaryDef.FavoriteToyIdents.Union(secondaryDef.FavoriteToyIdents).ToArray();

            largoDefinition.Sounds = Get<SlimeSounds>("Largo");
            largoDefinition.NativeZones = new ZoneDefinition[0];
            largoDefinition.showForZones = largoDefinition.NativeZones;

            // PREFAB
            largoDefinition.prefab = PrefabUtils.CopyPrefab(primaryDef.prefab);
            largoDefinition.prefab.name = "slime" + largoDefinition.name;
            largoDefinition.PrefabScale = primaryDef.PrefabScale + secondaryDef.PrefabScale;
            largoDefinition.prefab.transform.localScale = new Vector3(largoDefinition.PrefabScale, largoDefinition.PrefabScale, largoDefinition.PrefabScale);

            largoDefinition.prefab.AddComponent<FeralSlimeButtstomp>().GetCopyOf(Get<GameObject>("slimeHunterCrystal").GetComponent<FeralSlimeButtstomp>());
            largoDefinition.prefab.GetComponent<FeralSlimeButtstomp>().MinPlayerDamage += 10;
            largoDefinition.prefab.GetComponent<FeralSlimeButtstomp>().MaxPlayerDamage += 10;

            largoDefinition.prefab.GetComponent<Identifiable>().identType = largoDefinition;
            largoDefinition.prefab.GetComponent<SlimeEat>().SlimeDefinition = largoDefinition;
            largoDefinition.prefab.GetComponent<PlayWithToys>().SlimeDefinition = largoDefinition;
            largoDefinition.prefab.GetComponent<ReactToToyNearby>().SlimeDefinition = largoDefinition;
            largoDefinition.prefab.GetComponent<SlimeVarietyModules>().BaseModule = largoDefinition.BaseModule;
            largoDefinition.prefab.GetComponent<SlimeVarietyModules>().SlimeModules = largoDefinition.SlimeModules;
            largoDefinition.prefab.GetComponent<Vacuumable>().size = VacuumableSize.LARGE;

            largoDefinition.prefab.GetComponent<SlimeAudio>().SlimeSounds = largoDefinition.Sounds;
            largoDefinition.prefab.GetComponent<SlimeHealth>().MaxHealth = primaryDef.prefab.GetComponent<SlimeHealth>().MaxHealth + secondaryDef.prefab.GetComponent<SlimeHealth>().MaxHealth;
            largoDefinition.prefab.GetComponent<FleeThreats>().FearProfile = Get<FearProfile>("slimeStandardFearProfile");

            foreach (Component component in secondaryDef.prefab.GetComponents(Il2CppType.Of<Component>()))
            {
                if (!largoDefinition.prefab.GetComponent(component.GetIl2CppType()))
                    largoDefinition.prefab.AddComponent(component.GetIl2CppType()).GetCopyOf(component);
            }

            foreach (Il2CppSystem.Type excludedComponent in SunBear.largoExcludedComponents)
            {
                if (excludedComponent == null)
                    continue;

                if (largoDefinition.prefab.GetComponent(excludedComponent))
                    UnityEngine.Object.Destroy(largoDefinition.prefab.GetComponent(excludedComponent));
            }

            foreach (Il2CppSystem.Object transform in secondaryDef.prefab.transform)
            {
                if (transform.GetIl2CppType() != Il2CppType.Of<Transform>())
                    continue;
                Transform t = transform.TryCast<Transform>();
                if (t != null && largoDefinition.prefab.transform.Find(t.name) == null)
                    UnityEngine.Object.Instantiate(Get<GameObject>(t.name.Replace("(Clone)", "")), largoDefinition.prefab.transform);
            }

            // UnityEngine.Object.Destroy(largoDefinition.prefab.GetComponent<SlimeFeral>());
            if (largoDefinition.prefab.GetComponent<AweTowardsLargos>())
                UnityEngine.Object.Destroy(largoDefinition.prefab.GetComponent<AweTowardsLargos>());
            UnityEngine.Object.Destroy(largoDefinition.prefab.GetComponent<ColliderTotemLinkerHelper>());
            UnityEngine.Object.Destroy(largoDefinition.prefab.transform.FindChild("TotemLinker(Clone)").gameObject);
            UnityEngine.Object.Destroy(largoDefinition.prefab.transform.FindChild("SunBearTriggers(Clone)/SunBearProtectionTrigger").gameObject);

            // DIET
            largoDefinition.Diet = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink")).Diet;
            largoDefinition.Diet.MajorFoodGroups = primaryDef.Diet.MajorFoodGroups.Union(secondaryDef.Diet.MajorFoodGroups).ToArray();
            largoDefinition.Diet.MajorFoodIdentifiableTypeGroups = primaryDef.Diet.MajorFoodIdentifiableTypeGroups.Union(secondaryDef.Diet.MajorFoodIdentifiableTypeGroups).ToArray();
            largoDefinition.Diet.ProduceIdents = primaryDef.Diet.ProduceIdents.Concat(secondaryDef.Diet.ProduceIdents).ToArray();
            largoDefinition.Diet.AdditionalFoodIdents = primaryDef.Diet.AdditionalFoodIdents.Union(secondaryDef.Diet.AdditionalFoodIdents).ToArray();
            largoDefinition.Diet.FavoriteIdents = primaryDef.Diet.FavoriteIdents.Union(secondaryDef.Diet.FavoriteIdents).ToArray();

            List<IdentifiableTypeGroup> identifiableTypeGroups = largoDefinition.Diet.MajorFoodIdentifiableTypeGroups.ToList();
            List<IdentifiableType> additionalFoods = largoDefinition.Diet.AdditionalFoodIdents.ToList();
            List<IdentifiableType> favorites = largoDefinition.Diet.FavoriteIdents.ToList();

            foreach (var majorIdentifiableTypeGroup in largoDefinition.Diet.MajorFoodIdentifiableTypeGroups)
            {
                if (largoDefinition.Diet.MajorFoodIdentifiableTypeGroups.Count(x => x.name == majorIdentifiableTypeGroup.name) > 1)
                {
                    identifiableTypeGroups.Remove(identifiableTypeGroups.FirstOrDefault(x => x.name == majorIdentifiableTypeGroup.name));
                    largoDefinition.Diet.MajorFoodIdentifiableTypeGroups = identifiableTypeGroups.ToArray();
                }
            }

            foreach (var additionalFood in largoDefinition.Diet.AdditionalFoodIdents)
            {
                if (largoDefinition.Diet.AdditionalFoodIdents.Count(x => x.name == additionalFood.name) > 1)
                {
                    additionalFoods.Remove(additionalFoods.FirstOrDefault(x => x.name == additionalFood.name));
                    largoDefinition.Diet.AdditionalFoodIdents = additionalFoods.ToArray();
                }
            }

            foreach (var favorite in largoDefinition.Diet.FavoriteIdents)
            {
                if (largoDefinition.Diet.FavoriteIdents.Count(x => x.name == favorite.name) > 1)
                {
                    additionalFoods.Remove(additionalFoods.FirstOrDefault(x => x.name == favorite.name));
                    largoDefinition.Diet.FavoriteIdents = favorites.ToArray();
                }
            }

            largoDefinition.Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, largoDefinition);

            largoDefinition.properties = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink").properties);
            largoDefinition.defaultPropertyValues = UnityEngine.Object.Instantiate(Get<SlimeDefinition>("Pink")).defaultPropertyValues;
            #endregion SUN_BEAR_LARGO
        }
    }
}
