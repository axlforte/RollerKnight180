using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 Alexander Lara
 5/10/25
 Controlls the title screen, endgame screen, and game over screen
 */


public class AltScreenText : MonoBehaviour
{
    /// <summary>
    /// This function will be set up to be called from clicking the play button
    /// this loads game scene
    /// </summary>
    public void PlayButtonPressed(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// This function will be set up to be called from clicking the quit button
    /// This quits entire game
    /// </summary>
    public void QuitButtonPressed()
    {
        Application.Quit();
    }
}
