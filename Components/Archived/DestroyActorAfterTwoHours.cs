using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components.Archived
{
    [RegisterTypeInIl2Cpp]
    internal class DestroyActorAfterTwoHours : SRBehaviour
    {
        private TimeDirector timeDir;
        private double destroyTime;

        void Start()
        {
            timeDir = SceneContext.Instance.TimeDirector;
            destroyTime = timeDir.HoursFromNowOrStart(2);
        }

        void Update()
        {
            if (timeDir.HasReached(destroyTime))
            {
                SpawnAndPlayFX(Get<IdentifiableType>("PinkPlort").prefab.GetComponent<DestroyActorAfterTime>().destroyFX);
                Destroyer.DestroyActor(gameObject, "DestroyActorAfterTwoHours.Update");
            }
        }
    }
}
