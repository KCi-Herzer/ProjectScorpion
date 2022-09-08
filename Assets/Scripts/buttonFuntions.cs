using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFuntions : MonoBehaviour
{
    public void resume()
    {
        if (gameManager.instance.isPaused)
        {
            gameManager.instance.isPaused = !gameManager.instance.isPaused;
            gameManager.instance.cursorUnlockUnpause();
        }

    }

    public void restart()
    {
        gameManager.instance.cursorUnlockUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void playerRespawn()
    {
        gameManager.instance.playerScript.respawn();
    }

    public void quit()
    {
        Application.Quit();
    }

}
