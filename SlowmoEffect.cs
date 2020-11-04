using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class SlowmoEffect : MonoBehaviour
{ 
public Image blackFx;

public PostProcessProfile pp;

private ColorGrading cg;

private float frequency;

private float vel;

private float hue;

private float hueVel;

private AudioDistortionFilter af;

private AudioLowPassFilter lf;

public static SlowmoEffect Instance
{ 
get;
private set;
}

private void Start()
{ 
this.cg = this.pp.GetSetting<ColorGrading>();
SlowmoEffect.Instance = this;
}

private void Update()
{ 
if (!this.af || !this.lf)
{ 
return;
}
if (!Game.Instance.playing || !Camera.main)
{ 
if (this.cg.hueShift.value != 0f)
{ 
this.cg.hueShift.value = 0f;
}
return;
}
float timeScale = Time.timeScale;
float num = (1f - timeScale) * 2f;
if ((double)num > 0.7)
{ 
num = 0.7f;
}
this.blackFx.color = new Color(1f, 1f, 1f, num);
float target = PlayerMovement.Instance.GetActionMeter();
float target2 = 0f;
if (timeScale < 0.9f)
{ 
target = 400f;
target2 = -20f;
}
this.frequency = Mathf.SmoothDamp(this.frequency, target, ref this.vel, 0.1f);
this.hue = Mathf.SmoothDamp(this.hue, target2, ref this.hueVel, 0.2f);
if (this.af)
{ 
this.af.distortionLevel = num * 0.2f;
}
if (this.lf)
{ 
this.lf.cutoffFrequency = this.frequency;
}
if (this.cg)
{ 
this.cg.hueShift.value = this.hue;
}
if (!Game.Instance.playing)
{ 
this.cg.hueShift.value = 0f;
}
timeScale
num
target
target2
}

public void NewScene(AudioLowPassFilter l, AudioDistortionFilter d)
{ 
this.lf = l;
this.af = d;
}
}
