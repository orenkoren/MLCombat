using MiddleAges.Database;
using MiddleAges.Entities;
using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Questing
{
    public class QuestGiver : MonoBehaviour
    {
        public GameObject questGiver;
        public Entity player;

        private bool isInteractedWith;
        private void Awake()
        {
            GlobalEvents.InteractListeners += InteractedWith;
            GlobalEvents.QuestStateChangedListeners += UpdateQuest;
        }

        private void InteractedWith(object sender, GameObject thingsWithinRange)
        {
            isInteractedWith = thingsWithinRange == questGiver;
        }

        private void UpdateQuest(object sender, QuestData quest)
        {
            if (questGiver.name == quest.QuestGiver.gameObject.name && isInteractedWith)
            {
                string questName = quest.Name;
                if (Quests.GetQuestState(questName) == QuestState.Available)
                {
                    Quests.SetQuestState(questName, QuestState.InProgress);
                }
                if (Quests.GetQuestState(questName) == QuestState.Completed)
                    Quests.SetQuestState(questName, QuestState.Delivered);
            }

            isInteractedWith = false;
        }

        private void OnDestroy()
        {
            GlobalEvents.InteractListeners -= InteractedWith;
            GlobalEvents.QuestStateChangedListeners -= UpdateQuest;
        }
    }
}