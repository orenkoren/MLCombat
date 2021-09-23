using MiddleAges.Database;
using UnityEngine;

namespace MiddleAges.Sounds
{
    public class SoundPlayer : MonoBehaviour
    {
        private AudioSource audioSource;
        public Abilities abilities;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);
    }
}
