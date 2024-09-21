using HarmonyLib;
using Il2CppSystem;
using System.Collections;
using System.Reflection;
using static SUNBEAR.Assist.EnumPatchHelper;
using Array = System.Array;
using Enum = Il2CppSystem.Enum;
using Type = System.Type;

namespace CustomSpawnerUpgrade.Harmony
{
    [HarmonyPatch]
    internal static class EnumInfoPatch
    {
        private static MethodBase TargetMethod() => typeof(System.Enum).GetMethod("GetEnumInfo", AccessTools.all);

        private static void FixEnum(object type, ref ulong[] values, ref string[] names)
        {
            Type enumType = type as Type;
            EnumPatch patch;
            if (TryGetRawPatch(enumType, out patch))
            {
                List<KeyValuePair<ulong, string>> patchPairs = patch.GetPairs();
                ulong[] patchValues = patchPairs.Select((KeyValuePair<ulong, string> x) => x.Key).ToArray<ulong>();
                string[] patchNames = patchPairs.Select((KeyValuePair<ulong, string> x) => x.Value).ToArray<string>();
                HashSet<ulong> existingValuesSet = new HashSet<ulong>(values);
                List<ulong> newValuesList = new List<ulong>(values);
                List<string> newNamesList = ((names != null) ? new List<string>(names) : null);
                for (int i = 0; i < patchValues.Length; i++)
                {
                    if (!existingValuesSet.Contains(patchValues[i]))
                    {
                        newValuesList.Add(patchValues[i]);
                        if (newNamesList != null)
                            newNamesList.Add(patchNames[i]);
                    }
                }
                values = newValuesList.ToArray();
                Type elementType = typeof(ulong);
                IComparer comparer = (IComparer)typeof(Comparer<>).MakeGenericType(new Type[] { elementType }).GetProperty("Default").GetValue(null);
                if (newNamesList != null)
                {
                    names = newNamesList.ToArray();
                    Array.Sort(values, names, comparer);
                }
                else
                    Array.Sort(values, comparer);
            }
        }

        [HarmonyPostfix]
        public static void Postfix(object enumType, ref object __result)
        {
            Type enumInfoType = __result.GetType();
            FieldInfo namesField = enumInfoType.GetField("Names", AccessTools.all);
            FieldInfo valuesField = enumInfoType.GetField("Values", AccessTools.all);
            string[] names = (string[])namesField.GetValue(__result);
            ulong[] values = (ulong[])valuesField.GetValue(__result);
            FixEnum(enumType, ref values, ref names);
            namesField.SetValue(__result, names);
            valuesField.SetValue(__result, values);
        }
    }

    [HarmonyPatch(typeof(Enum), nameof(Enum.GetCachedValuesAndNames))]
    internal static class EnumInfoIL2CPPPatch
    {
        public static void Postfix(ref Enum.ValuesAndNames __result, RuntimeType enumType)
        {
            ulong[] il2CppStructArray = __result.Values;
            string[] il2CppStringArray = __result.Names;
            Il2CppSystem.Type tryCast = enumType.TryCast<Il2CppSystem.Type>();
            bool flag = tryCast == null;
            if (!flag)
            {
                FixEnum(tryCast, ref il2CppStructArray, ref il2CppStringArray);
            }
        }

        private static void FixEnum(Il2CppSystem.Type type, ref ulong[] values, ref string[] names)
        {
            EnumPatch patch;
            bool flag = !TryGetRawPatchInIL2CPP(type, out patch);
            if (!flag)
            {
                List<KeyValuePair<ulong, string>> patchPairs = patch.GetPairs();
                ulong[] patchValues = patchPairs.Select((KeyValuePair<ulong, string> x) => x.Key).ToArray<ulong>();
                string[] patchNames = patchPairs.Select((KeyValuePair<ulong, string> x) => x.Value).ToArray<string>();
                HashSet<ulong> existingValuesSet = new HashSet<ulong>(values);
                List<ulong> newValuesList = new List<ulong>(values);
                List<string> newNamesList = ((names != null) ? new List<string>(names) : null);
                for (int i = 0; i < patchValues.Length; i++)
                {
                    bool flag2 = !existingValuesSet.Contains(patchValues[i]);
                    if (flag2)
                    {
                        newValuesList.Add(patchValues[i]);
                        bool flag3 = newNamesList != null;
                        if (flag3)
                        {
                            newNamesList.Add(patchNames[i]);
                        }
                    }
                }
                values = newValuesList.ToArray();
                Type elementType = typeof(ulong);
                IComparer comparer = (IComparer)typeof(Comparer<>).MakeGenericType(new Type[] { elementType }).GetProperty("Default").GetValue(null);
                bool flag4 = newNamesList != null;
                if (flag4)
                {
                    names = newNamesList.ToArray();
                    Array.Sort(values, names, comparer);
                }
                else
                {
                    Array.Sort(values, comparer);
                }
            }
        }
    }
}
