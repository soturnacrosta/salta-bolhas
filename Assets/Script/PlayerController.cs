using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidade de movimento
    public float jumpForce = 10f; // Força do pulo

    [Header("Audio Settings")]
    public AudioClip jumpSound; // Som do salto
    private AudioSource audioSource; // Fonte de áudio

    private Rigidbody2D rb;
    private bool isGrounded = false; // Estado do jogador (se está no chão)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Referência ao Rigidbody2D do jogador
        audioSource = GetComponent<AudioSource>(); // Referência ao AudioSource do jogador
    }

    void Update()
    {
        // Movimento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal"); // Lê o input das setas (esquerda/direita)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Pular com Seta para Cima ou Barra de Espaço
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            PlayJumpSound(); // Reproduz o som do salto
        }

        // Virar o personagem para a direção que ele está andando
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o jogador tocou no chão
        if (collision.gameObject.CompareTag("Bubble"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica se o jogador saiu do chão
        if (collision.gameObject.CompareTag("Bubble"))
        {
            isGrounded = false;
        }
    }

    private void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound); // Reproduz o som do salto
        }
    }
}
