namespace MiddleAges.Resources
{
    public class AIStats : Stats
    {
        public int Health;
        public float HealthRegen;
        public int ExperienceReward;

        protected override void Awake()
        {
            base.Awake();
            hp.SetMaxResourcePoints(Health);
            hp.SetCurrentResourcePoints(Health);
            hp.SetRegenRate(HealthRegen);
        }

        protected override void Start()
        {
            base.Start();
            entity.animator.SetFloat("HealthPercentage", hp.GetResourcePercentage());
        }
    }
}
