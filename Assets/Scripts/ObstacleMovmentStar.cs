using System.Collections;
using UnityEngine;

public class ObstacleMovementStar : MonoBehaviour
{
    public Transform StartPos;
    public Transform EndPos;
    public Transform Object;
    public float duration = 1f; // Time to complete one loop
    public int points = 5; // Number of star points
    public float outerRadius = 2.5f; // Radius for the starheads
    public float innerRadius = 0.5f; // Radius for the inner points of the star

    private float startTime;
    private bool forward = true;

    private AnimationCurve easeCurve;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time; // Initialize the start time
        easeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    }

    private Vector3 StarFunction(float t)
    {
        // Total number of star points (outer + inner)
        int totalVertices = points * 2;

        // Determine which vertex we're interpolating towards
        float progress = t * totalVertices; // Map t (0-1) to vertices
        int currentVertex = Mathf.FloorToInt(progress); // Current vertex index
        float localT = progress - currentVertex; // Local progress between two vertices

        // Calculate angle for the current and next vertex
        float angleStep = Mathf.PI * 2 / totalVertices;
        float currentAngle = currentVertex * angleStep + Mathf.PI / 8;
        float nextAngle = (currentVertex + 1) * angleStep + Mathf.PI / 8;

        // Determine radius for current and next vertex (outer vs. inner)
        float currentRadius = (currentVertex % 2 == 0) ? outerRadius : innerRadius;
        float nextRadius = ((currentVertex + 1) % 2 == 0) ? outerRadius : innerRadius;

        // Calculate current and next positions
        Vector3 currentPos = new Vector3(
            Mathf.Cos(currentAngle) * currentRadius,
            Mathf.Sin(currentAngle) * currentRadius,
            0f
        );

        Vector3 nextPos = new Vector3(
            Mathf.Cos(nextAngle) * nextRadius,
            Mathf.Sin(nextAngle) * nextRadius,
            0f
        );

        // Interpolate between current and next vertex
        Vector3 interpolatedPos = Vector3.Lerp(currentPos, nextPos, localT);

        return new Vector3(interpolatedPos.x, interpolatedPos.y +2f, StartPos.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float elapsed = (Time.time - startTime) / duration;
        float per = forward ? elapsed : 1f - elapsed;

        // Switch direction if we reach the end
        if (elapsed >= 1f)
        {
            forward = !forward;
            startTime = Time.time; // Reset start time
        }

        // Clamp progress between 0 and 1
        per = Mathf.Clamp01(per);

        float easedPer = easeCurve.Evaluate(per);

        // Update position along the star path
        Object.position = StarFunction(easedPer);
    }

    private void DrawStarPath()
    {
        Gizmos.color = Color.blue;

        // Draw the star path using line segments
        int segments = points * 20; // High resolution for smooth drawing
        for (int i = 0; i < segments; i++)
        {
            float t1 = (float)i / segments;
            float t2 = (float)(i + 1) / segments;

            Vector3 point1 = StarFunction(t1);
            Vector3 point2 = StarFunction(t2);

            Gizmos.DrawLine(point1, point2);
        }
    }

    private void OnDrawGizmos()
    {
        DrawStarPath();

        Gizmos.color = Color.gray;
        Gizmos.DrawCube(StartPos.position, Vector3.one / 8);
        Gizmos.DrawCube(EndPos.position, Vector3.one / 8);
    }
}
