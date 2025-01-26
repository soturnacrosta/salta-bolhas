using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidade de movimento
    public float jumpForce = 10f; // Força do pulo

    [Header("Audio Settings")]
    public AudioClip jumpSound; // Som do salto
    private AudioSource audioSource; // Fonte de áudio

    [Header("Animator Settings")]
    public Animator facadeAnimator; // Referência ao Animator em outro objeto

    private Rigidbody2D rb;
    private bool isGrounded = false; // Estado do jogador (se está no chão)
    private bool isAshamed = false; // Estado de vergonha do jogador

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Referência ao Rigidbody2D do jogador
        audioSource = GetComponent<AudioSource>(); // Referência ao AudioSource do jogador

        // Certifique-se de que o Animator está configurado no Inspector
        if (facadeAnimator == null)
        {
            Debug.LogError("Animator não está configurado! Por favor, arraste o objeto no Inspector.");
        }
    }

    void Update()
    {
        // Movimento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal"); // Lê o input das setas (esquerda/direita)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Atualizar estados no Animator
        UpdateAnimatorStates();

        // Pular com Seta para Cima ou Barra de Espaço
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            PlayJumpSound(); // Reproduz o som do salto
            if (facadeAnimator != null)
            {
                facadeAnimator.SetBool("isJumping", true); // Define o estado de pulo
                facadeAnimator.SetBool("isGrounded", false); // Não está mais no chão
            }
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
            isAshamed = false; // Sai do estado envergonhado
            if (facadeAnimator != null)
            {
                facadeAnimator.SetBool("isGrounded", true); // Atualiza estado no Animator
                facadeAnimator.SetBool("isJumping", false); // Sai do estado de pulo
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica se o jogador saiu do chão
        if (collision.gameObject.CompareTag("Bubble"))
        {
            isGrounded = false;
            if (facadeAnimator != null)
            {
                facadeAnimator.SetBool("isGrounded", false); // Atualiza estado no Animator
            }
        }
    }

    private void UpdateAnimatorStates()
    {
        if (facadeAnimator == null) return;

        // Atualiza estado de queda
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            facadeAnimator.SetBool("isFalling", true);
            facadeAnimator.SetBool("isJumping", false);
        }
        else
        {
            facadeAnimator.SetBool("isFalling", false);
        }

        // Define o estado de vergonha caso necessário
        facadeAnimator.SetBool("isAshamed", isAshamed);
    }

    public void TriggerAshamedState()
    {
        // Chamado quando o jogador cai sem bolhas
        isAshamed = true;
        if (facadeAnimator != null)
        {
            facadeAnimator.SetBool("isAshamed", true);
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
