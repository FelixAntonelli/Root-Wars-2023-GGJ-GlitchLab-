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
        Music = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/MusicBG");
        UI = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/UI");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/MASTER/OneShots");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/MASTER");
    }

    void Update()
    {
        
        
        
       
    }

    public void setMusicVol (float newVol)
    {

        MusicVol = newVol;
        Music.setVolume(MusicVol);
    }
    public void setUIVol(float newVol)
    {
        UIVol = newVol;
        Music.setVolume(UIVol);
    }
    public void setSFXVol(float newVol)
    {
        SFXVol = newVol;
        Music.setVolume(SFXVol);
    }
    public void setMasterVol(float newVol)
    {
        MasterVol = newVol;
        Music.setVolume(MasterVol);
    }


}
