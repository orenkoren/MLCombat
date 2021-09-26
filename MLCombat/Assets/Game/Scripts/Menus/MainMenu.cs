using System.IO;
using MiddleAges.Events;
using MiddleAges.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleAges.UI
{
    public class MainMenu : Menu
    {
        public Image loadingBarFrame;
        public Image loadingBar;
        public TextMeshProUGUI loadingPercent;
        public Button continueGameButton;
        public Button newGameButton;

        void Start()
        {
            string path = Path.Combine(Application.persistentDataPath, "save.sav");
            if (!File.Exists(path))
                continueGameButton.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            loadingBar.gameObject.SetActive(false);
            loadingBarFrame.gameObject.SetActive(false);
        }

        void Update()
        {
            if (SceneLoader.loadMainScene != null)
            {
                loadingPercent.text = (int) (100 * SceneLoader.loadMainScene.progress) + "%";
                loadingBar.fillAmount = SceneLoader.loadMainScene.progress;
            }
        }

        public void StartGameButtonOnClick()
        {
            GlobalEvents.FireLoadGame(this, new LoadEventArgs());
            newGameButton.gameObject.SetActive(false);
            loadingBar.gameObject.SetActive(true);
            loadingBarFrame.gameObject.SetActive(true);
        }

        public void LoadGameButtonOnClick()
        {
            GlobalEvents.FireLoadGame(this, new LoadEventArgs(false));
            continueGameButton.gameObject.SetActive(false);
            loadingBar.gameObject.SetActive(true);
            loadingBarFrame.gameObject.SetActive(true);
        }
        
        public void ExitButtonOnClick()
        {
            Application.Quit();
        }
    } 
}

