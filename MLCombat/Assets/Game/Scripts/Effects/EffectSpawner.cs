using MiddleAges.Database;
using MiddleAges.Events;
using MiddleAges.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiddleAges.SpecialEffects
{
    public class EffectSpawner : MonoBehaviour
    {
        public List<EffectData> effects;

        private GameEvents events;
        private GameObject dynamicStunEffect;
        private Queue<DynamicEffect> dynamicEffects;
        private List<ParticleSystem> currentlyPlayingEffects;
        private bool isCurrentlyBlocking = false;

        void Start()
        {
            events = GetComponent<GameEvents>();
            dynamicEffects = new Queue<DynamicEffect>();
            currentlyPlayingEffects = new List<ParticleSystem>();
            events.SpawnEffectPrefabListeners += SpawnEffectPrefab;
            events.DamageTakenListeners += SpawnDamageTakenEffect;
            events.BlockFollowupStateListeners += SpawnBlockEffect;
            events.StunStateListeners += ToggleStunEffect;
            GlobalEvents.timeSlowListeners += SlowEffects;
        }

        private void Update()
        {
            UpdateDynamicEffects();
        }

        private void UpdateDynamicEffects()
        {
            if (!dynamicEffects.Any()) return;
            foreach (var effectData in dynamicEffects)
            {
                UpdateEffectTransform(effectData);
            }
        }
        private void UpdateEffectTransform(DynamicEffect dynamicEffect)
        {
            dynamicEffect.effectGameObject.transform.position = FindEffectDataByEffectObject(dynamicEffect.effectData.Type).EffectLocation.position;
            dynamicEffect.effectGameObject.transform.rotation = FindEffectDataByEffectObject(dynamicEffect.effectData.Type).EffectLocation.rotation;
        }

        private float CalculateEffectiveRange(float range, Transform senderTransform)
        {
            var crosshairTarget = RaycastManager.GetCrosshairTargetPoint();
            float cameraFloorHitDistance = Vector3.Distance(senderTransform.position, crosshairTarget);
            return cameraFloorHitDistance < range ?
                   cameraFloorHitDistance : range;
        }

        private void SpawnDamageTakenEffect(object sender, DamageTakenEventArgs args)
        {
            if (args.AbilityData.IsStun || isCurrentlyBlocking) return;

            CreateAndPlayEffect(FindSpecificEffectToPlay(EffectType.Blood), true);
        }

        private void SpawnBlockEffect(object sender, bool isBlock)
        {
            isCurrentlyBlocking = isBlock;
            if (isBlock)
                CreateAndPlayEffect(FindSpecificEffectToPlay(EffectType.Spark));
        }


        private void ToggleStunEffect(object sender, bool isStunned)
        {
            if (isStunned && !dynamicStunEffect)
            {
                dynamicStunEffect = CreateAndPlayEffect(FindSpecificEffectToPlay(EffectType.Stun), false, true);
            }
            else
            {
                Destroy(dynamicStunEffect);
                dynamicStunEffect = null;
            }
        }
        private GameObject CreateAndPlayEffect(EffectData effectData, bool isDynamic = false, bool requiresCancellation = false)
        {
            var effect = Instantiate(effectData.Effect, effectData.EffectLocation.position,
                effectData.EffectLocation.rotation);
            if (isDynamic)
            {
                dynamicEffects.Enqueue(new DynamicEffect(effectData, effect));
                foreach (var particleSystem in effectData.Effect.GetComponentsInChildren<ParticleSystem>())
                {
                    currentlyPlayingEffects.Add(particleSystem);
                }
                StartCoroutine(DestroyDynamicEffect(effectData));
            }
            else if (requiresCancellation)
            {
                return effect;
            }
            else
            {
                Destroy(effect, effectData.Lifetime);
            }

            return effect;
        }

        private EffectData FindSpecificEffectToPlay(EffectType effectType)
        {
            return effects.Find(effect => effect.Type == effectType);
        }

        private EffectData FindEffectDataByEffectObject(EffectType effectType)
        {
            return effects.Find(effect => effect.Type == effectType);
        }

        IEnumerator DestroyDynamicEffect(EffectData effectData)
        {
            yield return new WaitForSeconds(effectData.Lifetime);
            var effect = dynamicEffects.Dequeue();
            Destroy(effect.effectGameObject, 0);
            foreach (var particleSystem in effectData.Effect.GetComponentsInChildren<ParticleSystem>())
            {
                currentlyPlayingEffects.Remove(particleSystem);
            }
        }

        private class DynamicEffect
        {
            public EffectData effectData;
            public GameObject effectGameObject;

            public DynamicEffect(EffectData effectData, GameObject effectGameObject)
            {
                this.effectData = effectData;
                this.effectGameObject = effectGameObject;
            }
        }

        private void SpawnEffectPrefab(object sender, Effect effect)
        {
            var effectData = effect.EffectInfo;
            Transform senderTransform = ((GameObject)sender).transform;
            float effectiveRange;
            Quaternion effectRotation = new Quaternion();
            if (effectData.OriginPosition == EffectOrigin.Destination)
                effectiveRange = CalculateEffectiveRange(effectData.ForAbility.AbilityInfo.Range, senderTransform);
            else
                effectiveRange = effectData.StartRange;
            if (effectData.RotationType == EffectRotationType.Custom)
                effectRotation = Quaternion.Euler(effectData.EffectRotation);
            else if (effectData.RotationType == EffectRotationType.Sender)
                effectRotation = senderTransform.rotation;
            else if (effectData.RotationType == EffectRotationType.Camera)
            {
                var crosshairTarget = RaycastManager.GetCrosshairTargetPoint();
                effectRotation = Quaternion.LookRotation(crosshairTarget - (senderTransform.position + effectData.EffectOffset));
            }
            var effectClone = Instantiate(effectData.EffectObject, senderTransform.position
                                    + effectData.EffectOffset + senderTransform.forward * effectiveRange,
                                    effectRotation);
            foreach (var particleSystem in effectClone.GetComponentsInChildren<ParticleSystem>())
            {
                currentlyPlayingEffects.Add(particleSystem);
            }
            StartCoroutine(DestroyEffect(effectClone, effectData.Lifetime));
        }

        IEnumerator DestroyEffect(GameObject effectClone, float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(effectClone);
            foreach (var particleSystem in effectClone.GetComponentsInChildren<ParticleSystem>())
            {
                currentlyPlayingEffects.Remove(particleSystem);
            }
        }
        private void SlowEffects(object sender, TimeEventArgs eventArgs)
        {
            if (eventArgs.IsNormal)
            {
                foreach (var effect in currentlyPlayingEffects)
                {
                    var main = effect.main;
                    main.simulationSpeed = 1;
                }
            }
            else
            {
                foreach (var effect in currentlyPlayingEffects)
                {
                    var main = effect.main;
                    main.simulationSpeed = eventArgs.SlowOrRestoreAmount;
                }
            }
        }
        private void OnDestroy()
        {
            events.DamageTakenListeners -= SpawnDamageTakenEffect;
            events.BlockFollowupStateListeners -= SpawnBlockEffect;
            events.StunStateListeners -= ToggleStunEffect;
        }
    }
}
