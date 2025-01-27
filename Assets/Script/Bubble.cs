using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip popSound; // Som do estouro
    private AudioSource audioSource; // Fonte de áudio

    [Header("Sprite Settings")]
    public GameObject facadeObject; // Objeto fachada com o SpriteRenderer
    public Sprite normalSprite; // Sprite para o estado normal
    public Sprite playerOnBubbleSprite; // Sprite para o jogador em cima
    public Sprite deformSprite; // Sprite para deformação (usado para ambos os lados)
    public Sprite popSprite; // Sprite para o estouro

    private SpriteRenderer spriteRenderer;
    public delegate void BubbleDestroyed();
    public BubbleDestroyed OnBubbleDestroyed;

    private float lifetime;
    private bool isPopped = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Referência ao AudioSource

        // Obtém o SpriteRenderer do objeto fachada
        if (facadeObject != null)
        {
            spriteRenderer = facadeObject.GetComponent<SpriteRenderer>();
        }

        // Inicializa com o sprite normal
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    public void Initialize(float size)
    {
        lifetime = size * 5f; // Tempo de vida é proporcional ao tamanho
        Invoke(nameof(DestroyBubble), lifetime); // Destrói a bolha após o tempo de vida
    }

    public void MakeGhost()
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Random.Range(0.2f, 0.5f); // Transparência entre 20% e 50%
            spriteRenderer.color = color;
        }

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Desativa o colisor
        }
    }

    private void DestroyBubble()
    {
        if (isPopped) return; // Evita múltiplas chamadas para destruição

        isPopped = true;

        // Atualiza para o sprite de estouro
        if (spriteRenderer != null && popSprite != null)
        {
            spriteRenderer.sprite = popSprite;
        }

        PlayPopSound(); // Reproduz o som de estouro antes de destruir
        OnBubbleDestroyed?.Invoke(); // Notifica o spawner que a bolha foi destruída
        Destroy(gameObject, 0.5f); // Adiciona um pequeno delay para reproduzir o som
    }

    private void PlayPopSound()
    {
        if (audioSource != null && popSound != null)
        {
            audioSource.PlayOneShot(popSound); // Reproduz o som do estouro
        }
    }

    // Chamado quando o jogador está em cima da bolha
    public void SetPlayerOnBubble()
    {
        if (spriteRenderer != null && playerOnBubbleSprite != null)
        {
            spriteRenderer.sprite = playerOnBubbleSprite;
        }
    }

    // Chamado quando o jogador deforma a bolha para a direita
    public void SetDeformRight()
    {
        if (spriteRenderer != null && deformSprite != null)
        {
            spriteRenderer.sprite = deformSprite;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Chamado quando o jogador deforma a bolha para a esquerda
    public void SetDeformLeft()
    {
        if (spriteRenderer != null && deformSprite != null)
        {
            spriteRenderer.sprite = deformSprite;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Chamado para voltar ao estado normal (sem jogador ou deformação)
    public void SetNormal()
    {
        if (spriteRenderer != null && normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
