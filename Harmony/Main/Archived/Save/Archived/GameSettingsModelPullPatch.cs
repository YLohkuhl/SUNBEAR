using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using MelonLoader;
using SUNBEAR.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SUNBEAR.Assist.GeneralizedHelper;

namespace SUNBEAR.Harmony
{
    /*[HarmonyPatch]
    internal class GameSettingsModelPushPatch
    {
        public static MethodInfo TargetMethod() => typeof(GameSettingsModel).GetMethods().First(x => x.Name == "Push" && x.GetParameters().Length == 2);

        public static void Prefix(ref PersistenceIdReverseLookupTable<GameIconDefinition> persistenceIdToGameIconDefinition)
        {
            foreach (var gameIcon in gameIconsToPatch)
            {
                if (gameIcon)
                    persistenceIdToGameIconDefinition._indexTable = persistenceIdToGameIconDefinition._indexTable.ToArray().TryAdd(gameIcon.persistenceId);
            }
            MelonLogger.Msg("GAME SETTINGS PUSH PATCH");
        }
    }

    [HarmonyPatch]
    internal class GameSettingsModelPullPatch
    {
        public static MethodInfo TargetMethod() => typeof(GameSettingsModel).GetMethods().First(x => x.Name == "Pull" && x.GetParameters().Length == 2);

        public static void Prefix(ref PersistenceIdLookupTable<GameIconDefinition> persistenceLookup)
        {
            var iconTranslation = SRSingleton<GameContext>.Instance.AutoSaveDirector.SavedGame._gameIconTranslation;
            foreach (var gameIcon in gameIconsToPatch)
            {
                if (gameIcon)
                {
                    /*iconTranslation.RawLookupDictionary.TryAdd(gameIcon.persistenceId, gameIcon);
                    if (iconTranslation.ReverseLookupTable.IsNotNull())
                        iconTranslation.ReverseLookupTable._indexTable = iconTranslation.ReverseLookupTable._indexTable.ToArray().TryAdd(gameIcon.persistenceId);

                    persistenceLookup._primaryIndex = persistenceLookup._primaryIndex.ToArray().TryAdd(gameIcon.persistenceId);
                    persistenceLookup._reverseIndex.TryAdd(gameIcon.persistenceId, persistenceLookup._primaryIndex.Count - 1);
                }
            }
            MelonLogger.Msg("GAME SETTINGS PULL PATCH");
        }
    }*/
}
