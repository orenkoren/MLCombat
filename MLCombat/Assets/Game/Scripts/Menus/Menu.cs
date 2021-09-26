using UnityEngine;

namespace MiddleAges.UI
{
    public abstract class Menu : MonoBehaviour
    {
        public GameObject menuTexture;
        protected static bool IsClosed { get; set; }
        public virtual void Toggle(){}
    
        public static void ToggleCursor(bool cursorState)
        {
            Cursor.visible = cursorState;
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }  
}

