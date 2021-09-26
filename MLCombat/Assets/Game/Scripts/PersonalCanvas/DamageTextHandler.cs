using MiddleAges.Database;
using MiddleAges.Events;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MiddleAges.PersonalCanvas
{
    public class DamageTextHandler : MonoBehaviour
    {
        public float combatTextDisappearTime;
        public TextMeshProUGUI floatingDamage;
        public TextMeshProUGUI floatingHeal;
        public GameEvents events;
        public Abilities abiltiies;

        private Queue<CombatTextWithTimer> currentDamage;
        private Queue<CombatTextWithTimer> currentHeal;
        private AbilityData currentlyChanneledAbility;


        void Start()
        {
            currentDamage = new Queue<CombatTextWithTimer>();
            currentHeal = new Queue<CombatTextWithTimer>();
            events.DamageTakenListeners += AddDamageCombatText;
            events.HealingTakenListeners += AddHealingCombatText;
            events.AbilityTriggeredListeners += AddCurrentlyChanneledAbilityText;
            events.AbilityEndedListeners += RemoveCurrentlyChanneledAbilityText;
        }

        void Update()
        {
            UpdateDamageListTimers();
        }

        private void AddDamageCombatText(object sender, DamageTakenEventArgs damageArgs)
        {
            TextMeshProUGUI damageText = InstantiateDamageTextObject(damageArgs.DamageAmount, damageArgs, damageArgs.DamageType);
            currentDamage.Enqueue(new CombatTextWithTimer(damageText, 0));

        }

        private void AddHealingCombatText(object sender, HealingTakenEventArgs args)
        {
            TextMeshProUGUI healText = InstantiateHealingTextObject(args.HealingAmount, args.IsCrit);
            currentHeal.Enqueue(new CombatTextWithTimer(healText, 0));
        }

        private TextMeshProUGUI InstantiateHealingTextObject(int healingAmount, bool isCrit)
        {
            TextMeshProUGUI heal = Instantiate(floatingHeal, transform, false);
            heal.text = "+" + healingAmount;
            if (isCrit)
            {
                heal.fontSize += 0.25f;
                heal.text += " (Crit)";
            }
            heal.color = Color.green;
            return heal;
        }

        private TextMeshProUGUI InstantiateDamageTextObject(int finalAmount, DamageTakenEventArgs eventArgs, AbilityType abilityType)
        {
            TextMeshProUGUI damage = Instantiate(floatingDamage, transform, false); // TODO: use flyweight here
            damage.text = finalAmount.ToString();
            damage.fontSize = Vector3.Distance(transform.position, Camera.main.transform.position) / 15; // TODO: use jobs or precalculate it somewhere else
            if (currentlyChanneledAbility != null && currentlyChanneledAbility.DamageText != "" && !eventArgs.IsHitFromBehind)
                damage.text += " (" + currentlyChanneledAbility.DamageText + ")";
            if (eventArgs.AbilityData?.DamageText != "")
                damage.text += " (" + eventArgs.AbilityData.DamageText + ")";
            if (eventArgs.IsMiss)
                damage.text = "Miss";
            if (eventArgs.IsCrit)
            {
                damage.fontSize *= 1.5f;
                damage.text += " (Crit)";
            }
            if (abilityType == AbilityType.Spell)
                damage.color = Color.yellow;
            return damage;
        }

        private void UpdateDamageListTimers()
        {
            UpdateDamageTimers();
            UpdateCombatQueues(currentDamage);
            UpdateCombatQueues(currentHeal);
        }

        private void UpdateCombatQueues(Queue<CombatTextWithTimer> queue)
        {
            if (queue.Count != 0 && queue.Peek().CombatTextTime > combatTextDisappearTime)
            {
                Destroy(queue.Peek().CombatText.gameObject);
                queue.Dequeue();
            }
        }

        private void UpdateDamageTimers()
        {
            foreach (CombatTextWithTimer damage in currentDamage)
                damage.CombatTextTime += Time.deltaTime;
        }

        private void RemoveCurrentlyChanneledAbilityText(object sender, AbilityEventArgs abilityArgs)
        {
            if (abiltiies.GetAbility(abilityArgs.AbilityName).IsChanneled)
                currentlyChanneledAbility = null;
        }

        private void AddCurrentlyChanneledAbilityText(object sender, AbilityEventArgs abilityArgs)
        {
            if (abiltiies.GetAbility(abilityArgs.AbilityName).IsChanneled)
                currentlyChanneledAbility = abiltiies.GetAbility(abilityArgs.AbilityName);
        }

        private void OnDestroy()
        {
            events.DamageTakenListeners -= AddDamageCombatText;
            events.HealingTakenListeners -= AddHealingCombatText;
            events.AbilityTriggeredListeners -= AddCurrentlyChanneledAbilityText;
            events.AbilityEndedListeners -= RemoveCurrentlyChanneledAbilityText;
        }
    }

    public class CombatTextWithTimer
    {
        public TextMeshProUGUI CombatText { get; set; }
        public float CombatTextTime { get; set; }

        public CombatTextWithTimer(TextMeshProUGUI combatText, int combatTextTime)
        {
            CombatText = combatText;
            CombatTextTime = combatTextTime;
        }
    }
}