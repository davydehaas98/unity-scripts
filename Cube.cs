using UnityEngine;

public class Cube : MonoBehaviour
{
    public float height = 1, width = 1, depth = 1;
    public float xSpeed = 10, ySpeed = 20, zSpeed;
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

             
        mesh.vertices = vertices = new Vector3[] {
            new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f),  //front
            new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f),      //back
            new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f),      //right
            new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f),  //left
            new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, -0.5f),      //top
            new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f)   //bottom
        };

        mesh.triangles = new int[] {
            0, 1, 2,
            0, 2, 3,
            4, 5, 6,
            4, 6, 7,
            8, 9, 10,
            8, 10, 11,
            12, 13, 14,
            12, 14, 15,
            16, 17, 18,
            16, 18, 19,
            20, 22, 21,
            20, 23, 22
        };

        mesh.normals = new Vector3[] {
            new Vector3( 0, 0,-1), new Vector3( 0, 0,-1), new Vector3( 0, 0,-1), new Vector3( 0, 0,-1), // front
            new Vector3( 0, 0, 1), new Vector3( 0, 0, 1), new Vector3( 0, 0, 1), new Vector3( 0, 0, 1), // back
            new Vector3( 1, 0, 0), new Vector3( 1, 0, 0), new Vector3( 1, 0, 0), new Vector3( 1, 0, 0), // right
            new Vector3(-1, 0, 0), new Vector3(-1, 0, 0), new Vector3(-1, 0, 0), new Vector3(-1, 0, 0), // left
            new Vector3( 0, 1, 0), new Vector3( 0, 1, 0), new Vector3( 0, 1, 0), new Vector3( 0, 1, 0), // top
            new Vector3( 0,-1, 0), new Vector3( 0,-1, 0), new Vector3( 0,-1, 0), new Vector3( 0,-1, 0)  // bottom
        };
        
        mesh.uv = new Vector2[] {
            new Vector2(0f, 0f),      new Vector2(0.33f, 0),    new Vector2(0.33f,0.5f),  new Vector2 (0, 0.5f),     // front
            new Vector2(0.66f, 0.5f), new Vector2(1f, 0.5f),    new Vector2(1, 1),        new Vector2 (0.66f, 1),    // back
            new Vector2(0.66f, 0),    new Vector2(1, 0),        new Vector2(1, 0.5f),     new Vector2 (0.66f, 0.5f), // right
            new Vector2(0, 0.5f),     new Vector2(0.33f, 0.5f), new Vector2(0.33f, 1),    new Vector2 (0, 1),        // left
            new Vector2(0.33f, 0),    new Vector2(0.66f, 0),    new Vector2(0.66f, 0.5f), new Vector2 (0.33f, 0.5f), // top
            new Vector2(0.33f, 0.5f), new Vector2(0.66f, 0.5f), new Vector2(0.66f, 1),    new Vector2 (0.33f, 1)     // bottom
        };
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

