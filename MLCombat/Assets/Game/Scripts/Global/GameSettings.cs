using UnityEngine;

namespace MiddleAges.Events
{

    public class GameSettings : MonoBehaviour
    {
        public GameEvents events;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (Cursor.visible)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }

            //if(Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    Abilities.SetKeybind("PaladinHeavyAttack", KeyCode.E);
            //    events.FireKeybindChanged(this, new AbilityEventArgs("PaladinHeavyAttack"));
            //}
        }
    }

}
