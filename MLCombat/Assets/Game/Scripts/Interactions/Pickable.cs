using MiddleAges.Events;
using UnityEngine;

namespace Assets.Game.Scripts.Interactions
{
    public class Pickable : MonoBehaviour
    {
        private bool destroyPending;
        // Use this for initialization
        void Start()
        {
            GlobalEvents.InteractListeners += Pickup;
        }

        private void Pickup(object sender, GameObject gameObj)
        {
            if (gameObject.GetInstanceID() == gameObj.GetInstanceID() && !destroyPending)
            {
                destroyPending = true;
                Debug.Log(name);
                Destroy(gameObject, 1);
                GlobalEvents.FireItemPicked(this, gameObject);
            }
        }

        private void OnDestroy()
        {
            GlobalEvents.InteractListeners -= Pickup;
        }
    }
}