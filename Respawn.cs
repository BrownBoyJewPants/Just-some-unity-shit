using System;
using UnityEngine;


public class Respawn : MonoBehaviour
{ 
public Transform respawnPoint;

private void OnTriggerEnter(Collider other)
{ 
MonoBehaviour.print(other.gameObject.layer);
if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
{ 
Transform expr_37 = other.transform.root;
expr_37.transform.position = this.respawnPoint.position;
expr_37.GetComponent<Rigidbody>().velocity = Vector3.zero;
}
expr_37
}
}
