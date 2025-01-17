using System.Collections;
using UnityEngine;

public class ObstacleMovementLoops : MonoBehaviour
{
    public Transform StartPos;
    public Transform EndPos;
    public Transform Object;
    public float duration = 1f;
    public int segmentCount = 10; // Number of segments for the line
    public float oscillationAmplitude = 1f; // Amplitude of the vertical oscillation
    public float oscillationFrequency = 2f; // Frequency of the vertical oscillation

    private float startTime;
    private bool forward = true;

    private AnimationCurve easeCurve;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time; // Initialize the start time
        easeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }

    private float vertical_function(float x)
    {
        return -8 * x * x + 8 * x;
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

        // Evaluate the easing curve
        float easedPer = easeCurve.Evaluate(per);

        // Linear interpolation for horizontal movement
        Vector3 linearPosition = Vector3.Lerp(StartPos.position, EndPos.position, easedPer);


        // Apply the combined position
        Object.position = new Vector3(linearPosition.x, vertical_function(per), linearPosition.z);
    }

    void DrawSegmentedLine(Vector3 start, Vector3 end, int segments)
    {
        Gizmos.color = Color.blue;
        // Calculate the segment length
        for (int i = 0; i < segments; i++)
        {
            // Skip every other segment to create spaces
            if (i % 2 == 0)
            {
                float t1 = (float)i / segments;
                float t2 = (float)(i + 1) / segments;

                // Get the start and end points of the segment
                Vector3 point1 = Vector3.Lerp(start, end, t1);
                Vector3 point2 = Vector3.Lerp(start, end, t2);

                // Draw the segment
                Gizmos.DrawLine(point1, point2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawSegmentedLine(StartPos.position, EndPos.position, segmentCount);

        Gizmos.color = Color.gray;
        Gizmos.DrawCube(StartPos.position, Vector3.one / 8);
        Gizmos.DrawCube(EndPos.position, Vector3.one / 8);
    }
}
