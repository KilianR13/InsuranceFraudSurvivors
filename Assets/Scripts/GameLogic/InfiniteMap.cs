using UnityEngine;

/// <summary>
/// Class that handles the logic of the visual effect to make the player believe the map is infinite.
/// </summary>
public class InfiniteMap : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Map Pattern prefab")]
    [SerializeField] private GameObject mapPattern;

    [SerializeField] private float patternWidth = 32f;
    [SerializeField] private float patternHeight = 28f;

    [Header("Grid size")]
    [SerializeField] private int gridSize = 3;

    private GameObject[,] patterns;

    private int currentX;
    private int currentY;

    private void Start()
    {
        patterns = new GameObject[gridSize, gridSize];

        CreateMap();

        currentX = Mathf.FloorToInt(player.position.x / patternWidth);
        currentY = Mathf.FloorToInt(player.position.y / patternHeight);
    }

    private void Update()
    {
        int newX = Mathf.FloorToInt(player.position.x / patternWidth);
        int newY = Mathf.FloorToInt(player.position.y / patternHeight);

        if (newX != currentX || newY != currentY)
        {
            MoveMap(newX, newY);

            currentX = newX;
            currentY = newY;
        }
    }

    private void CreateMap()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject pattern = Instantiate(mapPattern, transform);

                float posX = (x - 1) * patternWidth;
                float posY = (y - 1) * patternHeight;

                pattern.transform.position = new Vector3(posX, posY, 0f);

                patterns[x, y] = pattern;
            }
        }
    }

    private void MoveMap(int playerX, int playerY)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                int worldX = playerX + (x - 1);
                int worldY = playerY + (y - 1);

                patterns[x, y].transform.position = new Vector3(
                    worldX * patternWidth,
                    worldY * patternHeight,
                    0f
                );
            }
        }
    }
}