global using UnityEngine;
global using static Utility;
global using Il2Cpp;
using MelonLoader;
using SUNBEAR;
using SUNBEAR.Data.Slimes;
using SUNBEAR.Data;
// using SUNBEAR.Components;
// using SUNBEAR.Data.Foods;

[assembly: MelonInfo(typeof(BearEntry), "SUNBEAR", "1.0.0", "FruitsyOG", null)]
[assembly: MelonGame("MonomiPark", "SlimeRancher2")]
[assembly: MelonColor(0, 254, 216, 177)]
namespace SUNBEAR
{
    public class BearEntry : MelonMod
    {

        /*
         *
         * CHECK FOR ANY LARGOS TO CHANGE THE SPLAT COLOR FOR, LIKE RINGTAIL. (COMPLETE)
         * ANALYZE BEHAVIORS IN AWAKE, ETC DUE TO POTENTIAL LAG WHEN SPAWNING, ETC.
         *
         */

        public override void OnInitializeMelon()
        {
            // -- OTHER
            SunBearPreferences.Initialize();

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

        public override void OnPreferencesLoaded()
        {
            SunBearPreferences.EnableModesIfSavage();
            SunBearPreferences.DisableMultipleModes();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            // -- OTHER
            LocalAssets.Load(sceneName);
            LocalInstances.Load(sceneName);

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
                        Get<SlimeDefinition>("Tarr").Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, Get<SlimeDefinition>("Tarr"));
                        break;
                    }
            }
        }
    }
}