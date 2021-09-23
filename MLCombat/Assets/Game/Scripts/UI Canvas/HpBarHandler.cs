using UnityEngine;

namespace MiddleAges.UI
{
    public class HpBarHandler : ResourceBarHandler
    {
        void Start()
        {
            resource = stats.GetHealthPoints();
        }
    }
}