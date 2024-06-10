using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GridManager gridManager;

    void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.UpArrow))
        ProcessInput(Vector2Int.up);
    else if (Input.GetKeyDown(KeyCode.DownArrow))
        ProcessInput(Vector2Int.down);
    else if (Input.GetKeyDown(KeyCode.LeftArrow))
        ProcessInput(Vector2Int.left);
    else if (Input.GetKeyDown(KeyCode.RightArrow))
        ProcessInput(Vector2Int.right);
}

void ProcessInput(Vector2Int direction)
{
    gridManager.ResetTileStates();
    gridManager.MoveTiles(direction);
}

}
