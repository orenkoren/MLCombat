using MiddleAges.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MiddleAges.Combat
{
    public class Flying : AbilityExecutor
    {
        public float maintainHeight;
        public float ascensionTime;
        public float descensionTime;
        public float flightDuration;
        public string animatorDuration;

        private NavMeshAgent agent;
        private Animator animator;
        private float currentFlightDuration;
        private bool shouldDescend;

        protected override void Start()
        {
            base.Start();
            agent = GetComponentInParent<NavMeshAgent>();
            animator = gameObject.transform.parent.gameObject.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!isAbilityActive) return;
            currentFlightDuration -= Time.deltaTime;
            animator.SetFloat(animatorDuration, currentFlightDuration);
            if (currentFlightDuration <= descensionTime && shouldDescend)
            {
                StartCoroutine(Descend());
                shouldDescend = false;
            }
        }

        protected override void ApplyAbility(object sender, AbilityEventArgs eventArgs)
        {
            if (eventArgs.AbilityName != ability.name) return;
            base.ApplyAbility(sender, eventArgs);
            StartCoroutine(Ascend());
            shouldDescend = true;
            currentFlightDuration = flightDuration;
        }

        protected override void FinishAbility(object sender, AbilityEventArgs eventArgs)
        {
            if (eventArgs.AbilityName != ability.name) return;
            base.FinishAbility(sender, eventArgs);
        }

        private IEnumerator Ascend()
        {
            float elapsedTime = 0;
            float currentOffset = agent.baseOffset;
            while (elapsedTime < ascensionTime)
            {
                agent.baseOffset = Mathf.Lerp(currentOffset, maintainHeight, elapsedTime / ascensionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Descend()
        {
            float elapsedTime = 0;
            float currentOffset = agent.baseOffset;
            while (elapsedTime < descensionTime)
            {
                agent.baseOffset = Mathf.Lerp(currentOffset, 0, elapsedTime / descensionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}