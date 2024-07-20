using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OutlineBoxCollider2D : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 5; // 4 corners + start point
            lineRenderer.loop = true;
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
            UpdateOutlinePositions();
        }

        lineRenderer.enabled = false; // Initially hide the outline
    }

    void UpdateOutlinePositions()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            Vector2 size = collider.size;
            Vector2 offset = collider.offset;
            Vector2 center = (Vector2)transform.position + offset;

            Vector3 bottomLeft = new Vector3(center.x - size.x / 2, center.y - size.y / 2, 0);
            Vector3 bottomRight = new Vector3(center.x + size.x / 2, center.y - size.y / 2, 0);
            Vector3 topLeft = new Vector3(center.x - size.x / 2, center.y + size.y / 2, 0);
            Vector3 topRight = new Vector3(center.x + size.x / 2, center.y + size.y / 2, 0);

            lineRenderer.SetPosition(0, bottomLeft);
            lineRenderer.SetPosition(1, bottomRight);
            lineRenderer.SetPosition(2, topRight);
            lineRenderer.SetPosition(3, topLeft);
            lineRenderer.SetPosition(4, bottomLeft); // Closing the loop
        }
    }

    public void ShowOutline()
    {
        lineRenderer.enabled = true;
    }

    public void HideOutline()
    {
        lineRenderer.enabled = false;
    }
}
