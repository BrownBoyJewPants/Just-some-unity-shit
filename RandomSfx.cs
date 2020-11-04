using System;
using UnityEngine;


public class RandomSfx : MonoBehaviour
{ 
public AudioClip[] sounds;

private void Awake()
{ 
AudioSource expr_06 = base.GetComponent<AudioSource>();
expr_06.clip = this.sounds[UnityEngine.Random.Range(0, this.sounds.Length - 1)];
expr_06.playOnAwake = true;
expr_06.pitch = 1f + UnityEngine.Random.Range(-0.3f, 0.1f);
expr_06.enabled = true;
expr_06
}
}
