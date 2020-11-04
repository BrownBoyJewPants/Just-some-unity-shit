using System;
using UnityEngine;


public class MoveCamera : MonoBehaviour
{ 
public Transform player;

private Vector3 offset;

private Camera cam;

public static MoveCamera Instance
{ 
get;
private set;
}

private void Start()
{ 
MoveCamera.Instance = this;
this.cam = base.transform.GetChild(0).GetComponent<Camera>();
this.cam.fieldOfView = GameState.Instance.fov;
this.offset = base.transform.position - this.player.transform.position;
}

private void Update()
{ 
base.transform.position = this.player.transform.position;
}

public void UpdateFov()
{ 
this.cam.fieldOfView = GameState.Instance.fov;
}
}
