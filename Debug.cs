using System;
using TMPro;
using UnityEngine;


public class Debug : MonoBehaviour
{ 
public TextMeshProUGUI fps;

public TMP_InputField console;

public TextMeshProUGUI consoleLog;

private bool fpsOn;

private bool speedOn;

private float deltaTime;

private void Start()
{ 
Application.targetFrameRate = 150;
}

private void Update()
{ 
this.Fps();
if (Input.GetKeyDown(KeyCode.Tab))
{ 
if (this.console.isActiveAndEnabled)
{ 
this.CloseConsole();
return;
}
this.OpenConsole();
}
}

private void Fps()
{ 
if (this.fpsOn || this.speedOn)
{ 
if (!this.fps.gameObject.activeInHierarchy)
{ 
this.fps.gameObject.SetActive(true);
}
this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
float num = this.deltaTime * 1000f;
float num2 = 1f / this.deltaTime;
string text = "";
if (this.fpsOn)
{ 
text += string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
}
if (this.speedOn)
{ 
text = text + "\nm/s: " + string.Format("{0:F1}", PlayerMovement.Instance.rb.velocity.magnitude);
}
this.fps.text = text;
return;
}
if (this.fps.enabled)
{ 
return;
}
this.fps.gameObject.SetActive(false);
num
num2
text
}

private void OpenConsole()
{ 
this.console.gameObject.SetActive(true);
this.console.Select();
this.console.ActivateInputField();
Cursor.lockState = CursorLockMode.None;
Cursor.visible = true;
PlayerMovement.Instance.paused = true;
Time.timeScale = 0f;
}

private void CloseConsole()
{ 
this.console.gameObject.SetActive(false);
Cursor.lockState = CursorLockMode.Locked;
Cursor.visible = false;
PlayerMovement.Instance.paused = false;
Time.timeScale = 1f;
}

public void RunCommand()
{ 
string text = this.console.text;
TextMeshProUGUI expr_12 = this.consoleLog;
expr_12.text = expr_12.text + text + "\n";
if (text.Length < 2 || text.Length > 30 || this.CountWords(text) != 2)
{ 
this.console.text = "";
this.console.Select();
this.console.ActivateInputField();
return;
}
this.console.text = "";
string arg_9E_0 = text.Substring(text.IndexOf(' ') + 1);
string a = text.Substring(0, text.IndexOf(' '));
int n;
if (!int.TryParse(arg_9E_0, out n))
{ 
TextMeshProUGUI expr_AB = this.consoleLog;
expr_AB.text += "Command not found\n";
return;
}
if (!(a == "fps"))
{ 
if (!(a == "fpslimit"))
{ 
if (!(a == "fov"))
{ 
if (!(a == "sens"))
{ 
if (!(a == "speed"))
{ 
if (a == "help")
{ 
this.Help();
}
}
else
{ 
this.OpenCloseSpeed(n);
}
}
else
{ 
this.ChangeSens(n);
}
}
else
{ 
this.ChangeFov(n);
}
}
else
{ 
this.FpsLimit(n);
}
}
else
{ 
this.OpenCloseFps(n);
}
this.console.Select();
this.console.ActivateInputField();
text
expr_12
arg_9E_0
a
n
expr_AB
}

private void Help()
{ 
string str = "The console can be used for simple commands.\nEvery command must be followed by number i (0 = false, 1 = true)\n<i><b>fps 1</b></i>            shows fps\n<i><b>speed 1</b></i>      shows speed\n<i><b>fov i</b></i>             sets fov to i\n<i><b>sens i</b></i>          sets sensitivity to i\n<i><b>fpslimit i</b></i>    sets max fps\n<i><b>TAB</b></i>              to open/close the console\n";
TextMeshProUGUI expr_0C = this.consoleLog;
expr_0C.text += str;
str
expr_0C
}

private void FpsLimit(int n)
{ 
Application.targetFrameRate = n;
TextMeshProUGUI textMeshProUGUI = this.consoleLog;
textMeshProUGUI.text = string.Concat(new object[]
{ 
textMeshProUGUI.text,
"Max FPS set to ",
n,
"\n"
});
textMeshProUGUI
}

private void OpenCloseFps(int n)
{ 
this.fpsOn = (n == 1);
TextMeshProUGUI expr_10 = this.consoleLog;
expr_10.text += ("FPS set to " + n == 1 + "\n").ToString();
expr_10
}

private void OpenCloseSpeed(int n)
{ 
this.speedOn = (n == 1);
TextMeshProUGUI expr_10 = this.consoleLog;
expr_10.text += ("Speedometer set to " + n == 1 + "\n").ToString();
expr_10
}

private void ChangeFov(int n)
{ 
GameState.Instance.SetFov((float)n);
TextMeshProUGUI textMeshProUGUI = this.consoleLog;
textMeshProUGUI.text = string.Concat(new object[]
{ 
textMeshProUGUI.text,
"FOV set to ",
n,
"\n"
});
textMeshProUGUI
}

private void ChangeSens(int n)
{ 
GameState.Instance.SetSensitivity((float)n);
TextMeshProUGUI textMeshProUGUI = this.consoleLog;
textMeshProUGUI.text = string.Concat(new object[]
{ 
textMeshProUGUI.text,
"Sensitivity set to ",
n,
"\n"
});
textMeshProUGUI
}

private int CountWords(string text)
{ 
int num = 0;
int i;
for (i = 0; i < text.Length; i++)
{ 
if (!char.IsWhiteSpace(text[i]))
{ 
break;
}
}
while (i < text.Length)
{ 
while (i < text.Length && !char.IsWhiteSpace(text[i]))
{ 
i++;
}
num++;
while (i < text.Length && char.IsWhiteSpace(text[i]))
{ 
i++;
}
}
return num;
num
i
}
}
