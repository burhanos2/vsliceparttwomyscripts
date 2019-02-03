using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("QuitApp");
    }
}
