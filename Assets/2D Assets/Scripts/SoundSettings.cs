using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider, _sfxSlider;

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSfx()
    {
        AudioManager.instance.ToggleSfx();
    }    

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);
    }

    public void SfxVolume()
    {
        AudioManager.instance.MusicVolume(_sfxSlider.value);
    }
}
