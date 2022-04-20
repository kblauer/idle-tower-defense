using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    private bool gamePaused = false;

    private int numEnemiesSpawned = 0;
    
    [SerializeField] private int maxNumEnemies = 100;
    [SerializeField] private int enemiesMissed = 0;
    [SerializeField] private int enemiesKilled = 0;
    [SerializeField] private float initialDelay = 5f;
    [SerializeField] private float enemyIntervalSeconds = 5f;
    [SerializeField] private TextMeshProUGUI enemyDisplay;
    [SerializeField] private GameObject finishPanel;
    [SerializeField] List<GameObject> enemies;

    private void Start() {
        // make sure finish panel is disabled
        finishPanel.SetActive(false);
        UpdateEnemyText();
        StartCoroutine("SpawnEnemies");
    }

    private void UpdateEnemyText() {
        enemyDisplay.text = string.Format("Kills: {0}/{1}   Missed: {2}", enemiesKilled, maxNumEnemies, enemiesMissed);
    }

    public void EnemyKilled() {
        enemiesKilled++;
        UpdateEnemyText();
        if (isGameOver()) {
            GameOver();
        }
    }

    public void EnemyMissed() {
        enemiesMissed++;
        UpdateEnemyText();
        if (isGameOver()) {
            GameOver();
        }
    }

    public void TogglePause() {
        gamePaused = !gamePaused;
        if (gamePaused) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    IEnumerator SpawnEnemies() {
        yield return new WaitForSeconds(initialDelay);

        // after initial delay, start spawning enemies with the interval
        int enemyIndex = 0;
        while(numEnemiesSpawned < maxNumEnemies) {
            GameObject createdEnemy = Instantiate(enemies[enemyIndex], new Vector3(10,10,10), Quaternion.identity);
            createdEnemy.GetComponent<FollowPath>().controlled = true;
            numEnemiesSpawned++;

            enemyIndex = 1 - enemyIndex;
            
            yield return new WaitForSeconds(enemyIntervalSeconds);
        }
    }

    private bool isGameOver() {
        return enemiesKilled + enemiesMissed >= maxNumEnemies;
    }

    private string finishText() {
        if (enemiesKilled == maxNumEnemies) {
            return "Perfect!";
        }
        else if (enemiesKilled > maxNumEnemies - 10) {
            return "Success";
        } else {
            return "Failure";
        }
    }

    private void GameOver() {
        TextMeshProUGUI[] texts = finishPanel.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts) {
            if (text.tag == "Finish") {
                text.text = finishText();
            }
        }
        finishPanel.SetActive(true);
        //Time.timeScale = 0;
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }
}
