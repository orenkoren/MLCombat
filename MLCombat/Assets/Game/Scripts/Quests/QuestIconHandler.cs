using MiddleAges.Database;
using MiddleAges.Entities;
using MiddleAges.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleAges.Questing
{
    public class QuestIconHandler : MonoBehaviour
    {
        public Image questBeforeInteractionIcon;
        public Image questInProgressIcon;
        public Image questCompleteIcon;
        public GameObject questGiver;

        void Start()
        {
            GlobalEvents.QuestStateChangedListeners += ChangeQuestIconOnQuestStateChange;
            ChangeQuestIconOnStart(Quests.GetQuestByQuestGiver(questGiver.name));
        }

        private void ChangeQuestIconOnStart(QuestData questData)
        {
            ChangeQuestIcon(questData);
        }

        private void ChangeQuestIconOnQuestStateChange(object sender, QuestData questData)
        {
            if (sender == null || questData.QuestGiver.name != questGiver.name)
                return;

            ChangeQuestIcon(questData);
        }

        private void ChangeQuestIcon(QuestData questData)
        {
            switch (questData.state)
            {
                case QuestState.Available:
                    questInProgressIcon.gameObject.SetActive(false);
                    questCompleteIcon.gameObject.SetActive(false);
                    questBeforeInteractionIcon.gameObject.SetActive(true);
                    break;
                case QuestState.InProgress:
                    questCompleteIcon.gameObject.SetActive(false);
                    questBeforeInteractionIcon.gameObject.SetActive(false);
                    questInProgressIcon.gameObject.SetActive(true);
                    break;
                case QuestState.Completed:
                    questBeforeInteractionIcon.gameObject.SetActive(false);
                    questInProgressIcon.gameObject.SetActive(false);
                    questCompleteIcon.gameObject.SetActive(true);
                    break;
                case QuestState.Delivered:
                    questBeforeInteractionIcon.gameObject.SetActive(false);
                    questInProgressIcon.gameObject.SetActive(false);
                    questCompleteIcon.gameObject.SetActive(false);
                    break;
            }
        }

        private void OnDestroy()
        {
            GlobalEvents.QuestStateChangedListeners -= ChangeQuestIconOnQuestStateChange;
        }
    }
}

