using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGameOver = false;

    private float currentScore = 0f; // Pontua��o atual
    private float highScore = 0f; // Recorde de pontua��o

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
            currentScore += Time.deltaTime; // Incrementa com base no tempo decorrido
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
                Debug.Log("Novo Recorde: " + highScore.ToString("F2"));
            }

            // Mostra a pontua��o final
            Debug.Log("Pontua��o Final: " + currentScore.ToString("F2"));

            // Reinicia o jogo ap�s 2 segundos
            Invoke("RestartGame", 2f);
        }
    }

    // Reinicia o jogo
    private void RestartGame()
    {
        // Reinicia a cena e reseta a pontua��o atual
        currentScore = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isGameOver = false;
    }

    // M�todo para acessar a pontua��o atual (opcional)
    public float GetCurrentScore()
    {
        return currentScore;
    }

    // M�todo para acessar o recorde de pontua��o (opcional)
    public float GetHighScore()
    {
        return highScore;
    }
}
