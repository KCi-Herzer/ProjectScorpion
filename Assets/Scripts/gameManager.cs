using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("-----Player------")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("-----UI------")]
    public GameObject menuCurrentlyOpen;
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject playerDamage;

    public Image HPBar;
    public TMP_Text enemyCounter;
    public TMP_Text waveCounter;
    public TMP_Text gameObjectives;

    public TMP_Text ammoCounter;

    public int enemyCount;
    public int totalObjectives;

    public bool isPaused;
    float timeScaleOrig;

    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.Find("Player Spawn Pos");

        timeScaleOrig = Time.timeScale;

        updateObjectiveUI();
        
        //countCurrentEnemies(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuCurrentlyOpen != playerDeadMenu && menuCurrentlyOpen != winMenu)
        {
            isPaused = !isPaused;
            menuCurrentlyOpen = pauseMenu;
            menuCurrentlyOpen.SetActive(isPaused);
            if (isPaused)
                cursorLockPause();
            else
                cursorUnlockUnpause();
        }
        StartCoroutine(countCurrentObjectives());
    }

    public void cursorLockPause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void cursorUnlockUnpause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = timeScaleOrig;
        if (menuCurrentlyOpen != null)
            menuCurrentlyOpen.SetActive(false);
        menuCurrentlyOpen = null;
    }

    public void playerIsDead()
    {
        isPaused = true;
        playerDeadMenu.SetActive(isPaused);
        menuCurrentlyOpen = playerDeadMenu;
        cursorLockPause();

        enemyAI[] currentEnemys;
        currentEnemys = FindObjectsOfType<enemyAI>();
        foreach (enemyAI enemy in currentEnemys)
        {
            enemy.playerDied();
        }
    }

    public void enemyDecrement()
    {
        enemyCount--;
        enemyCounter.text = enemyCount.ToString("F0");
        StartCoroutine(checkEnemyCount());
    }

    public void enemyIncrement(int amount)
    {
        enemyCount++;
        enemyCounter.text = enemyCount.ToString("F0");
    }
    public void updateObjectiveUI()
    {
        gameManager.instance.gameObjectives.text = gameManager.instance.totalObjectives.ToString("F0");
    }

    public IEnumerator countCurrentObjectives()
    {
        //foreach (enemyAI enemy in currentEnemys)
        //foreach (GameObjective gameObjective in GetComponents<GameObjective>())
        

        yield return new WaitForSeconds(2);
        
        if (totalObjectives <= 0)
        {
            menuCurrentlyOpen = winMenu;
            menuCurrentlyOpen.SetActive(true);
            cursorLockPause();
        }
    }

    IEnumerator checkEnemyCount()
    {
        if (enemyCount <= 0)
        {
            yield return new WaitForSeconds(2);
            //menuCurrentlyOpen = winMenu;
            //menuCurrentlyOpen.SetActive(true);
            //cursorLockPause();

            //The game manager tell the waveManager to start the next round
            waveManager.instance.NextWave();
        }
    }
}
