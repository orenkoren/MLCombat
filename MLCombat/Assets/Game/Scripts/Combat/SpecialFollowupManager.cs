using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class SpecialFollowupManager : MonoBehaviour
    {
        public float ResponseTimer;

        private ChannelAbilities channelManager;
        private GameEvents events;
        private float blockTimeCounter;
        private bool hasEnded;

        void Start()
        {
            channelManager = GetComponent<ChannelAbilities>();
            events = GetComponent<GameEvents>();
            events.DamageTakenListeners += BlockResponse;
        }

        void Update()
        {
            if(hasEnded == false && IsBlockResponseActive() == false)
            {
                hasEnded = true;
                events.FireBlockFollowupState(gameObject, false);
            }
            blockTimeCounter += Time.deltaTime;
        }

        private void BlockResponse(object sender, DamageTakenEventArgs eventArgs)
        {
            if (channelManager.GetCurrentlyChanneledAbility() == null) return;
            if (channelManager.GetCurrentlyChanneledAbility().Type == AbilityType.Block && eventArgs.IsHitFromBehind == false)
            {
                blockTimeCounter = 0;
                events.FireBlockFollowupState(gameObject, true);
                hasEnded = false;
            }
        }

        public bool IsBlockResponseActive() => blockTimeCounter <= ResponseTimer;

        private void OnDestroy()
        {
            events.DamageTakenListeners -= BlockResponse;
        }
    }
}