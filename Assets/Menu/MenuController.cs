using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;  

public class MenuController : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("CenaJogo");  
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();  
    }
}
