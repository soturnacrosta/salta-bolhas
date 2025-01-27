using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText; // Texto da pontuação no jogo

    private bool isGameOver = false;

    private int currentScore = 0; // Pontuação atual como número inteiro
    private int highScore = 0; // Recorde de pontuação
    private float scoreIncrementRate = 0.5f; // Fator de escala para a pontuação

    private void Awake()
    {
        // Configura a instância do GameManager
        if (Instance == null)
        {
            Instance = this; // Define a instância atual
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Garante que apenas uma instância exista
        }
    }

    private void Start()
    {
        // Carrega o recorde salvo
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Atualiza a pontuação inicial na interface
        UpdateScoreText();
    }

    private void Update()
    {
        // Atualiza a pontuação enquanto o jogo está ativo
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
            scoreText.text = "Score: " + currentScore; // Atualiza o texto da pontuação
        }
    }

    // Função para chamar Game Over
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Game Over!");

            // Atualiza o recorde de pontuação, se a pontuação atual for maior
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore); // Salva o recorde de pontuação
                PlayerPrefs.Save();
                Debug.Log("Novo Recorde: " + highScore);
            }

            // Mostra a pontuação final
            Debug.Log("Pontuação Final: " + currentScore);

            // Exibe a tela de Game Over
            GameOverUI gameOverUI = FindGameOverUI();
            if (gameOverUI != null)
            {
                gameOverUI.ShowGameOver(currentScore, highScore);
            }
            else
            {
                Debug.LogError("GameOverUI não encontrado na cena!");
            }
        }
    }

    // Reinicia o jogo
    public void RestartGame()
    {
        // Reinicia a pontuação atual
        currentScore = 0;
        isGameOver = false;

        // Reinicia a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private GameOverUI FindGameOverUI()
    {
        // Encontra a instância de GameOverUI na cena
        return Object.FindFirstObjectByType<GameOverUI>();
    }

    // Método para acessar a pontuação atual
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // Método para acessar o recorde de pontuação
    public int GetHighScore()
    {
        return highScore;
    }
}
