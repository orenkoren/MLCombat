using Assets.Game.Scripts.Combat;
using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class AbilityCombos : MonoBehaviour
    {
        public AbilityCombo[] Combos;
        public Animator animator;

        private GameEvents events;
        private PaladinCombat combat;
        private ChannelAbilities channelManager;
        private AbilityConditionManager conditionManager;
        private bool isAgent;
        [HideInInspector] public KeyCode currentKeyAgent;

        void Start()
        {
            isAgent = LearningManager.Instance.isAgent;
            events = GetComponent<GameEvents>();
            combat = GetComponent<PaladinCombat>();
            channelManager = GetComponent<ChannelAbilities>();
            conditionManager = GetComponent<AbilityConditionManager>();
            events.AbilityTriggeredListeners += ApplyCombo;
        }

        private void ApplyCombo(object sender, AbilityEventArgs abilityArgs)
        {
            foreach (var combo in Combos)
            {
                for (int i = 0; i < combo.Abilities.Length; i++)
                {
                    if (combo.Abilities[i].Ability.name == abilityArgs.AbilityName)
                    {
                        if (i == combo.Abilities.Length - 1)
                            combo.ComboStage = 0;
                        else
                        {
                            combo.ComboStage = i + 1;
                        }
                    }
                }
            }
        }

        void Update()
        {
            foreach (var combo in Combos)
            {
                if (isAgent ? currentKeyAgent == combo.ComboKey : Input.GetKeyDown(combo.ComboKey))
                {
                    print("using combo " + combo.ComboKey);
                    AbilityNameWithTrigger abilityToTrigger = combo.Abilities[combo.ComboStage];
                    if (combo.ComboStage == 0 && (conditionManager.IsAbilityUseable(abilityToTrigger.Ability.AbilityInfo) ||
                        (channelManager.IsCurrentlyChanneling() &&
                        channelManager.GetCurrentlyChanneledAbility().Name == combo.Abilities[combo.Abilities.Length - 1].Ability.AbilityInfo.Name)))
                    {
                        QueueNextStage(combo, abilityToTrigger);
                    }
                    if (combo.ComboStage > 0)
                    {
                        QueueNextStage(combo, abilityToTrigger);
                    }
                }
                else
                {
                    combo.currentTimer += Time.deltaTime;
                }
                if (combo.ComboStage > 0 && combo.currentTimer > combo.Interval)
                    EndCombo(combo);
            }
        }

        private void QueueNextStage(AbilityCombo combo, AbilityNameWithTrigger abilityToTrigger)
        {
            events.FireAbilityQueued(this, new AbilityEventArgs(abilityToTrigger.Ability.AbilityInfo.Name,
                                                                new AnimationEventArgs(abilityToTrigger.TriggerName)));
            combo.currentTimer = 0f;
        }

        public bool IsAbilityInCombo(string AbilityName)
        {
            foreach (var combo in Combos)
            {
                foreach (var ability in combo.Abilities)
                {
                    if (ability.Ability.AbilityInfo.Name == AbilityName) return true;
                }
            }
            return false;
        }

        private void EndCombo(AbilityCombo combo)
        {
            combo.currentTimer = 0f;
            combo.ComboStage = 0;
            foreach (var ability in combo.Abilities)
            {
                animator.ResetTrigger(ability.TriggerName);
            }
        }

        private void OnDestroy()
        {
            events.AbilityTriggeredListeners -= ApplyCombo;
        }

    }

    [System.Serializable]
    public class AbilityCombo
    {
        public AbilityNameWithTrigger[] Abilities;
        public KeyCode ComboKey;
        public float Interval;
        [HideInInspector]
        public int ComboStage;
        [HideInInspector]
        public float currentTimer;

        public AbilityCombo(AbilityNameWithTrigger[] abilities, KeyCode comboKey, float interval)
        {
            Abilities = abilities;
            ComboKey = comboKey;
            Interval = interval;
            currentTimer = interval;
        }
    }

}