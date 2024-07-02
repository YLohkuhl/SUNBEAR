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
    [HarmonyPatch(typeof(LookupDirector), nameof(LookupDirector.Awake))]
    internal static class LookupDirectorAwakePatch
    {
        private static IdentifiableTypeGroup[][] _registryIdentifiableGroups = [];

        public static void Prefix(LookupDirector __instance)
        {
            RegisterPedias();

            LocalInstances.PatchInitialize();
            SunBearLargos.PatchInitialize();

            _registryIdentifiableGroups =
            [
                [
                    Get<IdentifiableTypeGroup>("BaseSlimeGroup"),
                    Get<IdentifiableTypeGroup>("EdibleSlimeGroup"),
                    Get<IdentifiableTypeGroup>("SlimesSinkInShallowWaterGroup"),
                    Get<IdentifiableTypeGroup>("VaccableBaseSlimeGroup"),
                    Get<IdentifiableTypeGroup>("IdentifiableTypesGroup")
                ],
                [
                    Get<IdentifiableTypeGroup>("EdiblePlortFoodGroup"),
                    Get<IdentifiableTypeGroup>("PlortGroup"),
                    Get<IdentifiableTypeGroup>("IdentifiableTypesGroup")
                ]
            ];

            RegisterIdentifiables(__instance);
        }

        private static void RegisterPedias() => Grown.LoadPedia();

        private static void RegisterIdentifiables(LookupDirector director)
        {
            #region SLIMES/PLORTS/GORDO
            foreach (var identifiableTypeGroup in _registryIdentifiableGroups[0])
            {
                AddIdentifiableTypeToGroup(director, sunBearSlime, identifiableTypeGroup);
                if (identifiableTypeGroup.name != "VaccableBaseSlimeGroup")
                    AddIdentifiableTypeToGroup(director, cubSunBearSlime, identifiableTypeGroup);
            }

            foreach (var identifiableTypeGroup in _registryIdentifiableGroups[1])
                AddIdentifiableTypeToGroup(director, sunBearPlort, identifiableTypeGroup);

            if (SunBearPreferences.IsCasualMode() && SunBearPreferences.IsCasualCubs())
                AddIdentifiableTypeToGroup(director, cubSunBearSlime, Get<IdentifiableTypeGroup>("VaccableBaseSlimeGroup"));

            AddIdentifiableTypeToGroup(director, sunBearGordo, Get<IdentifiableTypeGroup>("GordoGroup"));
            #endregion

            #region LARGO
            foreach (var largoDefinition in SunBearLargos.sunBearLargoDefinitions)
            {
                AddIdentifiableTypeToGroup(director, largoDefinition, Get<IdentifiableTypeGroup>(largoDefinition.name.Replace("SunBear", "") + "LargoGroup"));
                LocalInstances.sunBearLargoGroup._memberTypes.Add(largoDefinition);
            }

            AddIdentifiableGroupToGroup(Get<IdentifiableTypeGroup>("SlimesSinkInShallowWaterGroup"), LocalInstances.sunBearLargoGroup);
            AddIdentifiableGroupToGroup(Get<IdentifiableTypeGroup>("EdibleSlimeGroup"), LocalInstances.sunBearLargoGroup);
            AddIdentifiableGroupToGroup(Get<IdentifiableTypeGroup>("LargoGroup"), LocalInstances.sunBearLargoGroup);

            foreach (var largoDefinition in SunBearLargos.sunBearLargoDefinitions)
                AddIdentifiableTypeToGroup(director, largoDefinition, Get<IdentifiableTypeGroup>("IdentifiableTypesGroup"));

            director.RegisterIdentifiableTypeGroup(LocalInstances.sunBearLargoGroup);
            #endregion

            AddIdentifiableTypeToGroup(director, Get<IdentifiableType>("WildHoneyCraft"), Get<IdentifiableTypeGroup>("FoodGroup"));
        }

        public static void AddIdentifiableTypeToGroup(LookupDirector director, IdentifiableType identifiableType, IdentifiableTypeGroup identifiableTypeGroup)
        {
            if (!identifiableTypeGroup._memberTypes.Contains(identifiableType))
                identifiableTypeGroup._memberTypes.Add(identifiableType);
            director.AddIdentifiableTypeToGroup(identifiableType, identifiableTypeGroup);
        }

        public static void AddIdentifiableGroupToGroup(IdentifiableTypeGroup toAddTo, IdentifiableTypeGroup toBeAdded)
        {
            if (!toAddTo._memberGroups.Contains(toBeAdded))
                toAddTo._memberGroups.TryAdd(toBeAdded);
            if (!toAddTo.GetRuntimeObject()._memberGroups.Contains(toBeAdded))
                toAddTo.GetRuntimeObject()._memberGroups.TryAdd(toBeAdded);
        }
    }
}
