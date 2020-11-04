using System;
using UnityEngine;


public class Break : MonoBehaviour
{ 
public GameObject replace;

private bool done;

private void OnCollisionEnter(Collision other)
{ 
if (this.done)
{ 
return;
}
if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
{ 
return;
}
Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
if (!component)
{ 
return;
}
if (component.velocity.magnitude > 18f)
{ 
if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
{ 
if (!PlayerMovement.Instance.IsCrouching())
{ 
return;
}
PlayerMovement.Instance.Slowmo(0.35f, 0.8f);
this.BreakDoor(component);
}
this.BreakDoor(component);
}
component
}

private void BreakDoor(Rigidbody rb)
{ 
Vector3 a = rb.velocity;
float magnitude = a.magnitude;
if (magnitude > 20f)
{ 
float d = magnitude / 20f;
a /= d;
}
Rigidbody[] componentsInChildren = UnityEngine.Object.Instantiate<GameObject>(this.replace, base.transform.position, base.transform.rotation).GetComponentsInChildren<Rigidbody>();
for (int i = 0; i < componentsInChildren.Length; i++)
{ 
componentsInChildren[i].velocity = a * 1.5f;
}
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.destructionAudio, base.transform.position, Quaternion.identity);
UnityEngine.Object.Destroy(base.gameObject);
this.done = true;
a
magnitude
d
componentsInChildren
i
}
}
