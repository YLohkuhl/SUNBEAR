using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using static SUNBEAR.Assist.GeneralizedHelper;

namespace SUNBEAR.Harmony
{
    [HarmonyPatch(typeof(SavedGame), nameof(SavedGame.Push), typeof(GameModel))]
    internal static class SavedGamePushPatch
    {
        public static void Prefix(SavedGame __instance)
        {
            foreach (var pediaEntry in pediasToPatch)
                if (pediaEntry)
                    __instance.pediaEntryLookup.TryAdd(pediaEntry.PersistenceId, pediaEntry);

            /*var iconTranslation = __instance._gameIconTranslation;
            foreach (var gameIcon in gameIconsToPatch)
            {
                if (gameIcon)
                {
                    iconTranslation.RawLookupDictionary.TryAdd(gameIcon.persistenceId, gameIcon);
                    iconTranslation.ReverseLookupTable._indexTable = iconTranslation.ReverseLookupTable._indexTable.ToArray().TryAdd(gameIcon.persistenceId);

                    iconTranslation.InstanceLookupTable._primaryIndex = __instance._gameIconTranslation.InstanceLookupTable._primaryIndex.ToArray().TryAdd(gameIcon.persistenceId);
                    iconTranslation.InstanceLookupTable._reverseIndex.TryAdd(gameIcon.persistenceId, __instance._gameIconTranslation.InstanceLookupTable._reverseIndex.Count - 1);
                }
            }*/
        }
    }
}
