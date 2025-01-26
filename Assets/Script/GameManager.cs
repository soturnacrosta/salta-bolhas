using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGameOver = false;

    private float currentScore = 0f; // Pontuação atual
    private float highScore = 0f; // Recorde de pontuação

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
            currentScore += Time.deltaTime; // Incrementa com base no tempo decorrido
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
                Debug.Log("Novo Recorde: " + highScore.ToString("F2"));
            }

            // Mostra a pontuação final
            Debug.Log("Pontuação Final: " + currentScore.ToString("F2"));

            // Reinicia o jogo após 2 segundos
            Invoke("RestartGame", 2f);
        }
    }

    // Reinicia o jogo
    private void RestartGame()
    {
        // Reinicia a cena e reseta a pontuação atual
        currentScore = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isGameOver = false;
    }

    // Método para acessar a pontuação atual (opcional)
    public float GetCurrentScore()
    {
        return currentScore;
    }

    // Método para acessar o recorde de pontuação (opcional)
    public float GetHighScore()
    {
        return highScore;
    }
}
