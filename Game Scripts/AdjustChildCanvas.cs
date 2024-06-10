using UnityEngine;

public class AdjustCanvasToParent : MonoBehaviour
{
    private RectTransform parentRectTransform;
    private RectTransform canvasRectTransform;

    void Start()
    {
        parentRectTransform = GetComponent<RectTransform>();
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
            AdjustCanvasSize();
        }
        else
        {
            Debug.LogError("No Canvas found as a child of " + gameObject.name);
        }
    }

    void AdjustCanvasSize()
    {
        // Match the canvas size to the parent size
        if (canvasRectTransform != null && parentRectTransform != null)
        {
            canvasRectTransform.sizeDelta = parentRectTransform.sizeDelta;
            canvasRectTransform.localScale = Vector3.one;
            canvasRectTransform.anchoredPosition = Vector2.zero;

            // Optional: Set anchor presets to stretch in all directions
            canvasRectTransform.anchorMin = Vector2.zero;
            canvasRectTransform.anchorMax = Vector2.one;
            canvasRectTransform.offsetMin = Vector2.zero;
            canvasRectTransform.offsetMax = Vector2.zero;
        }
    }
}
