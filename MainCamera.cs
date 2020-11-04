using System;
using UnityEngine;


public class MainCamera : MonoBehaviour
{ 
private void Awake()
{ 
if (!SlowmoEffect.Instance)
{ 
return;
}
SlowmoEffect.Instance.NewScene(base.GetComponent<AudioLowPassFilter>(), base.GetComponent<AudioDistortionFilter>());
}
}
