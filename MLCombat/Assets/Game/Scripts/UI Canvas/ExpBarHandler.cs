using MiddleAges.Resources;
using UnityEngine;

namespace MiddleAges.UI
{
    public class ExpBarHandler : ResourceBarHandler
    {
        void Start()
        {
            resource = ((PlayerStats)stats).GetExperiencePoints();
        }

        protected override void UpdateResourceBarAccordingToCurrentResourcePoints(int currentRp)
        {
            base.UpdateResourceBarAccordingToCurrentResourcePoints(currentRp);
            resourceText.text += " (" + ((float)currentRp * 100 / resource.GetMaxResourcePoints()).ToString("0") + "% ) ";
        }
    }
}