using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNBEAR.Components
{
    [RegisterTypeInIl2Cpp]
    internal class SunBearCacheTrigger : SRBehaviour
    {
        SunBearCache sunBearCache;

        void Start() => sunBearCache = GetComponentInParent<SunBearCache>();

        void OnTriggerEnter(Collider collider) => ProcessOnTriggerEnter(collider);

        void OnTriggerExit(Collider collider) => ProcessOnTriggerExit(collider);

        void ProcessOnTriggerEnter(Collider collider)
        {
            if (collider.gameObject == null)
                return;

            if (transform.name.Contains("CubTrigger"))
            {
                if (SunBearCache.IsCub(collider.gameObject))
                {
                    if (!sunBearCache.GetCubCache().Contains(collider.gameObject))
                    {
                        sunBearCache.GetCubCache().Add(collider.gameObject);
                        return;
                    }
                }
            }

            if (transform.name.Contains("BearTrigger"))
            {
                if (SunBearCache.IsBear(collider.gameObject))
                {
                    if (!sunBearCache.GetBearCache().Contains(collider.gameObject))
                    {
                        sunBearCache.GetBearCache().Add(collider.gameObject);
                        return;
                    }
                }
            }

            if (transform.name.Contains("HiveTrigger"))
            {
                if (SunBearCache.IsHive(collider.gameObject))
                {
                    if (!sunBearCache.GetHiveCache().Contains(collider.gameObject))
                    {
                        sunBearCache.GetHiveCache().Add(collider.gameObject);
                        return;
                    }
                }
            }

            if (transform.name.Contains("SlimeLargoTrigger"))
            {
                if (SunBearCache.IsSlimeOrLargo(collider.gameObject))
                {
                    if (!sunBearCache.GetSlimeLargoCache().Contains(collider.gameObject))
                    {
                        sunBearCache.GetSlimeLargoCache().Add(collider.gameObject);
                        return;
                    }
                }
            }
        }

        void ProcessOnTriggerExit(Collider collider)
        {
            if (collider.gameObject == null)
                return;

            if (transform.name.Contains("CubTrigger"))
            {
                if (SunBearCache.IsCub(collider.gameObject))
                {
                    if (sunBearCache.GetCubCache().Contains(collider.gameObject))
                    {
                        sunBearCache.GetCubCache().Remove(collider.gameObject);
                        return;
                    }
                }
            }

            if (transform.name.Contains("BearTrigger"))
            {
                if (SunBearCache.IsBear(collider.gameObject))
                {
                    if (sunBearCache.GetBearCache().Contains(collider.gameObject))
                    {
                        sunBearCache.GetBearCache().Remove(collider.gameObject);
                        return;
                    }
                }
            }

            if (transform.name.Contains("HiveTrigger"))
            {
                if (SunBearCache.IsHive(collider.gameObject))
                {
                    if (sunBearCache.GetHiveCache().Contains(collider.gameObject))
                    {
                        sunBearCache.GetHiveCache().Remove(collider.gameObject);
                        return;
                    }
                }
            }

            if (transform.name.Contains("SlimeLargoTrigger"))
            {
                if (SunBearCache.IsSlimeOrLargo(collider.gameObject))
                {
                    if (sunBearCache.GetSlimeLargoCache().Contains(collider.gameObject))
                    {
                        sunBearCache.GetSlimeLargoCache().Remove(collider.gameObject);
                        return;
                    }
                }
            }
        }
    }
}
