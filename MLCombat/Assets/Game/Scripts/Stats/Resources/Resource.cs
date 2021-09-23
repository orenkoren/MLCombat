using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleAges.Resources
{
    public class Resource : MonoBehaviour
    {
        protected float currentResourcePoints;
        protected float maxResourcePoints;

        public bool SafelyDecResource(float amount)
        {
            if (currentResourcePoints >= amount)
            {
                currentResourcePoints -= amount;
                return true;
            }
            return false;
        }

        public void ForciblyDecResource(float amount)
        {
            if (!SafelyDecResource(amount))
                currentResourcePoints = 0;
        }

        public virtual void IncResource(float amount)
        {
            if (currentResourcePoints + amount >= maxResourcePoints)
            {
                currentResourcePoints = maxResourcePoints;
            }
            else
            {
                currentResourcePoints += amount;
            }
        }
        
        public bool IsEmpty() => currentResourcePoints == 0;
        public float GetCurrentResourcePoints() => currentResourcePoints;
        public void SetCurrentResourcePoints(float current) => currentResourcePoints = current;
        public float GetMaxResourcePoints() => maxResourcePoints;
        public void SetMaxResourcePoints(float max) => maxResourcePoints = max;
        public float GetResourcePercentage() => currentResourcePoints / maxResourcePoints * 100;
        
    }
}