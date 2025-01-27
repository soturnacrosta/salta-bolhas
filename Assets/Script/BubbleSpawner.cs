using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject bubblePrefab; // Prefab da bolha
    public GameObject playerPrefab; // Prefab do jogador
    public float playerSpawnMinX = -8f; // Limite inferior do eixo X para o spawn
    public float playerSpawnMaxX = 8f; // Limite superior do eixo X para o spawn
    public float playerSpawnMinY = -4f; // Limite inferior do eixo Y para o spawn
    public float playerSpawnMaxY = 4f; // Limite superior do eixo Y para o spawn
    public float spawnRadius = 5f; // Raio m�ximo ao redor do jogador para spawnar bolhas
    public float spawnInterval = 2f; // Intervalo entre a cria��o de novas bolhas
    public int quantbolhas = 10;
    public int minActiveBubbles = 5; // N�mero m�nimo de bolhas ativas
    public float minBubbleDistance = 1.5f; // Dist�ncia m�nima entre as bolhas

    [Header("UI Elements")]
    

    private float timer; // Controlador de tempo para novas bolhas
    private List<GameObject> activeBubbles = new List<GameObject>(); // Lista de bolhas ativas
    private GameObject player; // Refer�ncia ao jogador

    void Start()
    {
     

        // Gera as bolhas iniciais e o jogador assim que o jogo come�a
        SpawnInitialSetup();
    }

    void Update()
    {
        // Controla o spawn de novas bolhas ap�s o setup inicial
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            if (activeBubbles.Count < minActiveBubbles)
            {
                SpawnBubble();
            }
            timer = 0f;
        }

        // Remove refer�ncias de bolhas destru�das da lista
        activeBubbles.RemoveAll(bubble => bubble == null);
    }

    void SpawnInitialSetup()
    {
        for (int i = 0; i < quantbolhas; i++)
        {
            Vector3 position = GenerateRandomPosition(Vector3.zero); // Posi��o inicial aleat�ria
            GameObject bubble = Instantiate(bubblePrefab, position, Quaternion.identity);

            ConfigureBubble(bubble);
            activeBubbles.Add(bubble);
        }

        // Escolhe uma bolha para spawnar o jogador
        int randomIndex = Random.Range(0, activeBubbles.Count);
        GameObject chosenBubble = activeBubbles[randomIndex];
        Vector3 playerSpawnPosition = chosenBubble.transform.position;

        float bubbleRadius = chosenBubble.transform.localScale.y / 2f;
        playerSpawnPosition.y += bubbleRadius + 0.1f;

        player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity); // Armazena o jogador
    }

    void SpawnBubble()
    {
        if (player == null) return;

        Vector3 position = GenerateRandomPosition(player.transform.position); // Gera posi��o ao redor do jogador
        GameObject bubble = Instantiate(bubblePrefab, position, Quaternion.identity);

        ConfigureBubble(bubble);
        activeBubbles.Add(bubble);
    }

    Vector3 GenerateRandomPosition(Vector3 center)
    {
        // Gera posi��o aleat�ria dentro do raio do jogador
        float x = Random.Range(center.x - spawnRadius, center.x + spawnRadius);
        float y = Random.Range(center.y - spawnRadius, center.y + spawnRadius);

        // Garante que a posi��o est� dentro dos limites globais
        x = Mathf.Clamp(x, playerSpawnMinX, playerSpawnMaxX);
        y = Mathf.Clamp(y, playerSpawnMinY, playerSpawnMaxY);

        return new Vector3(x, y, 0);
    }

    void ConfigureBubble(GameObject bubble)
    {
        Bubble bubbleScript = bubble.GetComponent<Bubble>();
        if (bubbleScript != null)
        {
            float size = Random.Range(0.2f, 1f);
            bubble.transform.localScale = new Vector3(size, size, 1);
            bubbleScript.Initialize(size);

            if (Random.value < 0.2f)
            {
                bubbleScript.MakeGhost();
            }

            // Callback para eventos relacionados � bolha
            bubbleScript.OnBubbleDestroyed = () =>
            {
                activeBubbles.Remove(bubble);

                // Incrementa a pontua��o no GameManager
                GameManager.Instance.AddScore(10);

                // Garante que sempre h� bolhas ao redor do jogador
                if (activeBubbles.Count < minActiveBubbles)
                {
                    SpawnBubble();
                }

                // Verifica se n�o h� bolhas restantes
                if (activeBubbles.Count == 0)
                {
                    GameManager.Instance.GameOver(); // Chama o Game Over
                }
            };
        }
    }


 


}
