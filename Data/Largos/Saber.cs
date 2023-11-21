using HarmonyLib;
using Il2CppInterop.Runtime;
using MelonLoader;
using SUNBEAR.Assist;
using SUNBEAR.Components;
using SUNBEAR.Data.Slimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Data.Largos
{
    internal class Saber
    {
        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        SlimeDefinition largoDefinition = Get<SlimeDefinition>("SunBearSaber");
                        SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
                        SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
                        string largoName = "Saber Sun Bear Largo";

                        LargoHelper.PresetLargo(largoName, largoDefinition);
                        largoDefinition.Sounds = Get<SlimeSounds>("SaberLargo");
                        largoDefinition.prefab.GetComponent<SlimeAudio>().SlimeSounds = largoDefinition.Sounds;

                        #region SABER_SUN_BEAR_LARGO
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
                        largoAppearance.Face.OnEnable();

                        List<SlimeAppearanceStructure> structures = largoAppearance.Structures.ToList();
                        structures.Remove(structures.TryGetBody());
                        structures.Remove(structures.TryGetFace());
                        structures.Add(new SlimeAppearanceStructure(secondaryDef.AppearancesDefault[0].Structures.TryGetBody()));
                        largoAppearance.Structures = structures.ToArray();

                        Material primaryMat = primaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];

                        Material largoMaterial = UnityEngine.Object.Instantiate(primaryMat);
                        largoMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        largoMaterial.name = largoAppearance.name + "_Body";
                        largoMaterial.SetTexture("_ColorMask", LocalAssets.maskSunBearSaberMulticolor);

                        largoAppearance.Structures.TryGetBody().DefaultMaterials[0] = largoMaterial;
                        largoAppearance._roarAppearance = secondaryDef.AppearancesDefault[0].RoarAppearance;

                        largoDefinition.AppearancesDefault = new SlimeAppearance[] { largoAppearance };
                        largoDefinition.prefab.hideFlags |= HideFlags.HideAndDontSave;
                        #endregion
                        break;
                    }
            }
        }
    }
}