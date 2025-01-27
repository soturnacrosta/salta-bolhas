using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidade de movimento
    public float jumpForce = 10f; // Força do pulo

    [Header("Sprite Settings")]
    public GameObject facadeObject; // Objeto fachada com o SpriteRenderer

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer do objeto fachada
    private bool isGrounded = false; // Estado do jogador (se está no chão)
    private Bubble currentBubble; // Referência à bolha atual (se houver)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Referência ao Rigidbody2D do jogador

        // Obtém o SpriteRenderer do objeto fachada
        if (facadeObject != null)
        {
            spriteRenderer = facadeObject.GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        // Movimento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal"); // Lê o input das setas (esquerda/direita)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Verifica se o jogador está se movendo lateralmente e chama a deformação
        if (isGrounded && currentBubble != null)
        {
            if (moveInput > 0)
            {
                currentBubble.SetDeformRight(); // Deforma para a direita
            }
            else if (moveInput < 0)
            {
                currentBubble.SetDeformLeft(); // Deforma para a esquerda
            }
            else
            {
                currentBubble.SetPlayerOnBubble(); // Jogador parado em cima da bolha
            }
        }

        // Pular com Seta para Cima ou Barra de Espaço
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;

            // Define a bolha como normal ao pular
            if (currentBubble != null)
            {
                currentBubble.SetNormal();
                currentBubble = null; // Remove a referência à bolha
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
        // Verifica se o jogador tocou em uma bolha
        if (collision.gameObject.CompareTag("Bubble"))
        {
            isGrounded = true;

            // Atualiza a bolha atual
            currentBubble = collision.gameObject.GetComponent<Bubble>();
            if (currentBubble != null)
            {
                currentBubble.SetPlayerOnBubble(); // Define o sprite como jogador em cima
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica se o jogador saiu de uma bolha
        if (collision.gameObject.CompareTag("Bubble"))
        {
            isGrounded = false;

            // Redefine o estado da bolha
            if (currentBubble != null)
            {
                currentBubble.SetNormal();
                currentBubble = null; // Remove a referência à bolha
            }
        }
    }
}
