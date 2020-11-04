using System;
using UnityEngine;


public class Bounce : MonoBehaviour
{ 
private void OnCollisionEnter(Collision other)
{ 
MonoBehaviour.print("yeet");
other.gameObject.GetComponent<Rigidbody>();
}
}
