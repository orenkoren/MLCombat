using MiddleAges.Database;
using MiddleAges.Events;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestLogHandler : MonoBehaviour
{
    public TextMeshProUGUI[] questEntries;

    private List<QuestData> questList;

    void Start()
    {
        questList = Quests.GetAllInProgress();
        UpdateQuestLog(null, null);
        GlobalEvents.QuestStateChangedListeners += UpdateQuestList;
        GlobalEvents.QuestUpdatesListeners += UpdateQuestLog;
    }

    private void UpdateQuestList(object sender, QuestData quest)
    {
        if (quest.state == QuestState.InProgress)
        {
            questList.Insert(0, quest);
        }

        if (quest.state == QuestState.Delivered)
        {
            questList.Remove(quest);
        }

        UpdateQuestLog(null, null);
    }

    void UpdateQuestLog(object sender, QuestData questData)
    {
        for (int i = 0; i < questEntries.Length; i++)
        {
            if (i < questList.Count)
            {
                questEntries[i].text = questList.ElementAt(i).QuestLogDescription + " " +
                                       questList.ElementAt(i).CurrentObjectiveCount + "/" +
                                       questList.ElementAt(i).NeededObjectiveCount;
            }
            else
            {
                questEntries[i].text = "";
            }
        }
    }

    private void OnDestroy()
    {
        GlobalEvents.QuestStateChangedListeners -= UpdateQuestList;
        GlobalEvents.QuestUpdatesListeners -= UpdateQuestLog;
    }
}
