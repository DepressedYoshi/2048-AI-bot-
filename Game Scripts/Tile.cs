using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public static int score = 0;  // Static score across all tiles
    private int value;  // The current value of the tile, private to ensure controlled access
    private TextMeshProUGUI textMesh;  // TextMeshPro for UI element

    public int Value {
        get => value;
        private set {
            this.value = value;
            UpdateText();  // Update text whenever value changes
            UpdateScore(value);  // Update score based on the tile value set
        }
    }
    public bool MergedThisTurn { get; set; }

    public bool IsActive => gameObject.activeSelf;  // Public property to check if the tile is active

    void Awake()
    {
        // Initialize TextMeshProUGUI on Awake to ensure it's ready before Start methods of other scripts
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshProUGUI component is missing on the Tile or its children!");
        }
    }

    void Start()
    {
        SetValue(Random.value > 0.9 ? 4 : 2);  // Randomly initialize tile value to 2 or 4 at start
    }

    // Method to update the text displayed on the tile
    private void UpdateText()
    {
        if (textMesh != null)
            textMesh.text = Value.ToString();
        else
            Debug.LogError("TextMeshPro component not found on the tile!");
    }

    // Method to set the value and update the text
    public void SetValue(int newValue)
    {
        if (IsActive)
        {
            Value = newValue;
        }
        else
        {
            Debug.LogError("Attempt to set value on an inactive Tile!");
        }
    }

    // Method to clear the tile (used when merging tiles)
    public void Clear()
    {
        Value = 0;
        gameObject.SetActive(false);  // Deactivate the tile GameObject
        MergedThisTurn = false;  // Reset merged flag
    }

    // Method to update the score
    private void UpdateScore(int points)
    {
        score += points;
        Debug.Log("Current Score: " + score);  // Display the current score in the debug log
    }
}
