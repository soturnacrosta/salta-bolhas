using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGameOver = false;

    private int currentScore = 0; // Pontuação atual como número inteiro
    private int highScore = 0; // Recorde de pontuação
    private float scoreIncrementRate = 0.5f; // Fator de escala para a pontuação

    private void Awake()
    {
        // Garantir que exista apenas um GameManager na cena
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Atualiza a pontuação enquanto o jogo está ativo
        if (!isGameOver)
        {
            currentScore += Mathf.FloorToInt(Time.deltaTime * scoreIncrementRate * 100); // Incremento proporcional ao tempo
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
                Debug.Log("Novo Recorde: " + highScore);
            }

            // Mostra a pontuação final
            Debug.Log("Pontuação Final: " + currentScore);

            // Exibe a tela de Game Over
            GameOverUI gameOverUI = Object.FindFirstObjectByType<GameOverUI>();
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
    private void RestartGame()
    {
        // Reinicia a cena e reseta a pontuação atual
        currentScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isGameOver = false;
    }

    // Método para acessar a pontuação atual (opcional)
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // Método para acessar o recorde de pontuação (opcional)
    public int GetHighScore()
    {
        return highScore;
    }
}
