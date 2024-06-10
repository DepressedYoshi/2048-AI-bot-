using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 4;
    public float tileSize = 1f;
    public float tileSpacing = 0.1f;
    public Transform gameBoard;
    private Tile[,] tileScripts;

    private void Start()
    {
        InitializeTiles();
        SpawnRandomTiles(2); // Spawn two initial tiles at the start of the game
    }

    private void InitializeTiles()
    {
        tileScripts = new Tile[gridSize, gridSize];

        float startOffset = -(gridSize - 1) * (tileSize + tileSpacing) / 2;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Vector3 localPos = new Vector3(startOffset + col * (tileSize + tileSpacing), startOffset + row * (tileSize + tileSpacing), 0);
                GameObject tile = Instantiate(tilePrefab, localPos, Quaternion.identity, gameBoard);
                tile.name = $"Tile_{row}_{col}";
                tile.SetActive(false);
                tileScripts[row, col] = tile.GetComponent<Tile>();
            }
        }
    }

     public void SpawnRandomTiles(int count)
    {
        List<int> availablePositions = new List<int>();
        for (int i = 0; i < gridSize * gridSize; i++)
        {
            availablePositions.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, availablePositions.Count);
            int flatIndex = availablePositions[index];
            availablePositions.RemoveAt(index);
            int row = flatIndex / gridSize;
            int col = flatIndex % gridSize;

            Tile tile = tileScripts[row, col];
            tile.gameObject.SetActive(true);
            tile.SetValue(Random.value > 0.9 ? 4 : 2);
        }
    }

    public void MoveTiles(Vector2Int direction)
    {
        bool moved = false;
        var traversal = GetTraversalPath(direction);

        foreach (var pos in traversal)
        {
            moved |= MoveTile(pos.x, pos.y, direction);
        }

        if (moved)
            SpawnRandomTiles(1);
    }

    public bool MoveTile(int row, int col, Vector2Int direction)
    {
        Tile tile = tileScripts[row, col];
        if (tile == null || !tile.IsActive) return false;

        bool moved = false;
        int nextRow = row + direction.y;
        int nextCol = col + direction.x;

        while (IsWithinBounds(nextRow, nextCol))
        {
            Tile nextTile = tileScripts[nextRow, nextCol];
            if (nextTile != null && nextTile.IsActive)
            {
                if (CanMergeTiles(tile, nextTile))
                {
                    MergeTiles(tile, nextTile);
                    moved = true;
                    break;
                }
                break; // Cannot move any further
            }
            else if (nextTile != null && !nextTile.IsActive)
            {
                SwapTiles(tile, nextTile);
                moved = true;
                row = nextRow;
                col = nextCol;
                nextRow += direction.y;
                nextCol += direction.x;
            }
        }
        return moved;
    }

    private bool CanMergeTiles(Tile current, Tile next)
    {
        return next.Value == current.Value && !next.MergedThisTurn && !current.MergedThisTurn;
    }

    private void MergeTiles(Tile current, Tile next)
    {
        next.SetValue(next.Value * 2);
        next.MergedThisTurn = true;
        current.Clear();
    }

    private void SwapTiles(Tile current, Tile next)
    {
        tileScripts[next.Row, next.Col] = current;
        tileScripts[current.Row, current.Col] = next;

        Tile temp = next;
        next = current;
        current = temp;

        Vector3 tempPosition = current.transform.position;
        current.transform.position = next.transform.position;
        next.transform.position = tempPosition;
    }
    

    private bool IsWithinBounds(int row, int col)
    {
        return row >= 0 && row < gridSize && col >= 0 && col < gridSize;
    }

    private IEnumerable<Vector2Int> GetTraversalPath(Vector2Int direction)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                int x = direction.x == 1 ? gridSize - 1 - col : col;
                int y = direction.y == 1 ? gridSize - 1 - row : row;
                path.Add(new Vector2Int(x, y));
            }
        }
        return path;
    }


    public void ResetTileStates(){
    for (int row = 0; row < gridSize; row++)
    {
        for (int col = 0; col < gridSize; col++)
        {
            Tile tile = tileScripts[row, col];
            if (tile != null && tile.IsActive)
            {
                tile.MergedThisTurn = false;
            }
        }
    }
}

}
