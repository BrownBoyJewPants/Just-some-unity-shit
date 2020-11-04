using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{ 
public bool playing;

public bool done;

public static Game Instance
{ 
get;
private set;
}

private void Start()
{ 
Game.Instance = this;
this.playing = false;
}

public void StartGame()
{ 
this.playing = true;
this.done = false;
Time.timeScale = 1f;
UIManger.Instance.StartGame();
Timer.Instance.StartTimer();
}

public void RestartGame()
{ 
SceneManager.LoadScene(SceneManager.GetActiveScene().name);
Time.timeScale = 1f;
this.StartGame();
}

public void EndGame()
{ 
this.playing = false;
}

public void NextMap()
{ 
Time.timeScale = 1f;
int buildIndex = SceneManager.GetActiveScene().buildIndex;
if (buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
{ 
this.MainMenu();
return;
}
SceneManager.LoadScene(buildIndex + 1);
this.StartGame();
buildIndex
}

public void MainMenu()
{ 
this.playing = false;
SceneManager.LoadScene("MainMenu");
UIManger.Instance.GameUI(false);
Time.timeScale = 1f;
}

public void Win()
{ 
this.playing = false;
Timer.Instance.Stop();
Time.timeScale = 0.05f;
Cursor.lockState = CursorLockMode.None;
Cursor.visible = true;
UIManger.Instance.WinUI(true);
float timer = Timer.Instance.GetTimer();
int num = int.Parse(SceneManager.GetActiveScene().name[0].ToString() ?? "");
int num2;
if (int.TryParse(SceneManager.GetActiveScene().name.Substring(0, 2) ?? "", out num2))
{ 
num = num2;
}
float num3 = SaveManager.Instance.state.times[num];
if (timer < num3 || num3 == 0f)
{ 
SaveManager.Instance.state.times[num] = timer;
SaveManager.Instance.Save();
}
MonoBehaviour.print("time has been saved as: " + Timer.Instance.GetFormattedTime(timer));
this.done = true;
timer
num
num2
num3
}
}
