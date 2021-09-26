using MiddleAges.Database;
using MiddleAges.Events;
using System;
using UnityEngine;

namespace MiddleAges.Resources
{
    public class ExperienceManager : MonoBehaviour
    {
        void Awake()
        {
            GlobalEvents.AddGameEventsListeners += AddNewGameEvent;
            GlobalEvents.RemoveGameEventsListeners += RemoveGameEvent;
            GlobalEvents.QuestStateChangedListeners += AwardQuestExperience;
        }

        private static void AwardQuestExperience(object sender, QuestData questData)
        {
            if (questData.state == QuestState.Delivered)
                GlobalEvents.FireExperienceGained(sender, questData.ExperienceReward);
        }

        private static void AddNewGameEvent(object sender, string e)
        {
            ((GameEvents)sender).DeathListeners += AwardDeathExperience;
        }

        private void RemoveGameEvent(object sender, string e)
        {
            ((GameEvents)sender).DeathListeners -= AwardDeathExperience;
        }

        private static void AwardDeathExperience(object sender, AbilityEventArgs e)
        {
            if (((GameObject)sender).GetComponent<AIStats>() == null) return;
            int expReward = ((GameObject)sender).GetComponent<AIStats>().ExperienceReward;// TODO: scriptable object for experience reward etc?
            GlobalEvents.FireExperienceGained(sender, expReward);
        }

        private void OnDestroy()
        {
            GlobalEvents.AddGameEventsListeners -= AddNewGameEvent;
            GlobalEvents.RemoveGameEventsListeners -= RemoveGameEvent;
            GlobalEvents.QuestStateChangedListeners -= AwardQuestExperience;
        }
    }
}