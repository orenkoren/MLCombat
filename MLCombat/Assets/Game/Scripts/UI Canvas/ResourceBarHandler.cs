using MiddleAges.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleAges.UI
{
    public abstract class ResourceBarHandler : MonoBehaviour
    {
        public TextMeshProUGUI resourceText;
        public Image resourceBar;
        public Stats stats;
        public bool isAI;

        protected Resource resource;
        void Update()
        {
            UpdateResourceBarAccordingToCurrentResourcePoints((int)resource.GetCurrentResourcePoints());
        }

        protected virtual void UpdateResourceBarAccordingToCurrentResourcePoints(int currentRp)
        {
            if (isAI && resource.GetCurrentResourcePoints() <= 0)
                resourceBar.transform.parent.gameObject.SetActive(false);

            resourceBar.fillAmount = currentRp / resource.GetMaxResourcePoints();
            if (resourceText != null)
                resourceText.text = currentRp + " / " + resource.GetMaxResourcePoints();
        }
    }
}