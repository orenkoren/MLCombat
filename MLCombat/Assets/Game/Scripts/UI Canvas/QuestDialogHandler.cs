using MiddleAges.Database;
using MiddleAges.Events;
using System.Collections;
using TMPro;
using UnityEngine;

namespace MiddleAges.UI
{
    public class QuestDialogHandler : IngameMenu
    {
        public TextMeshProUGUI dialogText;
        public TextMeshProUGUI completionDialogText;
        public GameObject accpetButton;
        public GameObject completeButton;

        private Coroutine printDialogCoroutine;
        private string currentDialog = "";
        private QuestDialogStatus status = QuestDialogStatus.DialogStarted;
        private QuestData quest;
        protected override void Start()
        {
            base.Start();
            GlobalEvents.InteractListeners += UpdateQuestDialog;
            GlobalEvents.InteractLeftRangeListeners += PlayerLeftRange;
            completeButton.SetActive(false);
            completionDialogText.text = "";
        }

        public override void Toggle()
        {
            {
                menuTexture.SetActive(!menuTexture.activeSelf);
                IsClosed = !IsClosed;
            }
        }

        private void PlayerLeftRange(object sender, GameObject obj)
        {
            status = QuestDialogStatus.DialogStarted;
            currentDialog = "";
            if (printDialogCoroutine != null)
            {
                StopCoroutine(printDialogCoroutine);
            }

            Close();
        }

        private void UpdateQuestDialog(object sender, GameObject gameObj)
        {
            quest = Quests.GetQuestByQuestGiver(gameObj.name);
            if (quest == null || quest.state == QuestState.InProgress) return;
            switch (status)
            {
                case QuestDialogStatus.DialogStarted:
                    menuTexture.SetActive(true);
                    printDialogCoroutine = StartPrintingDialog(quest);
                    status = QuestDialogStatus.CurrentlyPrintingDialog;
                    break;

                case QuestDialogStatus.CurrentlyPrintingDialog:
                    StopCoroutine(printDialogCoroutine);
                    printDialogCoroutine = null;
                    if (quest.state == QuestState.Available)
                        dialogText.text = quest.Description;
                    else
                        completionDialogText.text = quest.CompletionDescription;
                    status = QuestDialogStatus.FinishedPrintingDialog;
                    break;

                case QuestDialogStatus.FinishedPrintingDialog:
                    GlobalEvents.FireQuestStateChanged(gameObj, quest);
                    Toggle();
                    status = QuestDialogStatus.DialogStarted;
                    break;
            }
        }

        private Coroutine StartPrintingDialog(QuestData questData)
        {
            if (questData.state == QuestState.Available)
            {
                return StartCoroutine(PrintDialog(questData.Description.Length, questData.Description, dialogText));
            }
            //should we also add a dialog for interacting with the NPC while the quest is in progress?
            if(questData.state == QuestState.Completed)
            {
                PrepareCompletionUI();
                return StartCoroutine(PrintDialog(questData.CompletionDescription.Length, questData.CompletionDescription, completionDialogText));
            }
            return null;
        }

        private void PrepareCompletionUI()
        {
            accpetButton.SetActive(false);
            dialogText.text = "";
            currentDialog = "";
            completeButton.SetActive(true);
        }

        private IEnumerator PrintDialog(int length, string description, TextMeshProUGUI dialogText)
        {
            for (var i = 0; i < length; i++)
            {
                currentDialog += description[i];
                dialogText.text = currentDialog;
                yield return new WaitForSeconds(0.03333f);
            }

            status = QuestDialogStatus.FinishedPrintingDialog;
        }

        private void OnDestroy()
        {
            GlobalEvents.InteractListeners -= UpdateQuestDialog;
            GlobalEvents.InteractLeftRangeListeners -= PlayerLeftRange;
        }
    }
}
