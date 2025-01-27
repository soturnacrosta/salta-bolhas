using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject gameOverPanel; // Painel da tela de Game Over
    public GameObject scoreCanvas; // Referência ao Canvas da pontuação
    public TextMeshProUGUI scoreText; // Texto da pontuação final
    public TextMeshProUGUI highScoreText; // Texto do recorde

    private void Start()
    {
        // Certifique-se de que a tela de Game Over está oculta no início
        gameOverPanel.SetActive(false);
    }

    // Exibe a tela de Game Over
    public void ShowGameOver(int currentScore, int highScore)
    {
        // Ativa o painel de Game Over
        gameOverPanel.SetActive(true);

        // Desativa o Canvas da pontuação
        if (scoreCanvas != null)
        {
            scoreCanvas.SetActive(false);
        }

        // Atualiza os textos de pontuação
        scoreText.text = "Score: " + currentScore;
        highScoreText.text = "High Score: " + highScore;

        // Pausa o jogo
        Time.timeScale = 0f;
    }

    // Reinicia o jogo
    public void RetryGame()
    {
        // Restaura o tempo do jogo
        Time.timeScale = 1f;

        // Recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Volta ao menu principal
    public void GoToMenu()
    {
        // Restaura o tempo do jogo
        Time.timeScale = 1f;

        // Carrega a cena do menu principal
        SceneManager.LoadScene("MenuScene"); // Substitua "MenuScene" pelo nome exato da cena do menu
    }
}
