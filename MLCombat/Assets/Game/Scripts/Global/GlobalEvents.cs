using MiddleAges.Database;
using System;
using UnityEngine;

namespace MiddleAges.Events
{
    public static class GlobalEvents
    {
        public static event EventHandler<int> ExperienceGainedListeners;
        public static void FireExperienceGained(object sender, int amountGained) => ExperienceGainedListeners.Invoke(sender, amountGained);

        public static event EventHandler<GameObject> PickedItemListeners;
        public static void FireItemPicked(object sender, GameObject item) => PickedItemListeners.Invoke(sender, item);

        public static event EventHandler<string> AddGameEventsListeners;
        public static void FireGameEventsAdded(object sender, string name) => AddGameEventsListeners.Invoke(sender, name);

        public static event EventHandler<string> RemoveGameEventsListeners;
        public static void FireGameEventsRemoved(object sender, string name) => RemoveGameEventsListeners?.Invoke(sender, name);

        public static event EventHandler<GameObject> InteractListeners;
        public static void FireInteract(object sender, GameObject gameObj) => InteractListeners.Invoke(sender, gameObj);

        public static event EventHandler<GameObject> InteractEnteredRangeListeners;
        public static void FireInteractEnteredRange(object sender, GameObject gameObj) => InteractEnteredRangeListeners.Invoke(sender, gameObj);

        public static event EventHandler<GameObject> InteractLeftRangeListeners;
        public static void FireInteractLeftRange(object sender, GameObject gameObj) => InteractLeftRangeListeners.Invoke(sender, gameObj);

        public static event EventHandler<QuestData> QuestUpdatesListeners;
        public static void FireQuestUpdate(object sender, QuestData questData) => QuestUpdatesListeners.Invoke(sender, questData);
        
        public static event EventHandler<QuestData> QuestStateChangedListeners;
        public static void FireQuestStateChanged(object sender, QuestData questData) => QuestStateChangedListeners.Invoke(sender, questData);

        public static event EventHandler<LoadEventArgs> LoadGameListeners;
        public static void FireLoadGame(object sender, LoadEventArgs loadArgs) => LoadGameListeners.Invoke(sender, loadArgs);

        public static event EventHandler<AbilityEventArgs> KeybindChangedListeners;
        public static event EventHandler<AbilityEventArgs> AbilityActionSlotChangedListeners;

        public static void FireKeybindChanged(object sender, AbilityEventArgs abilityArgs) => KeybindChangedListeners.Invoke(sender, abilityArgs);
        public static void FireAbilityActionSlotChanged(object sender, AbilityEventArgs abilityArgs) => AbilityActionSlotChangedListeners.Invoke(sender, abilityArgs);

        public static event EventHandler<TimeEventArgs> timeSlowListeners;
        public static void FireTimeSlow(object sender, TimeEventArgs slowDownTo) => timeSlowListeners?.Invoke(sender, slowDownTo);
    }

    public class LoadEventArgs
    {
        public bool IsNewGame { get; set; }
        public LoadEventArgs(bool isNewGame = true)
        {
            IsNewGame = isNewGame;
        }
    }

    public class TimeEventArgs
    {
        public bool IsNormal;
        public float SlowOrRestoreAmount;

        public TimeEventArgs(bool isNormal, float slowOrRestoreAmount)
        {
            IsNormal = isNormal;
            SlowOrRestoreAmount = slowOrRestoreAmount;
        }
    }
}
