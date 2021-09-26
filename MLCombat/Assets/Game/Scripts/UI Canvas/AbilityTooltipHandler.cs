using UnityEngine;
using System.Collections;
using MiddleAges.Database;
using TMPro;
using UnityEngine.EventSystems;

namespace MiddleAges.UI
{
    public class AbilityTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject tooltip;
        public TextMeshProUGUI tooltipText;
        public int actionBarNumber;

        private Abilities abilities;
        private AbilityData ability;


        private void Start()
        {
            abilities = GameObject.FindGameObjectWithTag("Player").GetComponent<Abilities>();
            ability = abilities.GetAbilityBySlotNumber(actionBarNumber);
            SetTooltipText();
        }

        private void SetTooltipText()
        {
            tooltipText.text = ability.Tooltip;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.SetActive(false);
        }
    }
}