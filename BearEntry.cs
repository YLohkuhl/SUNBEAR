global using UnityEngine;
global using static Utility;
global using Il2Cpp;
using MelonLoader;
using SUNBEAR;
using SUNBEAR.Data.Slimes;
using SUNBEAR.Data;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using Il2CppInterop.Runtime;
using SUNBEAR.Components;
using UnityEngine.SceneManagement;
using SUNBEAR.Enums;
using SUNBEAR.Data.Upgrades;
// using SUNBEAR.Components;
// using SUNBEAR.Data.Foods;

[assembly: MelonInfo(typeof(BearEntry), "SUNBEAR", "1.2.0", "YLohkuhl", "https://www.nexusmods.com/slimerancher2/mods/66")]
[assembly: MelonGame("MonomiPark", "SlimeRancher2")]
[assembly: MelonColor(0, 254, 216, 177)]
namespace SUNBEAR
{
    public class BearEntry : MelonMod
    {
        public override void OnInitializeMelon()
        {
            // -- PREFERENCES
            SunBearPreferences.Initialize();

            // -- ENUMS
            LandPlotUpgrade.Initialize();

            // -- SLIMES
            SunBear.Initialize();
            SunBear.Grown.Initialize();
            SunBear.Young.Initialize();
            SunBear.Gordo.Initialize();

            // -- LARGOS
            // SunBearLargos.Initialize();

            // -- FOODS
            // Sunberry.Initialize();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            // -- OTHER
            LocalAssets.Load(sceneName);
            LocalInstances.Load(sceneName);

            // -- UPGRADES
            BearFriendly.Load(sceneName);

            // -- SLIMES
            SunBear.Grown.Load(sceneName);
            SunBear.Young.Load(sceneName);
            SunBear.Gordo.Load(sceneName);

            // -- LARGOS
            SunBearLargos.Load(sceneName);

            // -- FOODS
            // Sunberry.Load(sceneName);

            switch (sceneName)
            {
                case "GameCore":
                    {
                        Get<IdentifiablePediaEntry>("WildHoneyCraft")._highlightSet = Get<PediaHighlightSet>("FoodHightlights");
                        Get<SlimeDefinition>("Tarr").Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, Get<SlimeDefinition>("Tarr"));
                        // Get<ScriptedValueRangeOptionDefinition>("GameIcon")._maxValue = Get<GameIconDefinitionCollection>("GameIconCollection").Count - 1;
                        break;
                    }
                case "zoneCore":
                    {
                        if (!GameObject.Find("SunBearGlobalStatistics"))
                        {
                            var globalStatistics = new GameObject("SunBearGlobalStatistics", Il2CppType.Of<SunBearGlobalStatistics>());
                            SceneManager.MoveGameObjectToScene(globalStatistics, SceneManager.GetSceneByName("zoneCore"));
                        }
                        break;
                    } 
            }
        }
    }
}