using MiddleAges.Events;
using MiddleAges.Resources;
using TMPro;
using UnityEngine;

namespace MiddleAges.UI
{

    public class StatHandler : MonoBehaviour
    {
        public PlayerStats stats;
        public GameEvents events;
        public TextMeshProUGUI levelText;

        private int currentLevel;

        void Start()
        {
            currentLevel = stats.level;
            levelText.text = currentLevel.ToString();
            events.LevelUpListeners += UpdateLevelup;
        }

        private void UpdateLevelup(object sender, int level)
        {
            currentLevel = level;
            levelText.text = currentLevel.ToString();
        }

        private void OnDestroy()
        {
            events.LevelUpListeners -= UpdateLevelup;
        }
    }
}
