using HarmonyLib;
using SUNBEAR.Components;
using SUNBEAR.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Data.Slimes.SunBear;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.Awake))]
    internal static class AutoSaveDirectorAwakePatch
    {
        public static void Prefix(AutoSaveDirector __instance)
        {
            LocalInstances.ASDInitialize();
            SunBearLargos.ASDInitialize();

            Get<IdentifiableTypeGroup>("BaseSlimeGroup").memberTypes.Add(sunBearSlime);
            Get<IdentifiableTypeGroup>("EdibleSlimeGroup").memberTypes.Add(sunBearSlime);
            Get<IdentifiableTypeGroup>("SlimesSinkInShallowWaterGroup").memberTypes.Add(sunBearSlime);
            Get<IdentifiableTypeGroup>("VaccableBaseSlimeGroup").memberTypes.Add(sunBearSlime);

            Get<IdentifiableTypeGroup>("EdiblePlortFoodGroup").memberTypes.Add(sunBearPlort);
            Get<IdentifiableTypeGroup>("PlortGroup").memberTypes.Add(sunBearPlort);

            Get<IdentifiableTypeGroup>("BaseSlimeGroup").memberTypes.Add(cubSunBearSlime);
            Get<IdentifiableTypeGroup>("EdibleSlimeGroup").memberTypes.Add(cubSunBearSlime);
            Get<IdentifiableTypeGroup>("SlimesSinkInShallowWaterGroup").memberTypes.Add(cubSunBearSlime);
            if (SunBearPreferences.IsCasualMode() && SunBearPreferences.IsCasualCubs())
                Get<IdentifiableTypeGroup>("VaccableBaseSlimeGroup").memberTypes.Add(cubSunBearSlime);

            Get<IdentifiableTypeGroup>("GordoGroup").memberTypes.Add(sunBearGordo);

            foreach (SlimeDefinition largoDefinition in SunBearLargos.sunBearLargoDefinitions)
            {
                Get<IdentifiableTypeGroup>(largoDefinition.name.Replace("SunBear", "") + "LargoGroup").memberTypes.Add(largoDefinition);
                LocalInstances.sunBearLargoGroup.memberTypes.Add(largoDefinition);
            }

            Get<IdentifiableTypeGroup>("SlimesSinkInShallowWaterGroup").memberGroups.Add(LocalInstances.sunBearLargoGroup);
            Get<IdentifiableTypeGroup>("EdibleSlimeGroup").memberGroups.Add(LocalInstances.sunBearLargoGroup);
            Get<IdentifiableTypeGroup>("LargoGroup").memberGroups.Add(LocalInstances.sunBearLargoGroup);

            Get<IdentifiableTypeGroup>("FoodGroup").memberTypes.Add(Get<IdentifiableType>("WildHoneyCraft"));

            __instance.identifiableTypes.memberTypes.Add(sunBearSlime);
            __instance.identifiableTypes.memberTypes.Add(cubSunBearSlime);
            __instance.identifiableTypes.memberTypes.Add(sunBearPlort);

            foreach (SlimeDefinition largoDefinition in SunBearLargos.sunBearLargoDefinitions)
                __instance.identifiableTypes.memberTypes.Add(largoDefinition);
        }
    }
}
