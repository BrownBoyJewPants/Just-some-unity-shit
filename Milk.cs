using System;
using UnityEngine;


public class Milk : MonoBehaviour
{ 
private void Update()
{ 
float z = Mathf.PingPong(Time.time, 1f);
Vector3 axis = new Vector3(1f, 1f, z);
base.transform.Rotate(axis, 0.5f);
z
axis
}

private void OnTriggerEnter(Collider other)
{ 
if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
{ 
if (PlayerMovement.Instance.IsDead())
{ 
return;
}
Game.Instance.Win();
MonoBehaviour.print("Player won");
}
}
}
