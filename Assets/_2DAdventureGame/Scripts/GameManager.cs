using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private int totalEnemies = 0;
    private int fixedEnemiesCount = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        totalEnemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;
        UpdateCounter();
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void RobotFixed()
    {
        if (isGameOver) return;

        fixedEnemiesCount++;
        UpdateCounter();

        if (fixedEnemiesCount >= totalEnemies)
        {
            WinGame();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (UIHandler.instance != null)
        {
            UIHandler.instance.DisplayLoseScreen();
        }
    }

    private void WinGame()
    {
        isGameOver = true;

        if (UIHandler.instance != null)
        {
            UIHandler.instance.DisplayWinScreen();
        }
    }

    private void UpdateCounter()
    {
        if (UIHandler.instance != null)
        {
            UIHandler.instance.SetCounter(fixedEnemiesCount, totalEnemies);
        }
    }
}