using System;
using UnityEngine;


public class UIManger : MonoBehaviour
{ 
public GameObject gameUI;

public GameObject deadUI;

public GameObject winUI;

public static UIManger Instance
{ 
get;
private set;
}

private void Awake()
{ 
UIManger.Instance = this;
}

private void Start()
{ 
this.gameUI.SetActive(false);
}

public void StartGame()
{ 
this.gameUI.SetActive(true);
this.DeadUI(false);
this.WinUI(false);
}

public void GameUI(bool b)
{ 
this.gameUI.SetActive(b);
}

public void DeadUI(bool b)
{ 
this.deadUI.SetActive(b);
}

public void WinUI(bool b)
{ 
this.winUI.SetActive(b);
MonoBehaviour.print("setting win UI");
}
}
