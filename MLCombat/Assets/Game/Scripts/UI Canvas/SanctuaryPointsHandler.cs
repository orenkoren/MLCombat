using MiddleAges.Resources;

namespace MiddleAges.UI
{
    public class SanctuaryPointsHandler : SpecialResourceBarHandler
    {
        void Start()
        {
            resource = FindObjectOfType<SanctuaryPoints>();
        }
    }
}