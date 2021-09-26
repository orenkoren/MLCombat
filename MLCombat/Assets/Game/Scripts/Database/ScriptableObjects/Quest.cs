using UnityEngine;
using UnityEditor;

namespace MiddleAges.Database
{
    [CreateAssetMenu(fileName = "Quests", menuName = "Database/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        public QuestData QuestInfo;
    }
}