using MiddleAges.Events;

namespace MiddleAges.Resources
{
    public class ExperiencePoints : Resource
    {
        private PlayerStats stats;

        void Start()
        {
            stats = GetComponentInParent<PlayerStats>();
            GlobalEvents.ExperienceGainedListeners += GainExperience;
        }

        public override void IncResource(float amount)
        {
            currentResourcePoints += amount;
        }

        private void GainExperience(object sender, int expAmount)
        {
            if (LearningManager.Instance.isAgent == false)
            {
                int expInLevel = stats.progression.GetExperience(stats.level);
                IncResource(expAmount);
                while (GetCurrentResourcePoints() >= expInLevel && stats.level < stats.progression.characterClass.maxLevel)
                {
                    print("level up");
                    stats.Levelup();
                    SafelyDecResource(expInLevel);
                    expInLevel = stats.progression.GetExperience(stats.level);
                }
                if (stats.level == stats.progression.characterClass.maxLevel &&
                    GetCurrentResourcePoints() >= stats.progression.GetExperience(stats.level))
                    SetCurrentResourcePoints(stats.progression.GetExperience(stats.level));
            }
        }

    }
}
