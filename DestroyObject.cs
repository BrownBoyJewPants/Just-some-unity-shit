using System;
using UnityEngine;


public class DestroyObject : MonoBehaviour
{ 
public float time;

private void Start()
{ 
base.Invoke("DestroySelf", this.time);
}

private void DestroySelf()
{ 
UnityEngine.Object.Destroy(base.gameObject);
}
}
