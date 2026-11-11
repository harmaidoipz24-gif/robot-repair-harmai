using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Налаштування рівня")]
    public int totalEnemies = 1; // Кількість роботів, яких треба відремонтувати

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
        UpdateCounter();
    }

    private void Update()
    {
        // Якщо гра завершена (перемога або поразка), натискання R перезавантажує сцену
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Викликається при кожному ремонті бота
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

    // Викликається при 0 HP гравця
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