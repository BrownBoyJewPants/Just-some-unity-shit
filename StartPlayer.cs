using System;
using UnityEngine;


public class StartPlayer : MonoBehaviour
{ 
private void Awake()
{ 
for (int i = base.transform.childCount - 1; i >= 0; i--)
{ 
MonoBehaviour.print("removing child: " + i);
base.transform.GetChild(i).parent = null;
}
UnityEngine.Object.Destroy(base.gameObject);
i
}
}
