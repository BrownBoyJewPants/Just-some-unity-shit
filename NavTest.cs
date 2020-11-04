using System;
using UnityEngine;
using UnityEngine.AI;


public class NavTest : MonoBehaviour
{ 
private NavMeshAgent agent;

private void Start()
{ 
this.agent = base.GetComponent<NavMeshAgent>();
}

private void Update()
{ 
if (!PlayerMovement.Instance)
{ 
return;
}
Vector3 position = PlayerMovement.Instance.transform.position;
if (this.agent.isOnNavMesh)
{ 
this.agent.destination = position;
MonoBehaviour.print("goin");
}
position
}
}
