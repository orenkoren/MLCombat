using MiddleAges.Events;
using System.Collections;
using UnityEngine;

namespace MiddleAges.Arena
{
    public class ArenaFloor : MonoBehaviour
    {
        public int WaveRecoveryTime;
        public ArenaWave[] Waves;

        private int currentKills;
        private int currentWave;
        void Start()
        {
            GlobalEvents.AddGameEventsListeners += AddDeathListeners;
            GlobalEvents.RemoveGameEventsListeners += RemoveDeathListeners;
            SpawnWave();
        }

        private void SpawnWave()
        {
            foreach (var spawn in Waves[currentWave].Spawns)
            {
                Instantiate(spawn.Enemy, spawn.SpawnLocation.position, spawn.SpawnLocation.rotation);
            }
        }

        IEnumerator FinishWave()
        {
            yield return new WaitForSeconds(WaveRecoveryTime);
            if (currentWave < Waves.Length - 1)
            {
                currentWave++;
                SpawnWave();
            }
        }

        private void AddDeathListeners(object sender, string e)
        {
            ((GameEvents)sender).DeathListeners += SomethingDied;
        }

        private void RemoveDeathListeners(object sender, string e)
        {
            ((GameEvents)sender).DeathListeners -= SomethingDied;
        }

        private void SomethingDied(object sender, AbilityEventArgs e)
        {
            currentKills++;
            if (currentKills == Waves[currentWave].RequiredKills)
            {
                currentKills = 0;
                StartCoroutine(FinishWave());
            }
        }
        private void OnDestroy()
        {
            GlobalEvents.AddGameEventsListeners -= AddDeathListeners;
            GlobalEvents.RemoveGameEventsListeners -= RemoveDeathListeners;
        }
    }

    [System.Serializable]
    public class ArenaWave
    {
        public ArenaSpawn[] Spawns;
        public int RequiredKills;
    }

    [System.Serializable]
    public class ArenaSpawn
    {
        public GameObject Enemy;
        public Transform SpawnLocation;
    }
}