using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Use this if you're using TextMeshPro

public class ResizeBackground : MonoBehaviour
{
    public TextMeshProUGUI textComponent;  // Reference to the TextMeshPro component
    public RectTransform imageRectTransform;  // Reference to the Image's RectTransform
    public float padding = 75f;  // Padding around the text (in pixels)

    void Update()
    {
        // Get the preferred size of the text
        Vector2 textSize = new Vector2(textComponent.preferredWidth, textComponent.preferredHeight + 50);

        // Add padding (both width and height)
        Vector2 newSize = textSize + new Vector2(padding, padding);

        // Set the size of the Image's RectTransform
        imageRectTransform.sizeDelta = newSize;
    }
}
