using MiddleAges.Database;
using MiddleAges.Events;
using MiddleAges.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleAges.UI
{

    public class AbilityIconHandler : MonoBehaviour
    {
        public GameObject[] abilitySlotIcons;
        public GameEvents events;
        public Abilities abilities;
        public CooldownUtil cooldownUtil;
        public Image[] abilitySlotPanels;

        private Dictionary<int, string> abilitySlotPanelToName;
        private bool allAbilitiesReady;

        public void Start()
        {
            abilitySlotPanelToName = new Dictionary<int, string>();
            allAbilitiesReady = true;
            events.AbilityExecutedListeners += AbilityUsed;
            events.AbilityUsableListeners += AbilityReady;

            for (var i = 0; i < abilitySlotIcons.Length; i++)
            {
                if (abilitySlotIcons[i].GetComponent<Image>())
                {
                    abilitySlotIcons[i].GetComponent<Image>().sprite = abilities.GetAbilityBySlotNumber(i).Icon;
                }
            }
        }

        public void Update()
        {
            if (!abilitySlotPanelToName.Any())
                allAbilitiesReady = true;

            if (!allAbilitiesReady)
            {
                foreach (var ability in abilitySlotPanelToName)
                    AnimateAbilitySlotPanel(ability.Key, ability.Value);
            }
        }

        public void AbilityUsed(object sender, AbilityEventArgs abilityArgs)
        {
            AbilityData abilityData = abilities.GetAbility(abilityArgs.AbilityName);
            if (abilities.AbilityHasActionSlot(abilityData.Name))
            {
                int abilitySlotPanelIndex = abilities.GetActionSlotNumber(abilityData.Name);
                if (!abilitySlotPanelToName.ContainsKey(abilitySlotPanelIndex))
                    abilitySlotPanelToName.Add(abilitySlotPanelIndex, abilityData.Name);
                allAbilitiesReady = false;
            }

        }

        private void AbilityReady(object sender, AbilityEventArgs abilityArgs)
        {
            AbilityData abilityData = abilities.GetAbility(abilityArgs.AbilityName);
            if (abilityData != null && abilities.AbilityHasActionSlot(abilityData.Name))
            {
                int abilitySlotPanelIndex = abilities.GetActionSlotNumber(abilityData.Name);
                if (abilityArgs.IsUsable)
                {
                    UnfillPanel(abilitySlotPanels[abilitySlotPanelIndex]);
                }
                else
                {
                    FillPanel(abilitySlotPanels[abilitySlotPanelIndex]);
                }
                abilitySlotPanelToName.Remove(abilitySlotPanelIndex);
            }

        }

        private void AnimateAbilitySlotPanel(int abilitySlotPanelIndex, string abilityName)
        {
            float currentCooldown = cooldownUtil.GetCurrentCooldown(abilityName);
            float originalCooldown = abilities.GetAbility(abilityName).Cooldown;
            Image abilitySlotPanel = abilitySlotPanels[abilitySlotPanelIndex];
            FillPanel(abilitySlotPanel);
            abilitySlotPanel.fillAmount = currentCooldown / originalCooldown;
        }

        private void UnfillPanel(Image abilitySlotPanel)
        {
            abilitySlotPanel.fillAmount = 0;
        }

        private void FillPanel(Image abilitySlotPanel)
        {
            if (abilitySlotPanel.fillAmount == 0)
                abilitySlotPanel.fillAmount = 1;
        }

        private void OnDestroy()
        {
            events.AbilityExecutedListeners -= AbilityUsed;
            events.AbilityReadyListeners -= AbilityReady;
        }
    }
}
