using System;
using UnityEngine;


public class Music : MonoBehaviour
{ 
private AudioSource music;

private float multiplier;

private float desiredVolume;

private float vel;

public static Music Instance
{ 
get;
private set;
}

private void Awake()
{ 
Music.Instance = this;
this.music = base.GetComponent<AudioSource>();
this.music.volume = 0.04f;
this.multiplier = 1f;
}

private void Update()
{ 
this.desiredVolume = 0.016f * this.multiplier;
if (Game.Instance.playing)
{ 
this.desiredVolume = 0.6f * this.multiplier;
}
this.music.volume = Mathf.SmoothDamp(this.music.volume, this.desiredVolume, ref this.vel, 0.6f);
}

public void SetMusicVolume(float f)
{ 
this.multiplier = f;
}
}
