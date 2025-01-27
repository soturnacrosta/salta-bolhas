using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText; // Texto da pontua��o no jogo

    private bool isGameOver = false;

    private int currentScore = 0; // Pontua��o atual como n�mero inteiro
    private int highScore = 0; // Recorde de pontua��o
    private float scoreIncrementRate = 0.5f; // Fator de escala para a pontua��o

    private void Awake()
    {
        // Configura a inst�ncia do GameManager
        if (Instance == null)
        {
            Instance = this; // Define a inst�ncia atual
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Garante que apenas uma inst�ncia exista
        }
    }

    private void Start()
    {
        // Carrega o recorde salvo
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Atualiza a pontua��o inicial na interface
        UpdateScoreText();
    }

    private void Update()
    {
        // Atualiza a pontua��o enquanto o jogo est� ativo
        if (!isGameOver)
        {
            currentScore += Mathf.FloorToInt(Time.deltaTime * scoreIncrementRate * 100); // Incremento proporcional ao tempo
            UpdateScoreText();
        }
    }

    public void AddScore(int amount)
    {
        if (!isGameOver)
        {
            currentScore += amount;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore; // Atualiza o texto da pontua��o
        }
    }

    // Fun��o para chamar Game Over
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Game Over!");

            // Atualiza o recorde de pontua��o, se a pontua��o atual for maior
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore); // Salva o recorde de pontua��o
                PlayerPrefs.Save();
                Debug.Log("Novo Recorde: " + highScore);
            }

            // Mostra a pontua��o final
            Debug.Log("Pontua��o Final: " + currentScore);

            // Exibe a tela de Game Over
            GameOverUI gameOverUI = FindGameOverUI();
            if (gameOverUI != null)
            {
                gameOverUI.ShowGameOver(currentScore, highScore);
            }
            else
            {
                Debug.LogError("GameOverUI n�o encontrado na cena!");
            }
        }
    }

    // Reinicia o jogo
    public void RestartGame()
    {
        // Reinicia a pontua��o atual
        currentScore = 0;
        isGameOver = false;

        // Reinicia a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private GameOverUI FindGameOverUI()
    {
        // Encontra a inst�ncia de GameOverUI na cena
        return Object.FindFirstObjectByType<GameOverUI>();
    }

    // M�todo para acessar a pontua��o atual
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // M�todo para acessar o recorde de pontua��o
    public int GetHighScore()
    {
        return highScore;
    }
}
