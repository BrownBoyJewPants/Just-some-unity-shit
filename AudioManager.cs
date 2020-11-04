using System;
using UnityEngine;


namespace Audio
{ 
public class AudioManager : MonoBehaviour
{ 
public Sound[] sounds;

public Sound[] footsteps;

public Sound[] wallrun;

public Sound[] jumps;

public AudioLowPassFilter filter;

private float desiredFreq = 500f;

private float velFreq;

private float freqSpeed = 0.2f;

public bool muted;

public static AudioManager Instance
{ 
get;
set;
}

private void Awake()
{ 
AudioManager.Instance = this;
Sound[] array = this.sounds;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound = array[i];
sound.source = base.gameObject.AddComponent<AudioSource>();
sound.source.clip = sound.clip;
sound.source.loop = sound.loop;
sound.source.volume = sound.volume;
sound.source.pitch = sound.pitch;
sound.source.bypassListenerEffects = sound.bypass;
}
array = this.footsteps;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound2 = array[i];
sound2.source = base.gameObject.AddComponent<AudioSource>();
sound2.source.clip = sound2.clip;
sound2.source.loop = sound2.loop;
sound2.source.volume = sound2.volume;
sound2.source.pitch = sound2.pitch;
sound2.source.bypassListenerEffects = sound2.bypass;
}
array = this.wallrun;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound3 = array[i];
sound3.source = base.gameObject.AddComponent<AudioSource>();
sound3.source.clip = sound3.clip;
sound3.source.loop = sound3.loop;
sound3.source.volume = sound3.volume;
sound3.source.pitch = sound3.pitch;
sound3.source.bypassListenerEffects = sound3.bypass;
}
array = this.jumps;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound4 = array[i];
sound4.source = base.gameObject.AddComponent<AudioSource>();
sound4.source.clip = sound4.clip;
sound4.source.loop = sound4.loop;
sound4.source.volume = sound4.volume;
sound4.source.pitch = sound4.pitch;
sound4.source.bypassListenerEffects = sound4.bypass;
}
array
i
sound
sound2
sound3
sound4
}

private void Update()
{ 
}

public void MuteSounds(bool b)
{ 
if (b)
{ 
AudioListener.volume = 0f;
}
else
{ 
AudioListener.volume = 1f;
}
this.muted = b;
}

public void PlayButton()
{ 
if (this.muted)
{ 
return;
}
Sound[] array = this.sounds;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound = array[i];
if (sound.name == "Button")
{ 
sound.source.pitch = 0.8f + UnityEngine.Random.Range(-0.03f, 0.03f);
break;
}
}
this.Play("Button");
array
i
sound
}

public void PlayPitched(string n, float v)
{ 
if (this.muted)
{ 
return;
}
Sound[] array = this.sounds;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound = array[i];
if (sound.name == n)
{ 
sound.source.pitch = 1f + UnityEngine.Random.Range(-v, v);
break;
}
}
this.Play(n);
array
i
sound
}

public void MuteMusic()
{ 
Sound[] array = this.sounds;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound = array[i];
if (sound.name == "Song")
{ 
sound.source.volume = 0f;
return;
}
}
array
i
sound
}

public void SetVolume(float v)
{ 
Sound[] array = this.sounds;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound = array[i];
if (sound.name == "Song")
{ 
sound.source.volume = v;
return;
}
}
array
i
sound
}

public void UnmuteMusic()
{ 
Sound[] array = this.sounds;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound = array[i];
if (sound.name == "Song")
{ 
sound.source.volume = 1.15f;
return;
}
}
array
i
sound
}

public void Play(string n)
{ 
if (this.muted && n != "Song")
{ 
return;
}
Sound[] array = this.sounds;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound = array[i];
if (sound.name == n)
{ 
sound.source.Play();
return;
}
}
array
i
sound
}

public void PlayFootStep()
{ 
if (this.muted)
{ 
return;
}
int num = UnityEngine.Random.Range(0, this.footsteps.Length - 1);
this.footsteps[num].source.Play();
num
}

public void PlayLanding()
{ 
if (this.muted)
{ 
return;
}
int num = UnityEngine.Random.Range(0, this.wallrun.Length - 1);
this.wallrun[num].source.Play();
num
}

public void PlayJump()
{ 
if (this.muted)
{ 
return;
}
int num = UnityEngine.Random.Range(0, this.jumps.Length - 1);
Sound sound = this.jumps[num];
if (sound.source)
{ 
sound.source.Play();
}
num
sound
}

public void Stop(string n)
{ 
Sound[] array = this.sounds;
for (int i = 0; i < array.Length; i++)
{ 
Sound sound = array[i];
if (sound.name == n)
{ 
sound.source.Stop();
return;
}
}
array
i
sound
}

public void SetFreq(float freq)
{ 
this.desiredFreq = freq;
}
}
}
