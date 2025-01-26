using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip popSound; // Som do estouro
    private AudioSource audioSource; // Fonte de áudio
    public delegate void BubbleDestroyed();
    public BubbleDestroyed OnBubbleDestroyed;

    private float lifetime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Referência ao AudioSource
    }

    public void Initialize(float size)
    {
        lifetime = size * 5f; // Tempo de vida é proporcional ao tamanho
        Invoke(nameof(DestroyBubble), lifetime); // Destrói a bolha após o tempo de vida
    }

    public void MakeGhost()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
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
        PlayPopSound(); // Reproduz o som de estouro antes de destruir
        OnBubbleDestroyed?.Invoke(); // Notifica o spawner que a bolha foi destruída
        Destroy(gameObject, 0.1f); // Adiciona um pequeno delay para reproduzir o som
    }

    private void PlayPopSound()
    {
        if (audioSource != null && popSound != null)
        {
            audioSource.PlayOneShot(popSound); // Reproduz o som do estouro
        }
    }
}
