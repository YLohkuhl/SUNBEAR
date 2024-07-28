using Il2CppMonomiPark.SlimeRancher.Regions;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearGoto : SRBehaviour
    {
        private SlimeRandomMove slimeRandomMove;
        private Rigidbody slimeBody;
        private SlimeAudio slimeAudio;
        private float startTime;

        public float facingSpeed = 5;
        public float facingStability = 1;
        public float pursuitSpeedFactor;

        void Awake()
        {
            if (pursuitSpeedFactor == default)
            {
                if (SunBearPreferences.IsRealisticMode.Value)
                    pursuitSpeedFactor = SunBearCache.IsCub(gameObject) ? 1 : 2.5f;
                else
                    pursuitSpeedFactor = SunBearCache.IsCub(gameObject) ? 0.8f : 2;
            }

            slimeRandomMove = GetComponent<SlimeRandomMove>();
            slimeBody = GetComponent<Rigidbody>();
            slimeAudio = GetComponent<SlimeAudio>();
            startTime = Time.time;
        }

        public bool IsBlocked(GameObject target) => slimeRandomMove.IsBlocked(target);

        public float ScootCycleSpeed() => Mathf.Sin((Time.time - startTime) * 6.2831855f) + 1f;

        public void RotateTowards(Vector3 dirToTarget) => slimeRandomMove.RotateTowards(dirToTarget, facingSpeed, facingStability);

        public void MoveTowards(Vector3 targetPos, bool shouldJump, ref float nextJumpAvail, float jumpStrength)
        {
            if (slimeRandomMove.IsGrounded())
            {
                Vector3 vector = targetPos - transform.position;
                float sqrMagnitude = vector.sqrMagnitude;
                Vector3 normalized = vector.normalized;
                RotateTowards(normalized);
                if (shouldJump)
                {
                    if (Time.fixedTime >= nextJumpAvail)
                    {
                        float d = Mathf.Min(1f, Mathf.Sqrt(sqrMagnitude) / 30);
                        slimeBody.AddForce((normalized * d + Vector3.up).normalized * jumpStrength * slimeBody.mass, ForceMode.Impulse);
                        slimeAudio.Play(slimeAudio.SlimeSounds.JumpClipDefinition);
                        slimeAudio.Play(slimeAudio.SlimeSounds.VoiceJumpClipDefinition);
                        nextJumpAvail = Time.time + 1;
                        return;
                    }
                }
                else
                {
                    if (sqrMagnitude <= 9f)
                    {
                        slimeBody.AddForce(normalized * (480 * pursuitSpeedFactor * slimeBody.mass * Time.fixedDeltaTime));
                        return;
                    }
                    float num = ScootCycleSpeed();
                    slimeBody.AddForce(normalized * (150 * slimeBody.mass * pursuitSpeedFactor * Time.fixedDeltaTime * num));
                    Vector3 position = transform.position + Vector3.down * (0.5f * transform.localScale.y);
                    slimeBody.AddForceAtPosition(normalized * (270 * slimeBody.mass * Time.fixedDeltaTime * num), position);
                }
            }
        }

    }
}
