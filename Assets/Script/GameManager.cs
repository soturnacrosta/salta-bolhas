using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGameOver = false;

    private int currentScore = 0; // Pontua��o atual como n�mero inteiro
    private int highScore = 0; // Recorde de pontua��o
    private float scoreIncrementRate = 0.5f; // Fator de escala para a pontua��o

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
        // Atualiza a pontua��o enquanto o jogo est� ativo
        if (!isGameOver)
        {
            currentScore += Mathf.FloorToInt(Time.deltaTime * scoreIncrementRate * 100); // Incremento proporcional ao tempo
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
                Debug.Log("Novo Recorde: " + highScore);
            }

            // Mostra a pontua��o final
            Debug.Log("Pontua��o Final: " + currentScore);

            // Exibe a tela de Game Over
            GameOverUI gameOverUI = Object.FindFirstObjectByType<GameOverUI>();
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
    private void RestartGame()
    {
        // Reinicia a cena e reseta a pontua��o atual
        currentScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isGameOver = false;
    }

    // M�todo para acessar a pontua��o atual (opcional)
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // M�todo para acessar o recorde de pontua��o (opcional)
    public int GetHighScore()
    {
        return highScore;
    }
}
