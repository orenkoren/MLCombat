using MiddleAges.Database;
using MiddleAges.Events;
using System;
using UnityEngine;

namespace MiddleAges.Questing
{
    public class QuestManager : MonoBehaviour
    {
        private void Awake()
        {
            GlobalEvents.AddGameEventsListeners += AddNewGameEvent;
            GlobalEvents.RemoveGameEventsListeners += RemoveGameEvent;
            GlobalEvents.PickedItemListeners += PickedItem;
        }

        private void AddNewGameEvent(object sender, string e)
        {
            ((GameEvents)sender).DeathListeners += SomethingDied;
        }

        private void RemoveGameEvent(object sender, string e)
        {
            ((GameEvents)sender).DeathListeners -= SomethingDied;
        }

        private void PickedItem(object sender, GameObject item)
        {
            foreach (var quest in Quests.questsData.Values)
            {
                if(quest.Objective.name == item.name && quest.type == QuestType.Pickups && quest.state == QuestState.InProgress)
                {
                    Quests.IncrementObjectiveCount(quest.Name);
                    if (Quests.GetQuestState(quest.Name) == QuestState.Completed)
                        CompleteQuest(quest);
                    Debug.Log(quest.QuestGiver.gameObject.name);
                    GlobalEvents.FireQuestUpdate(quest.QuestGiver.gameObject, quest); // should this be inside Quests.IncrementKillCount?
                }
            }
        }

        private void SomethingDied(object sender, AbilityEventArgs eventArgs)
        {
            string senderName = ((GameObject)sender).name;

            foreach (var quest in Quests.questsData.Values)
            {
                if (quest.Objective.name == senderName && quest.type == QuestType.KillQuest && quest.state == QuestState.InProgress)
                {
                    Quests.IncrementObjectiveCount(quest.Name);
                    if (Quests.GetQuestState(quest.Name) == QuestState.Completed)
                        CompleteQuest(quest);
                    GlobalEvents.FireQuestUpdate(quest.QuestGiver.gameObject, quest); // should this be inside Quests.IncrementKillCount?
                }
            }
        }

        private void CompleteQuest(QuestData questData)
        {
            GlobalEvents.FireQuestStateChanged(questData.QuestGiver.gameObject, questData);
            Debug.Log("quest complete");
        }

        private void OnDestroy()
        {
            GlobalEvents.AddGameEventsListeners -= AddNewGameEvent;
            GlobalEvents.RemoveGameEventsListeners -= RemoveGameEvent;
            GlobalEvents.PickedItemListeners -= PickedItem;
        }
    }
}