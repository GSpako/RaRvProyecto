using System.Collections;
using UnityEngine;

public class ObstacleMovementLoops : MonoBehaviour
{
    public Transform StartPos;
    public Transform EndPos;
    public Transform Object;
    public float duration = 1f;
    public int segmentCount = 30; // Number of segments for the line
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

    private float horizontal_function(float x)
    {
        float a = 14.625f;
        float b = -23.25f;
        float c = 10.625f;
        float d = -1.0f;

        return a * x * x * x + b * x * x + c * x + d;
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

        float easedper = easeCurve.Evaluate(per);

        // Apply the combined position
        Object.position = new Vector3(horizontal_function(easedper), vertical_function(easedper), StartPos.position.z);
    }

    void DrawSegmentedLine(Vector3 start, Vector3 end, int segments)
    {
        Gizmos.color = Color.blue;
        // Calculate the segment length
        for (int i = 0; i < segments; i++)
        {
            // Skip every other segment to create spaces
            //if (i % 2 == 0)
            //{
                float t1 = (float)i / segments;
                float t2 = (float)(i + 1) / segments;

                Vector3 point1 = Vector3.Lerp(start, end, t1);
                point1.x = horizontal_function(t1);
                point1.y = vertical_function(t1);
                Vector3 point2 = Vector3.Lerp(start, end, t2);
                point2.x = horizontal_function(t2);
                point2.y = vertical_function(t2);

                // Draw the segment
                Gizmos.DrawLine(point1, point2);
            //}
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
