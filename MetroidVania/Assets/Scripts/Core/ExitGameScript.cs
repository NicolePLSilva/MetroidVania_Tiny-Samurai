using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameScript : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
