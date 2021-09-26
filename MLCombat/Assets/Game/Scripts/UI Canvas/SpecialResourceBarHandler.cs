using MiddleAges.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleAges.UI
{
    public class SpecialResourceBarHandler : MonoBehaviour
    {
        public Resource resource;
        public int pointNumber;
        public Image image;

        private float currentPoints;

        void Update()
        {
            currentPoints = resource.GetCurrentResourcePoints();
            UpdateResourceBarAccordingToCurrentResourcePoints(currentPoints - (pointNumber - 1));
        }

        protected virtual void UpdateResourceBarAccordingToCurrentResourcePoints(float currentRp)
        {
            if (currentRp > 0)
                image.fillAmount = currentRp;
            else
                image.fillAmount = 0;
        }
    }
}