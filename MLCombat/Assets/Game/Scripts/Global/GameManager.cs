using MiddleAges.Utils;
using UnityEngine;

namespace MiddleAges.Manager
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}
