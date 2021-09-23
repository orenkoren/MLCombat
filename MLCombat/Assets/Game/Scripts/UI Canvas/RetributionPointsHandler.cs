using MiddleAges.Resources;
using UnityEngine;

namespace MiddleAges.UI
{
    public class RetributionPointsHandler : SpecialResourceBarHandler
    {
        void Start()
        {
            resource = FindObjectOfType<RetributionPoints>();
        }
    }
}