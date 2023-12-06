using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Assist
{
    // eeeek came from heart slime eeeek
    internal class GordoBuilder
    {
        public static void PlaceGordo(IdentifiableType gordoIdentifiable, Transform parent, Vector3 position, float rotationAngle)
        {
            GameObject instantiatedGordo = UnityEngine.Object.Instantiate(gordoIdentifiable.prefab, PrefabUtils.DisabledParent.transform);
            instantiatedGordo.SetActive(false);
            instantiatedGordo.name = gordoIdentifiable.prefab.name;

            instantiatedGordo.transform.SetParent(parent);
            instantiatedGordo.transform.position = position;
            instantiatedGordo.transform.RotateAround(instantiatedGordo.transform.position, instantiatedGordo.transform.up, rotationAngle);

            var gordoEat = instantiatedGordo.GetComponent<GordoEat>();
            gordoEat._director = gordoEat.GetComponentInParent<IdDirector>();
            gordoEat._director.persistenceDict.Add(gordoEat, gordoIdentifiable.name + "Dict");
            instantiatedGordo.SetActive(true);
        }

        public static (IdentifiableType, GameObject) CreateGordo(IdentifiableType baseGordo, SlimeDefinition baseSlime, IdentifiableType assignedGordo, Sprite assignedGordoIcon, string assignedGordoName, int feedCount, GameObject[] gordoRewards)
        {
            assignedGordo.localizedName = GeneralizedHelper.CreateTranslation("Pedia", "t." + assignedGordo.localizationSuffix, assignedGordoName);
            assignedGordo.showForZones = baseSlime.NativeZones;
            assignedGordo.icon = assignedGordoIcon;

            GameObject assignedGordoPrefab = UnityEngine.Object.Instantiate(baseGordo.prefab);
            GameObject baseSlimePrefab = baseSlime.prefab;
            assignedGordoPrefab.Prefabitize();
            assignedGordoPrefab.hideFlags |= HideFlags.HideAndDontSave;
            assignedGordoPrefab.name = "gordo" + assignedGordo.name.Replace("Gordo", "");

            assignedGordoPrefab.GetComponent<GordoEat>().TargetCount = feedCount;
            assignedGordoPrefab.GetComponent<GordoEat>().SlimeDefinition = baseSlime;
            assignedGordoPrefab.GetComponent<GordoRewards>().RewardPrefabs = gordoRewards;
            assignedGordoPrefab.GetComponent<GordoIdentifiable>().identType = assignedGordo;

            Material gordoMaterial = baseSlime.AppearancesDefault[0].Structures[0].DefaultMaterials[0];
            GameObject slime_gordo = assignedGordoPrefab.transform.Find("Vibrating/slime_gordo").gameObject;
            slime_gordo.GetComponent<SkinnedMeshRenderer>().sharedMaterial = gordoMaterial;

            SlimeFace baseSlimeFace = baseSlime.AppearancesDefault[0].Face;
            GordoFaceComponents gordoFace = assignedGordoPrefab.GetComponent<GordoFaceComponents>();

            gordoFace.StrainEyes = baseSlimeFace.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.SCARED).Eyes;
            gordoFace.StrainMouth = baseSlimeFace.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.SCARED).Mouth;
            gordoFace.BlinkEyes = baseSlimeFace.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.BLINK).Eyes;
            gordoFace.ChompOpenMouth = baseSlimeFace.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.CHOMP_OPEN).Mouth;
            gordoFace.HappyMouth = baseSlimeFace.ExpressionFaces.First(x => x.SlimeExpression == SlimeFace.SlimeExpression.HAPPY).Mouth;

            assignedGordo.prefab = assignedGordoPrefab;
            SRSingleton<GameContext>.Instance.LookupDirector.gordoDict.Add(assignedGordo, assignedGordo.prefab);
            SRSingleton<GameContext>.Instance.LookupDirector.gordoEntries.items.Add(assignedGordo.prefab);

            return (assignedGordo, assignedGordo.prefab);
        }
    }
}
