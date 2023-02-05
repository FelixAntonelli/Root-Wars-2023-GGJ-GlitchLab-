using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus UI;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;


    float MusicVol = 1f;
    float UIVol = 1f;
    float SFXVol = 1f;
    float MasterVol = 1f;


    private void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/Music&Bg");
        UI = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/UI");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/OneShots");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/MASTER");
    }

    void Update()
    {
        Music.setVolume(MusicVol);
        Music.setVolume(UIVol);
        Music.setVolume(SFXVol);
        Music.setVolume(MasterVol);
    }

    public void setMusicVol (System.Single newVol)
    {
        MusicVol = newVol - 19F;
    }
    public void setUIVol(System.Single newVol)
    {
        UIVol = newVol - 19F;
    }
    public void setSFXVol(System.Single newVol)
    {
        SFXVol = newVol - 19f;
    }
    public void setMasterVol(System.Single newVol)
    {
        MasterVol = newVol - 19f;
    }


}
