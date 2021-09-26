using UnityEngine;

namespace MiddleAges.Database
{
    public class DatabaseLoader : MonoBehaviour
    {
        public Quest[] quests;

        public void Awake()
        {
            foreach (var quest in quests)
            {
                Quests.AddQuest(quest.QuestInfo);
            }
        }
    }
}