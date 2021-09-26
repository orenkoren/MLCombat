using MiddleAges.Database;
using MiddleAges.Events;
using MiddleAges.Saving;
using UnityEngine;

namespace MiddleAges.Resources
{
    public class PlayerStats : Stats, ISaveable
    {
        public Progression progression;
        public int level = 1;

        private ExperiencePoints experience;
        private RetributionPoints retribution;
        private SanctuaryPoints sanctuary;

        protected override void Awake()
        {
            base.Awake();
            retribution = GetComponentInChildren<RetributionPoints>();
            sanctuary = GetComponentInChildren<SanctuaryPoints>();
            UpdateHealthValues();
            UpdateRetributionValues();
            UpdateSanctuaryValues();
            experience = GetComponentInChildren<ExperiencePoints>();
            experience.SetMaxResourcePoints(progression.GetExperience(level));
        }

        public ExperiencePoints GetExperiencePoints() => experience;

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.L))
                Levelup();
            if (Input.GetKeyDown(KeyCode.K))
            {
                foreach (var gameobj in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    gameobj.GetComponent<GameEvents>().FireDeath(gameobj, new AbilityEventArgs("Death", new AnimationEventArgs("Death")));
                }
            }
        }

        public override bool HasEnoughResources(AbilityData abilityData)
        {
            if (abilityData.RetCost > 0 && retribution.GetCurrentResourcePoints() < abilityData.RetCost)
                return false;
            if (abilityData.SancCost > 0 && sanctuary.GetCurrentResourcePoints() < abilityData.SancCost)
                return false;
            return true;
        }

        public void Levelup()
        {
            if (level >= progression.characterClass.maxLevel)
                return;
            ChangeLevel(level + 1);
            UpdateHealthValues();
            experience.SetMaxResourcePoints(progression.GetExperience(level));
        }

        public object CaptureState() => new PlayerData((int)hp.GetMaxResourcePoints(),
                                                     (int)hp.GetCurrentResourcePoints(), level,
                                                     (int)experience.GetCurrentResourcePoints(), (int)experience.GetMaxResourcePoints());

        public void RestoreState(object state)
        {
            PlayerData playerData = (PlayerData)state;
            hp.SetMaxResourcePoints(playerData.MaxHealth);
            hp.SetCurrentResourcePoints(playerData.CurrentHealth);
            experience.SetMaxResourcePoints(playerData.MaxExp);
            experience.SetCurrentResourcePoints(playerData.CurrentExp);
            ChangeLevel(playerData.Level);
        }

        private void ChangeLevel(int newLevel)
        {
            level = newLevel;
            events.FireLevelChanged(this, level);
        }

        private void UpdateHealthValues()
        {
            hp.SetMaxResourcePoints(progression.GetHealth(level));
            hp.SetCurrentResourcePoints(progression.GetHealth(level));
            hp.SetRegenRate(progression.GetHealthRegenRate(level));
        }

        private void UpdateRetributionValues()
        {
            retribution.SetMaxResourcePoints(5);
            retribution.SetCurrentResourcePoints(5);
            retribution.SetRegenRate(0.2f);
        }

        private void UpdateSanctuaryValues()
        {
            sanctuary.SetMaxResourcePoints(5);
            sanctuary.SetCurrentResourcePoints(5);
            sanctuary.SetRegenRate(0.2f);
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public int MaxHealth;
        public int MaxMana;
        public int CurrentHealth;
        public int CurrentMana;
        public int Level;
        public int CurrentExp;
        public int MaxExp;

        public PlayerData(int maxHealth, int currentHealth, int level, int currentExp, int maxExp)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            Level = level;
            CurrentExp = currentExp;
            MaxExp = maxExp;
        }
    }
}
