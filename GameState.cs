using Audio;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class GameState : MonoBehaviour
{ 
public GameObject ppVolume;

public PostProcessProfile pp;

private MotionBlur ppBlur;

public bool graphics = true;

public bool muted;

public bool blur = true;

public bool shake = true;

public bool slowmo = true;

private float sensitivity = 1f;

private float volume;

private float music;

public float fov = 1f;

public float cameraShake = 1f;

public static GameState Instance
{ 
get;
private set;
}

private void Start()
{ 
GameState.Instance = this;
this.ppBlur = this.pp.GetSetting<MotionBlur>();
this.graphics = SaveManager.Instance.state.graphics;
this.shake = SaveManager.Instance.state.cameraShake;
this.blur = SaveManager.Instance.state.motionBlur;
this.slowmo = SaveManager.Instance.state.slowmo;
this.muted = SaveManager.Instance.state.muted;
this.sensitivity = SaveManager.Instance.state.sensitivity;
this.music = SaveManager.Instance.state.music;
this.volume = SaveManager.Instance.state.volume;
this.fov = SaveManager.Instance.state.fov;
this.UpdateSettings();
}

public void SetGraphics(bool b)
{ 
this.graphics = b;
this.ppVolume.SetActive(b);
SaveManager.Instance.state.graphics = b;
SaveManager.Instance.Save();
}

public void SetBlur(bool b)
{ 
this.blur = b;
if (b)
{ 
this.ppBlur.shutterAngle.value = 160f;
}
else
{ 
this.ppBlur.shutterAngle.value = 0f;
}
SaveManager.Instance.state.motionBlur = b;
SaveManager.Instance.Save();
}

public void SetShake(bool b)
{ 
this.shake = b;
if (b)
{ 
this.cameraShake = 1f;
}
else
{ 
this.cameraShake = 0f;
}
SaveManager.Instance.state.cameraShake = b;
SaveManager.Instance.Save();
}

public void SetSlowmo(bool b)
{ 
this.slowmo = b;
SaveManager.Instance.state.slowmo = b;
SaveManager.Instance.Save();
}

public void SetSensitivity(float s)
{ 
float num = Mathf.Clamp(s, 0f, 5f);
this.sensitivity = num;
if (PlayerMovement.Instance)
{ 
PlayerMovement.Instance.UpdateSensitivity();
}
SaveManager.Instance.state.sensitivity = num;
SaveManager.Instance.Save();
num
}

public void SetMusic(float s)
{ 
float musicVolume = Mathf.Clamp(s, 0f, 1f);
this.music = musicVolume;
if (Music.Instance)
{ 
Music.Instance.SetMusicVolume(musicVolume);
}
SaveManager.Instance.state.music = musicVolume;
SaveManager.Instance.Save();
MonoBehaviour.print("music saved as: " + this.music);
musicVolume
}

public void SetVolume(float s)
{ 
float num = Mathf.Clamp(s, 0f, 1f);
this.volume = num;
AudioListener.volume = num;
SaveManager.Instance.state.volume = num;
SaveManager.Instance.Save();
num
}

public void SetFov(float f)
{ 
float num = Mathf.Clamp(f, 50f, 150f);
this.fov = num;
if (MoveCamera.Instance)
{ 
MoveCamera.Instance.UpdateFov();
}
SaveManager.Instance.state.fov = num;
SaveManager.Instance.Save();
num
}

public void SetMuted(bool b)
{ 
AudioManager.Instance.MuteSounds(b);
this.muted = b;
SaveManager.Instance.state.muted = b;
SaveManager.Instance.Save();
}

private void UpdateSettings()
{ 
this.SetGraphics(this.graphics);
this.SetBlur(this.blur);
this.SetSensitivity(this.sensitivity);
this.SetMusic(this.music);
this.SetVolume(this.volume);
this.SetFov(this.fov);
this.SetShake(this.shake);
this.SetSlowmo(this.slowmo);
this.SetMuted(this.muted);
}

public bool GetGraphics()
{ 
return this.graphics;
}

public float GetSensitivity()
{ 
return this.sensitivity;
}

public float GetVolume()
{ 
return this.volume;
}

public float GetMusic()
{ 
return this.music;
}

public float GetFov()
{ 
return this.fov;
}

public bool GetMuted()
{ 
return this.muted;
}
}
