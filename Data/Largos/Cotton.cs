using HarmonyLib;
using SUNBEAR.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Data.Largos
{
    internal class Cotton
    {
        public static void Load(string sceneName)
        {
            switch (sceneName)
            {
                case "GameCore":
                    {
                        SlimeDefinition largoDefinition = Get<SlimeDefinition>("SunBearCotton");
                        SlimeDefinition primaryDef = largoDefinition.BaseSlimes[0];
                        SlimeDefinition secondaryDef = largoDefinition.BaseSlimes[1];
                        string largoName = "Cotton Sun Bear Largo";

                        LargoHelper.PresetLargo(largoName, largoDefinition);
                        largoDefinition.Sounds = Get<SlimeSounds>("CottonLargo");
                        largoDefinition.prefab.GetComponent<SlimeAudio>().SlimeSounds = largoDefinition.Sounds;

                        #region COTTON_SUN_BEAR_LARGO
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
                        structures.Remove(structures.TryGetEars());
                        structures.Add(new SlimeAppearanceStructure(secondaryDef.AppearancesDefault[0].Structures.TryGetEars()));
                        largoAppearance.Structures = structures.ToArray();

                        Material primaryMat = primaryDef.AppearancesDefault[0].Structures.TryGetBody().DefaultMaterials[0];
                        Material largoMaterial = UnityEngine.Object.Instantiate(primaryMat);
                        largoMaterial.hideFlags |= HideFlags.HideAndDontSave;
                        largoMaterial.name = largoAppearance.name + "_Body";

                        largoAppearance.Structures[0].DefaultMaterials[0] = largoMaterial;
                        largoAppearance.Structures.TryGetEars().DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                        largoAppearance.Structures.TryGetTail().DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];
                        largoAppearance.Structures.FirstOrDefault(x =>
                            x.Element.Type == SlimeAppearanceElement.ElementType.WHISKERS || 
                            x.Element.Name.Contains("Whiskers", StringComparison.OrdinalIgnoreCase)).DefaultMaterials[0] = primaryDef.AppearancesDefault[0].Structures[2].DefaultMaterials[0];

                        largoDefinition.AppearancesDefault = new SlimeAppearance[] { largoAppearance };
                        largoDefinition.prefab.hideFlags |= HideFlags.HideAndDontSave;
                        #endregion
                        break;
                    }
            }
        }
    }
}
