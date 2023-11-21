using HarmonyLib;
using Il2Cpp;
using SUNBEAR.Assist;
using SUNBEAR.Data.Slimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Data.Largos
{
    internal class Angler
    {
        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        SlimeDefinition largoDefinition = Get<SlimeDefinition>("SunBearAngler");
                        SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
                        SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
                        string largoName = "Sun Bear Angler Largo";

                        LargoHelper.PresetLargo(largoName, largoDefinition);
                        largoDefinition.Sounds = Get<SlimeSounds>("AnglerLargo");
                        largoDefinition.prefab.GetComponent<SlimeAudio>().SlimeSounds = largoDefinition.Sounds;

                        #region SUN_BEAR_ANGLER_LARGO
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
                        largoAppearance._face = UnityEngine.Object.Instantiate(secondaryDef.AppearancesDefault[0].Face);
                        largoAppearance.Face.name = "faceSlimeSunBearAngler";

                        SlimeExpressionFace[] expressionFaces = new SlimeExpressionFace[0];
                        foreach (SlimeExpressionFace slimeExpressionFace in largoAppearance.Face.ExpressionFaces)
                        {
                            Material slimeEyes = null;
                            Material slimeMouth = null;

                            if (slimeExpressionFace.Eyes)
                                slimeEyes = UnityEngine.Object.Instantiate(slimeExpressionFace.Eyes);
                            if (slimeExpressionFace.Mouth)
                                slimeMouth = UnityEngine.Object.Instantiate(slimeExpressionFace.Mouth);

                            if (slimeEyes)
                            {
                                slimeEyes.SetColor("_EyeRed", SunBear.sunBearPalette[4]);
                                slimeEyes.SetColor("_EyeGreen", Color.gray);
                                slimeEyes.SetColor("_EyeBlue", SunBear.sunBearPalette[4]);
                            }
                            if (slimeMouth)
                            {
                                slimeMouth.SetColor("_MouthTop", SunBear.sunBearPalette[1]);
                                slimeMouth.SetColor("_MouthMid", Color.gray);
                                slimeMouth.SetColor("_MouthBot", SunBear.sunBearPalette[1]);
                            }
                            slimeExpressionFace.Eyes = slimeEyes;
                            slimeExpressionFace.Mouth = slimeMouth;
                            expressionFaces = expressionFaces.AddToArray(slimeExpressionFace);
                        }
                        largoAppearance.Face.ExpressionFaces = expressionFaces;
                        largoAppearance.Face.OnEnable();

                        Material primaryMat = primaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];

                        Material largoMaterial = UnityEngine.Object.Instantiate(primaryMat);
                        largoMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        largoMaterial.name = largoAppearance.name + "_Body";

                        largoAppearance.Structures[0].DefaultMaterials[0] = largoMaterial;
                        largoAppearance.Structures[2].DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                        largoAppearance.Structures.FirstOrDefault(x =>
                            x.Element.Type == SlimeAppearanceElement.ElementType.FACE_ATTACH ||
                            x.Element.Name.Contains("Lure")).DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                        largoAppearance.Structures.FirstOrDefault(x =>
                            x.Element.Type == SlimeAppearanceElement.ElementType.TOP ||
                            x.Element.Name.Contains("Fin (Top)")).DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                        largoAppearance.Structures.FirstOrDefault(x =>
                            x.Element.Type == SlimeAppearanceElement.ElementType.SIDE ||
                            x.Element.Name.Contains("Fins (Side)")).DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];

                        largoDefinition.AppearancesDefault = new SlimeAppearance[] { largoAppearance };
                        largoDefinition.prefab.hideFlags |= HideFlags.HideAndDontSave;
                        #endregion
                        break;
                    }
            }
        }
    }
}
