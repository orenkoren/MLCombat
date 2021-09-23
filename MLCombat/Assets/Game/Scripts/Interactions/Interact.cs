using MiddleAges.Events;
using MiddleAges.Manager;
using UnityEngine;

namespace Assets.Game.Scripts.Interactions
{
    public class Interact : MonoBehaviour
    {
        public LayerMask interactableThings;
        public float interactDistance;

        private GameObject thingWithinRange;
        private GameObject hitObject;
        private GameObject emptyObj;

        private void Start()
        {
            emptyObj = new GameObject();
            emptyObj.name = "";
            thingWithinRange = emptyObj;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && thingWithinRange)
            {
                GlobalEvents.FireInteract(this, thingWithinRange);
            }
            InteractRangeHandling();
        }

        private void InteractRangeHandling()
        {
            var isInteracting = RaycastManager.GetCrosshairTarget(interactableThings, out RaycastHit hit);
            if (isInteracting)
                hitObject = hit.transform.gameObject;
            else
                hitObject = emptyObj;

            if (thingWithinRange.GetInstanceID() != hitObject.GetInstanceID())
            {
                GlobalEvents.FireInteractLeftRange(this, thingWithinRange);
                GlobalEvents.FireInteractEnteredRange(this, hitObject);
                thingWithinRange = hitObject;
            }
        }
    }
}