using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private SavingSystem savingSystem;
        private SceneLoader sceneLoader;
        void Start()
        {
            savingSystem = GetComponent<SavingSystem>();
            sceneLoader = GetComponent<SceneLoader>();
            GlobalEvents.LoadGameListeners += LoadGame;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
                savingSystem.Save(defaultSaveFile);
            if (Input.GetKeyDown(KeyCode.F4))
                savingSystem.Load(defaultSaveFile);
        }
        private void LoadGame(object sender, LoadEventArgs args)
        {
            if (args.IsNewGame)
                LoadNewGame();
            else
                LoadSavedGame();
        }
        private void LoadSavedGame()
        {
            sceneLoader.StartLoadingScene("Level1",() => savingSystem.Load("save"));
        }

        private void LoadNewGame()
        {
            sceneLoader.StartLoadingScene("Level1");
        }

        private void OnDestroy()
        {
            GlobalEvents.LoadGameListeners -= LoadGame;
        }

    }
}