using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppMonomiPark.SlimeRancher.World;
using Il2CppSystem.Dynamic.Utils;
using MelonLoader;
using SUNBEAR.Assist;
using SUNBEAR.Components;
using SUNBEAR.Data.Largos;
using SUNBEAR.Data.Slimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Localization.Settings;

namespace SUNBEAR.Data
{
    internal class SunBearLargos
    {
        internal static List<SlimeDefinition> baseSlimeDefinitions = new List<SlimeDefinition>();
        internal static List<SlimeDefinition> sunBearLargoDefinitions = new List<SlimeDefinition>();
        internal static List<SlimeDefinition> feralSunBearLargoDefinitions = new List<SlimeDefinition>();

        public static void ASDInitialize()
        {
            baseSlimeDefinitions = new List<SlimeDefinition>()
            {
                Get<SlimeDefinition>("Pink"),
                Get<SlimeDefinition>("Rock"),
                Get<SlimeDefinition>("Tabby"),
                Get<SlimeDefinition>("Phosphor"),
                Get<SlimeDefinition>("Honey"),
                Get<SlimeDefinition>("Hunter"),
                Get<SlimeDefinition>("Saber"),
                Get<SlimeDefinition>("Boom"),
                Get<SlimeDefinition>("Crystal"),
                Get<SlimeDefinition>("Cotton"),
                Get<SlimeDefinition>("Flutter"),
                Get<SlimeDefinition>("Angler"),
                Get<SlimeDefinition>("Batty"),
                Get<SlimeDefinition>("Ringtail"),
                Get<SlimeDefinition>("Tangle"),
                Get<SlimeDefinition>("Dervish")
            };

            foreach (SlimeDefinition baseSlime in baseSlimeDefinitions)
            {
                var sunBearSlimeLargo = LargoHelper.CreateIdentifiable(SunBear.sunBearSlime, baseSlime, Color.Lerp(SunBear.sunBearPalette[0], baseSlime.AppearancesDefault[0].SplatColor, 0.5f));
                sunBearLargoDefinitions.Add(sunBearSlimeLargo);
            }

            feralSunBearLargoDefinitions.Add(Get<SlimeDefinition>("SunBearHunter"));
        }

        public static void Load(string sceneName)
        {
            Pink.Load(sceneName);
            Rock.Load(sceneName);
            Tabby.Load(sceneName);
            Phosphor.Load(sceneName);
            Honey.Load(sceneName);
            Hunter.Load(sceneName);
            Saber.Load(sceneName);
            Boom.Load(sceneName);
            Crystal.Load(sceneName);
            Cotton.Load(sceneName);
            Flutter.Load(sceneName);
            Angler.Load(sceneName);
            Batty.Load(sceneName);
            Ringtail.Load(sceneName);
            Tangle.Load(sceneName);
            Dervish.Load(sceneName);

            if (sceneName == "GameCore")
            {
                foreach (SlimeDefinition sunBearLargo in sunBearLargoDefinitions)
                {
                    LargoHelper.RegisterDefinitionAndAppearance(sunBearLargo);
                    sunBearLargo.BaseSlimes[1].Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, sunBearLargo.BaseSlimes[1]);
                }
                SunBear.sunBearSlime.Diet.RefreshEatMap(SRSingleton<GameContext>.Instance.SlimeDefinitions, SunBear.sunBearSlime);
            }
        }
    }
}
