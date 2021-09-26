using MiddleAges.Saving;
using UnityEngine;

namespace MiddleAges.UI
{
    public class IngameMenu : Menu
    {
        protected virtual void Start()
        {
            IsClosed = true;
            menuTexture.SetActive(false);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Toggle();
            }
        }
        public override void Toggle()
        {
            {
                Time.timeScale = 1 - Time.timeScale;
                menuTexture.SetActive(!menuTexture.activeSelf);
                ToggleCursor(!Cursor.visible);
                IsClosed = !IsClosed;
            }
        }

        public void Close()
        {
            menuTexture.SetActive(false);
            IsClosed = true;
        }

        public void ExitToMainMenu()
        {
            Toggle();
            GetComponent<SceneLoader>().StartLoadingScene("MainMenu");
        }
    }
}
