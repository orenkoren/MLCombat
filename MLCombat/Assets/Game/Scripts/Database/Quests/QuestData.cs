using UnityEngine;

namespace MiddleAges.Database
{
    [System.Serializable]
    public class QuestData
    {
        public string Name;
        public QuestState state;
        public QuestType type;
        public GameObject QuestGiver;
        public GameObject Objective;
        public int NeededObjectiveCount;
        public int CurrentObjectiveCount;
        public string Description;
        public string CompletionDescription;
        public string QuestLogDescription;
        public int ExperienceReward;
    }
}