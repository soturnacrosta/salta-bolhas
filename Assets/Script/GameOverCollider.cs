using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o jogador colidiu
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
