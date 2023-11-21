using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components.Archived
{
    // No collider on tentacle ;-;
    [RegisterTypeInIl2Cpp]
    internal class SunBearTentacleBite : SRBehaviour
    {
        private SlimeAudio slimeAudio;
        private Chomper chomper;

        void Start()
        {
            slimeAudio = GetComponent<SlimeAudio>();
            chomper = GetComponent<Chomper>();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;

            if (!collision.gameObject.GetComponent<TentacleHook>())
                return;

            AttemptSpinAndBite(collision.gameObject);
        }

        public void StartTentacleChomp(GameObject other, bool quick, bool invokeCompleteBite = true)
        {
            chomper.faceAnim.SetTrigger(quick ? "triggerChompOpenQuick" : "triggerChompOpen");
            chomper.bodyAnim.SetBool(quick ? chomper.animQuickBiteId : chomper.animBiteId, true);
            if (invokeCompleteBite)
                CompleteBite(other, quick);
        }

        bool AttemptSpinAndBite(GameObject other)
        {
            if (chomper.CanChomp() && other != null)
            {
                transform.LookAt(other.transform);
                StartTentacleChomp(other, false);
                return true;
            }
            return false;
        }

        void CompleteBite(GameObject other, bool quick)
        {
            if (other == null)
                return;

            slimeAudio.Play(slimeAudio.slimeSounds.attackClipDefinition);
            if (other.GetComponentInParent<TentacleGrapple>().grappling)
                other.GetComponentInParent<TentacleGrapple>().Release();
            chomper.bodyAnim.SetBool(quick ? chomper.animQuickBiteId : chomper.animBiteId, false);
            MelonLogger.Msg("Completed tentacle bite");
        }
    }
}
