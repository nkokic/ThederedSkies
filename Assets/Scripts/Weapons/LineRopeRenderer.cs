using UnityEngine;

public class LineRopeRenderer : IRopeRenderer
{
    public void RenderRope(Vector2 startPoint, Vector2 endPoint, LineRenderer lineRenderer)
    {
        Vector3[] y = { startPoint, endPoint };
        lineRenderer.SetPositions(y);
    }
}

