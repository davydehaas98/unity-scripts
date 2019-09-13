using UnityEngine;

public class Cone : MonoBehaviour
{
    public float xSpeed, ySpeed, zSpeed;
    public int radius = 1, height = 1;

    private int steps = 30;
    private Mesh mesh;
    private Vector3[] vertices;

    private void Awake()
    {
        Generate();
    }

    private void Update()
    {
        transform.Rotate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime);
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        
        // Vertices
        vertices = new Vector3[steps + 2];

        for (int i = 0; i < steps; i++)
        {
            vertices[i] = new Vector3(Mathf.Cos(i * Mathf.PI * 2 / steps) * radius, 0, Mathf.Sin(i * Mathf.PI * 2 / steps) * radius);
        }

        vertices[steps] = new Vector3(0, 0, 0);
        vertices[steps + 1] = new Vector3(0, height, 0);

        mesh.vertices = vertices;

        // Triangles
        int[] triangles = new int[steps * 3 * 2 + 2 * 3];

        for (int i = 0, j = 0; i < steps - 1; i++)
        {
            triangles[j] = i;
            j++;
            triangles[j] = i + 1;
            j++;
            triangles[j] = steps;
            j++;
            triangles[j] = i + 1;
            j++;
            triangles[j] = i;
            j++;
            triangles[j] = steps + 1;
            j++;
        }

        triangles[180] = steps - 1;
        triangles[181] = 0;
        triangles[182] = steps;
        triangles[183] = 0;
        triangles[184] = steps - 1;
        triangles[185] = steps + 1;

        mesh.triangles = triangles;

        // Normals
        Vector3[] normals = new Vector3[steps + 2];

        for (int i = 0; i < steps; i++)
        {
            normals[i] = new Vector3(Mathf.Cos(i * Mathf.PI * 2 / steps), Mathf.Tan(height / radius + (Mathf.PI / 2)), Mathf.Sin(i * Mathf.PI * 2 / steps));
        }
        
        normals[30] = new Vector3(0, -1, 0);
        normals[31] = new Vector3(0, 1, 0);
        
        mesh.normals = normals;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;

        Gizmos.color = Color.black;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
