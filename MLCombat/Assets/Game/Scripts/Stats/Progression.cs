using UnityEngine;

namespace MiddleAges.Resources
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        public ProgressionCharacterClass characterClass;


        public int GetHealth(int level)
        {
            if (level <= characterClass.maxLevel)
                return characterClass.healthLevels[level - 1];
            return characterClass.healthLevels[characterClass.maxLevel - 1];
        }
        
        public float GetHealthRegenRate(int level)
        {
            if (level <= characterClass.maxLevel)
                return characterClass.healthRegenLevels[level - 1];
            return characterClass.healthRegenLevels[characterClass.maxLevel - 1];
        }

        public int GetExperience(int level) => characterClass.experienceLevels[level - 1];

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public int[] healthLevels;
            public float[] healthRegenLevels;
            public int[] experienceLevels;
            public int maxLevel;
        }
    }
}