using HarmonyLib;
using SUNBEAR.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SUNBEAR.Data.Largos
{
    internal class Batty
    {
        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        SlimeDefinition largoDefinition = Get<SlimeDefinition>("SunBearBatty");
                        SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
                        SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
                        string largoName = "Sun Bear Batty Largo";

                        LargoHelper.PresetLargo(largoName, largoDefinition);
                        largoDefinition.Sounds = Get<SlimeSounds>("BattyLargo");
                        largoDefinition.prefab.GetComponent<SlimeAudio>().SlimeSounds = largoDefinition.Sounds;

                        #region SUN_BEAR_BATTY_LARGO
                        // APPEARANCE
                        SlimeAppearance largoAppearance = UnityEngine.Object.Instantiate(primaryDef.AppearancesDefault[0]);
                        SlimeAppearanceApplicator slimeAppearanceApplicator = largoDefinition.prefab.GetComponent<SlimeAppearanceApplicator>();
                        largoAppearance.name = primaryDef.AppearancesDefault[0].name + secondaryDef.AppearancesDefault[0].name;
                        largoAppearance._dependentAppearances = new SlimeAppearance[] { primaryDef.AppearancesDefault[0], secondaryDef.AppearancesDefault[0] };
                        slimeAppearanceApplicator.Appearance = largoAppearance;
                        slimeAppearanceApplicator.SlimeDefinition = largoDefinition;

                        // REST OF APPEARANCE
                        foreach (SlimeAppearanceStructure secondaryStruct in secondaryDef.AppearancesDefault[0].Structures)
                        {
                            if (secondaryStruct.SupportsFaces || secondaryStruct.Element.Type == SlimeAppearanceElement.ElementType.FACE || secondaryStruct.Element.Name.Contains("Face", StringComparison.OrdinalIgnoreCase))
                                continue;

                            if (secondaryStruct.Element.Type == SlimeAppearanceElement.ElementType.BODY || secondaryStruct.Element.Name.Contains("Body", StringComparison.OrdinalIgnoreCase))
                                continue;

                            if (secondaryStruct.Element.Type == SlimeAppearanceElement.ElementType.EARS || secondaryStruct.Element.Name.Contains("Ears", StringComparison.OrdinalIgnoreCase))
                                continue;

                            largoAppearance.Structures = largoAppearance.Structures.ToArray().AddToArray(new SlimeAppearanceStructure(secondaryStruct));
                        }
                        largoAppearance._face = secondaryDef.AppearancesDefault[0].Face;

                        Material primaryMat = primaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];

                        Material largoMaterial = UnityEngine.Object.Instantiate(primaryMat);
                        largoMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        largoMaterial.name = largoAppearance.name + "_Body";

                        largoAppearance.Structures[0].DefaultMaterials[0] = largoMaterial;
                        largoAppearance.Structures.TryGetWings().DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                        largoAppearance.Structures.TryGetEars().DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                        largoAppearance._wingFlapAnimationOverride = secondaryDef.AppearancesDefault[0]._wingFlapAnimationOverride;

                        largoDefinition.AppearancesDefault = new SlimeAppearance[] { largoAppearance };
                        largoDefinition.prefab.hideFlags |= HideFlags.HideAndDontSave;
                        #endregion
                        break;
                    }
            }
        }
    }
}
