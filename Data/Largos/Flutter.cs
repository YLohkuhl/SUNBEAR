using HarmonyLib;
using SUNBEAR.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Data.Largos
{
    internal class Flutter
    {
        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        SlimeDefinition largoDefinition = Get<SlimeDefinition>("SunBearFlutter");
                        SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
                        SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
                        string largoName = "Flutter Sun Bear Largo";

                        LargoHelper.PresetLargo(largoName, largoDefinition);

                        #region FLUTTER_SUN_BEAR_LARGO
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

                        List<SlimeAppearanceStructure> structures = largoAppearance.Structures.ToList();
                        structures.Remove(structures.TryGetWings());
                        structures.Add(new SlimeAppearanceStructure(Get<SlimeAppearance>("CottonDefaultFlutterDefault").Structures.TryGetWings()));
                        largoAppearance.Structures = structures.ToArray();

                        Material primaryMat = primaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];
                        Material secondaryMat = secondaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];

                        /*Material largoMaterial = UnityEngine.Object.Instantiate(secondaryMat);
                        largoMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        largoMaterial.name = largoAppearance.name + "_Body";
                        largoMaterial.SetTexture("_ColorMask", LocalAssets.maskSunBearMulticolorGreen);
                        largoMaterial.SetColor("_GreenTopColor", secondaryMat.GetColor("_TopColor").AdjustBrightness(0.3f));
                        largoMaterial.SetColor("_GreenMiddleColor", secondaryMat.GetColor("_MiddleColor").AdjustBrightness(0.3f));
                        largoMaterial.SetColor("_GreenBottomColor", secondaryMat.GetColor("_BottomColor").AdjustBrightness(0.3f));

                        List<string> shaderKeywords = largoMaterial.GetShaderKeywords().ToList();
                        shaderKeywords.Add("_BODYCOLORING_MULTI");
                        shaderKeywords.Remove("_BODYCOLORING_DEFAULT");
                        largoMaterial.SetShaderKeywords(shaderKeywords.ToArray());*/

                        /*Material antennaeMaterial = UnityEngine.Object.Instantiate(secondaryDef.AppearancesDefault[0].Structures.FirstOrDefault(x =>
                            x.Element.Type == SlimeAppearanceElement.ElementType.FACE_ATTACH ||
                            x.Element.Name.Contains("Antennae", StringComparison.OrdinalIgnoreCase)).DefaultMaterials[0]);
                        antennaeMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        antennaeMaterial.name = largoAppearance.name + "_Antennae";
                        antennaeMaterial.SetColor("_TopColor", secondaryMat.GetColor("_TopColor"));
                        antennaeMaterial.SetColor("_MiddleColor", secondaryMat.GetColor("_MiddleColor"));
                        antennaeMaterial.SetColor("_BottomColor", secondaryMat.GetColor("_BottomColor"));*/

                        Material wingsMaterial = UnityEngine.Object.Instantiate(secondaryDef.AppearancesDefault[0].Structures.TryGetWings().DefaultMaterials[0]);
                        wingsMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        wingsMaterial.name = largoAppearance.name + "_Wings";
                        wingsMaterial.SetColor("_RedTopColor", primaryMat.GetColor("_RedTopColor"));
                        wingsMaterial.SetColor("_RedMiddleColor", primaryMat.GetColor("_RedMiddleColor"));
                        wingsMaterial.SetColor("_RedBottomColor", primaryMat.GetColor("_RedBottomColor"));
                        wingsMaterial.SetColor("_GreenTopColor", Color.gray);
                        wingsMaterial.SetColor("_GreenMiddleColor", Color.gray);
                        wingsMaterial.SetColor("_GreenBottomColor", Color.gray);
                        wingsMaterial.SetColor("_BlueTopColor", primaryMat.GetColor("_GreenTopColor"));
                        wingsMaterial.SetColor("_BlueMiddleColor", primaryMat.GetColor("_GreenMiddleColor"));
                        wingsMaterial.SetColor("_BlueBottomColor", primaryMat.GetColor("_GreenBottomColor"));

                        // largoAppearance.Structures[0].DefaultMaterials[0] = largoMaterial;
                        // largoAppearance.Structures[2].DefaultMaterials[0] = secondaryMat;
                        largoAppearance.Structures.FirstOrDefault(x =>
                            x.Element.Type == SlimeAppearanceElement.ElementType.FACE_ATTACH ||
                            x.Element.Name.Contains("Antennae", StringComparison.OrdinalIgnoreCase)).DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                        largoAppearance.Structures.TryGetWings().DefaultMaterials[0] = wingsMaterial;

                        largoDefinition.AppearancesDefault = new SlimeAppearance[] { largoAppearance };
                        largoDefinition.prefab.hideFlags |= HideFlags.HideAndDontSave;
                        #endregion
                        break;
                    }
            }
        }
    }
}
