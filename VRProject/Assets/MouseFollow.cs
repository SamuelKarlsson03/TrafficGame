using UnityEngine;

public class MouseFollow : MonoBehaviour
{

    void Update()
    {
        // Capture the current mouse position in screen coordinates.
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position from screen coordinates to world coordinates.
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10.0f));

        // Set the GameObject's position to the calculated worldMousePosition, ignoring the Y axis.
        transform.position = new Vector3(worldMousePosition.x, transform.position.y, worldMousePosition.z);
    }
}
