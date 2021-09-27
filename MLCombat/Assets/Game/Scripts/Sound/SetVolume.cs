using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour {

    public AudioMixer mixer;
    public float startingVolume;

    private void Start()
    {
        SetLevel(startingVolume);
    }

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
}