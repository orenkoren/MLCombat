using System.Collections.Generic;
using System.Linq;

namespace MiddleAges.Database
{
    public static class Quests
    {
        #region Public Properties

        public static Dictionary<string, QuestData> questsData;

        public static void AddQuest(QuestData questInfo)
        {
            questsData.Add(questInfo.Name, questInfo);
        }

        #endregion Public Properties

        #region Constructor

        static Quests()
        {
            questsData = new Dictionary<string, QuestData>();
        }

        public static void SetQuestState(string questName, QuestState newState)
        {
            questsData[questName].state = newState;
        }

        public static void IncrementObjectiveCount(string questName)
        {
            QuestData quest = questsData[questName];
            if (quest.CurrentObjectiveCount < quest.NeededObjectiveCount)
                quest.CurrentObjectiveCount++;
            if (quest.CurrentObjectiveCount == quest.NeededObjectiveCount)
                quest.state = QuestState.Completed;
        }

        public static QuestState GetQuestState(string questName)
        {
            return questsData[questName].state;
        }

        public static List<QuestData> GetAllInProgress() //rename
        {
            return questsData.Values.Where((quest) => quest.state == QuestState.InProgress || quest.state == QuestState.Completed).ToList();
        }
        public static List<QuestData> GetAllQuestData()
        {
            return questsData.Values.ToList();
        }

        public static QuestData GetQuestByQuestGiver(string giverName)
        {
            return questsData.FirstOrDefault((quest) => quest.Value.QuestGiver.name == giverName).Value;
        }

        #endregion Constructor 
    }
}