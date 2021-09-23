using System.Collections;
using UnityEngine;

namespace MiddleAges.Resources
{
    public abstract class RegenerableResource : Resource
    {
        protected float regenRate;
        protected Coroutine regenCoroutine;

        private float updatesPerSecond = 60;

        protected virtual void Start()
        {
            regenCoroutine = StartCoroutine(Regen());
        }

        protected IEnumerator Regen()
        {
            for (; ; )
            {
                if (currentResourcePoints < maxResourcePoints)
                {
                    IncResource(regenRate / updatesPerSecond);
                    yield return new WaitForSeconds(1 / updatesPerSecond);
                }
                else
                {
                    yield return null;
                }
            }
        }

        public void SetRegenRate(float newRegen) => regenRate = newRegen;
    }
}