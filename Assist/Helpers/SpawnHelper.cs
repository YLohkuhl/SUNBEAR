using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using MelonLoader;
using SUNBEAR.Data.Slimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Il2Cpp.EditorLocationData;

namespace SUNBEAR.Assist.Helpers
{
    internal class SpawnHelper
    {
        private static readonly string[] ignoredNodeNames = new string[]
        {
            "Only",
            "Puddle",
            "Gold",
            "Yolky",
            "Feral",
            "Phosphor",
            "Ringtail",
            "Cotton",
            "Angler",
            "Cave",
            "Largo"
        };

        // These are personalized for the Sun Bear slimes!!
        public static List<DirectedActorSpawner.SpawnConstraint[]> AddConstraintToLocation(Transform location, DirectedActorSpawner.SpawnConstraint constraint)
        {
            if (location.transform.GetChild(2).GetComponent<DirectedSlimeSpawner>().Constraints.FirstOrDefault(x => x == constraint, null) != null)
                return null;

            List<DirectedActorSpawner.SpawnConstraint[]> locationConstraints = new List<DirectedActorSpawner.SpawnConstraint[]>();
            foreach (var transform in location)
            {
                if (transform.GetIl2CppType() != Il2CppType.Of<Transform>())
                    continue;

                var nodeSlime = transform.TryCast<Transform>().gameObject.GetComponent<DirectedSlimeSpawner>();
                if (nodeSlime == null)
                    continue;

                if (ignoredNodeNames.Any(s => nodeSlime.name.Contains(s)))
                    continue;

                locationConstraints.Add(nodeSlime.Constraints);

                if (nodeSlime.Constraints.FirstOrDefault(x => x == constraint, null) == null)
                    nodeSlime.Constraints = nodeSlime.Constraints.ToArray().AddToArray(constraint);
            }
            return locationConstraints;
        }

        public static void RemoveFromLocation(string identifiableName, List<DirectedActorSpawner.SpawnConstraint[]> locationConstraints) 
        {
            if (locationConstraints.Count > 0)
            {
                foreach (var constraints in locationConstraints)
                {
                    foreach (var constraint in constraints)
                    {
                        if (constraint.Slimeset.Members.FirstOrDefault(x => x.IdentType.name.Contains(identifiableName), null) != null)
                        {
                            List<SlimeSet.Member> members = constraint.Slimeset.Members.ToList();
                            members.RemoveAt(members.FindIndex(x => x.IdentType.name == identifiableName));
                            constraint.Slimeset.Members = members.ToArray();
                        }
                    }
                }
            }
        }
    }
}
