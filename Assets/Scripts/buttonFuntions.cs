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
        gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.uiClickSound[Random.Range(0, gameManager.instance.playerScript.uiClickSound.Length)], gameManager.instance.playerScript.uiClickSoundVol);
    }

    public void restart()
    {
        gameManager.instance.cursorUnlockUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.uiClickSound[Random.Range(0, gameManager.instance.playerScript.uiClickSound.Length)], gameManager.instance.playerScript.uiClickSoundVol);
    }

    public void playerRespawn()
    {
        gameManager.instance.playerScript.respawn();
        gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.uiClickSound[Random.Range(0, gameManager.instance.playerScript.uiClickSound.Length)], gameManager.instance.playerScript.uiClickSoundVol);
    }

    public void quit()
    {
        Application.Quit();
        gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.uiClickSound[Random.Range(0, gameManager.instance.playerScript.uiClickSound.Length)], gameManager.instance.playerScript.uiClickSoundVol);
    }

}
