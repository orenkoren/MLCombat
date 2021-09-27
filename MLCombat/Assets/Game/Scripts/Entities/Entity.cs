using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Entities
{
    public class Entity : MonoBehaviour
    {
        public Animator animator;
        public LayerMask groundMask;
        public float groundDistance;
        public Transform groundCheck;
        public Rigidbody rb;
        public GameEvents events;

        protected void Start()
        {
            if (LearningManager.Instance.isAgent == false)
            {
                events.DeathListeners += ApplyDeath;
                events.ResurrectionListeners += ApplyRessurection;
            }
        }

        private bool isAlive = true;

        public bool IsAlive() => isAlive;

        public virtual void ApplyDeath(object sender, AbilityEventArgs arg)
        {
            print("dieing");
            isAlive = false;
        }

        public void ApplyRessurection(object sender, int placeHolder)
        {
            print("applying ress");
            isAlive = true;
        }

        private void OnDestroy()
        {
            if (LearningManager.Instance.isAgent == false)
            {
                events.DeathListeners -= ApplyDeath;
                events.ResurrectionListeners -= ApplyRessurection;
            }
        }
    }
}
