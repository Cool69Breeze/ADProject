using UnityEngine;

public class Circle : MonoBehaviour
{
    public int segments = 64;  // Количество сегментов для приближения круга
    public float radius = 1f;

    void Start()
    {
        CreateCircle();
    }

    void CreateCircle()
    {
        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.positionCount = segments + 1;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        float deltaTheta = (2f * Mathf.PI) / segments;
        float theta = 0f;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            line.SetPosition(i, new Vector3(x, y, 0));
            theta += deltaTheta;
        }
    }
}
