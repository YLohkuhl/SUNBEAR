using Il2CppSystem;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearPlayful : SRBehaviour
    {
        private enum Mode { NONE, ROLL, JUMP, SPIN_JUMP, BITE }
        private static Dictionary<Mode, float> MODE_WEIGHTS = new Dictionary<Mode, float>();

        private SlimeRandomMove slimeRandomMove;
        private SlimeEmotions slimeEmotions;
        private SlimeAudio slimeAudio;
        private Rigidbody rigidbody;
        private Chomper chomper;

        private Mode currentMode;
        private Vector3 jumpDirection;

        private float nextModeChange;
        private float nextRollTime;
        private float nextJumpTime;
        private float nextBiteTime;

        private float extraJump;
        private float extraScootSpeed;

        static SunBearPlayful()
        {
            MODE_WEIGHTS[Mode.NONE] = 0.5f;
            MODE_WEIGHTS[Mode.ROLL] = 0.4f;
            MODE_WEIGHTS[Mode.JUMP] = 0.3f;
            MODE_WEIGHTS[Mode.SPIN_JUMP] = 0.3f;
            MODE_WEIGHTS[Mode.BITE] = 0.2f;
        }

        void Start()
        {
            slimeRandomMove = GetComponent<SlimeRandomMove>();
            slimeEmotions = GetComponent<SlimeEmotions>();
            slimeAudio = GetComponent<SlimeAudio>();
            rigidbody = GetComponent<Rigidbody>();
            chomper = GetComponent<Chomper>();
        }

        void FixedUpdate()
        {
            if (SRSingleton<SceneContext>.Instance.TimeDirector.IsFastForwarding())
                return;

            if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.HUNGER) >= 0.99f || slimeEmotions.GetCurr(SlimeEmotions.Emotion.AGITATION) >= 0.9f || slimeEmotions.GetCurr(SlimeEmotions.Emotion.FEAR) >= 0.6f)
                return;

            Process();
        }

        private System.Collections.IEnumerator BiteOnlyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            try
            {
                if (gameObject != null)
                {
                    chomper._bodyAnim.SetBool(chomper._animBiteId, false);
                    slimeAudio.Play(slimeAudio.SlimeSounds.AttackClipDefinition);
                }
            }
            catch
            {

            }
            yield break;
        }

        void Process()
        {
            if (!slimeRandomMove.IsGrounded())
                return;

            if (Time.fixedTime > nextModeChange)
            {
                currentMode = URandom.Pick(MODE_WEIGHTS, Mode.NONE);
                nextModeChange = Time.time + 10;
            }

            switch (currentMode)
            {
                case Mode.NONE:
                    #region NONE
                    if (slimeRandomMove.enabled == false)
                        slimeRandomMove.enabled = true;
                    // MelonLogger.Msg("None");
                    #endregion
                    break;

                case Mode.ROLL:
                    #region ROLL
                    if (Time.time > nextRollTime && slimeRandomMove.IsGrounded())
                    {
                        if (slimeRandomMove.enabled == true)
                            slimeRandomMove.enabled = false;
                        // rollDirection = UnityEngine.Random.insideUnitSphere;
                        extraScootSpeed = slimeRandomMove.ScootSpeedFactor + 0.5f;

                        slimeAudio.Play(slimeAudio.SlimeSounds.RollClipDefinition);
                        slimeAudio.Play(slimeAudio.SlimeSounds.VoiceFunClipDefinition);

                        nextRollTime = Time.fixedTime + 3;
                        // MelonLogger.Msg("Rolling");
                    }
                    Vector3 rollAxis = transform.right;
                    rollAxis.y = 0;
                    rollAxis.Normalize();

                    rigidbody.AddTorque(rollAxis * (550 * extraScootSpeed * rigidbody.mass * Time.fixedDeltaTime));
                    rigidbody.AddForce(Vector3.Cross(rollAxis, Vector3.up) * (350 * rigidbody.mass * Time.fixedDeltaTime));
                    #endregion
                    break;

                case Mode.JUMP:
                    #region JUMP
                    if (Time.time > nextJumpTime && rigidbody.velocity.sqrMagnitude <= 25f && slimeRandomMove.IsGrounded())
                    {
                        if (slimeRandomMove.enabled == true)
                            slimeRandomMove.enabled = false;
                        jumpDirection = UnityEngine.Random.insideUnitSphere;
                        extraJump = slimeRandomMove._maxJump + 6;

                        float calculation = Mathf.Min(1f, Mathf.Sqrt(transform.position.sqrMagnitude) / 30f);
                        rigidbody.AddForce((jumpDirection * calculation + Vector3.up).normalized * extraJump * rigidbody.mass, ForceMode.Impulse);
                        slimeAudio.Play(slimeAudio.SlimeSounds.JumpClipDefinition);
                        slimeAudio.Play(slimeAudio.SlimeSounds.VoiceFunClipDefinition);

                        nextJumpTime = Time.fixedTime + 3;
                        // MelonLogger.Msg("Jumping");
                        return;
                    }
                    #endregion
                    break;

                case Mode.SPIN_JUMP:
                    #region SPIN_JUMP
                    if (Time.time > nextJumpTime && rigidbody.velocity.sqrMagnitude <= 25f && slimeRandomMove.IsGrounded())
                    {
                        if (slimeRandomMove.enabled == true)
                            slimeRandomMove.enabled = false;
                        jumpDirection = UnityEngine.Random.insideUnitSphere;
                        extraJump = slimeRandomMove._maxJump + 6;

                        float calculation = Mathf.Min(1f, Mathf.Sqrt(transform.position.sqrMagnitude) / 30f);
                        rigidbody.AddForce((jumpDirection * calculation + Vector3.up).normalized * extraJump * rigidbody.mass, ForceMode.Impulse);
                        slimeAudio.Play(slimeAudio.SlimeSounds.JumpClipDefinition);
                        slimeAudio.Play(slimeAudio.SlimeSounds.VoiceFunClipDefinition);

                        nextJumpTime = Time.fixedTime + 3;
                        // MelonLogger.Msg("Spin Jumping");
                        return;
                    }
                    rigidbody.AddTorque(transform.up * 10);
                    #endregion
                    break;

                case Mode.BITE:
                    #region BITE
                    if (Time.time > nextBiteTime && !chomper.IsChomping() && slimeRandomMove.IsGrounded())
                    {
                        if (slimeRandomMove.enabled == true)
                            slimeRandomMove.enabled = false;

                        chomper._faceAnim.SetTrigger("triggerChompOpen");
                        chomper._bodyAnim.SetBool(chomper._animBiteId, true);
                        MelonCoroutines.Start(BiteOnlyAfterDelay(1));

                        nextBiteTime = Time.fixedTime + 4;
                        // MelonLogger.Msg("Biting");
                        return;
                    }
                    #endregion
                    break;

                default:
                    currentMode = Mode.NONE;
                    break;
            }
        }
    }
}
