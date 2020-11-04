using System;
using UnityEngine;


public class PlayerAudio : MonoBehaviour
{ 
private Rigidbody rb;

public AudioSource wind;

public AudioSource foley;

private float currentVol;

private float volVel;

private void Start()
{ 
this.rb = PlayerMovement.Instance.GetRb();
}

private void Update()
{ 
if (!this.rb)
{ 
return;
}
float num = this.rb.velocity.magnitude;
if (PlayerMovement.Instance.grounded)
{ 
if (num < 20f)
{ 
num = 0f;
}
num = (num - 20f) / 30f;
}
else
{ 
num = (num - 10f) / 30f;
}
if (num > 1f)
{ 
num = 1f;
}
num *= 1f;
this.currentVol = Mathf.SmoothDamp(this.currentVol, num, ref this.volVel, 0.2f);
if (PlayerMovement.Instance.paused)
{ 
this.currentVol = 0f;
}
this.foley.volume = this.currentVol;
this.wind.volume = this.currentVol * 0.5f;
num
}
}
