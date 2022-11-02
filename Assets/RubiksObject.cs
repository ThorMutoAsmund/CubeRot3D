using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR;

public class RubiksObject : MonoBehaviour
{
    public Material yellowMaterial;
    public Material orangeMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material redMaterial;
    public Material whiteMaterial;
    public Material insideMaterial;

    const int backFace = 0;
    const int rightFace = 1;
    const int topFace = 2;
    const int bottomFace = 3;
    const int leftFace = 4;
    const int frontFace = 5;

    private static float edgeSize = 0.5f;
    private static Vector3[] vertexList = {
        new Vector3(-edgeSize, -edgeSize, -edgeSize),
        new Vector3(-edgeSize, edgeSize, -edgeSize),
        new Vector3(edgeSize, edgeSize, -edgeSize),
        new Vector3(edgeSize, -edgeSize, -edgeSize),
        new Vector3(edgeSize, -edgeSize, edgeSize),
        new Vector3(edgeSize, edgeSize, edgeSize),
        new Vector3(-edgeSize, edgeSize, edgeSize),
        new Vector3(-edgeSize, -edgeSize, edgeSize)
    };
    private static int[] faceList = {
        0, 1, 2, //   1: face arrière
        0, 2, 3,
        3, 2, 5, //   2: face droite
        3, 5, 4,
        5, 2, 1, //   3: face dessue
        5, 1, 6,
        3, 4, 7, //   4: face dessous
        3, 7, 0,
        0, 7, 6, //   5: face gauche
        0, 6, 1,
        4, 5, 6, //   6: face avant
        4, 6, 7
    };
    private static Vector2[] newUVs = {
        new Vector2(0, 1),
        new Vector2(0, 1),
        new Vector2(0, 1),
        new Vector2(0, 1),
        new Vector2(0, 1),
        new Vector2(0, 1),
        new Vector2(0, 1),
        new Vector2(0, 1)
    };

    // Start is called before the first frame update
    void Start()
    {
        MakeCube();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MakeCube()
    {
        for (var y = 1; y >= -1; --y)
        {
            for (var z = 1; z >= -1; --z)
            {
                for (var x = -1; x <= 1;  ++x)
                {
                    var miniCube = MakeMiniCube(y, z, x);
                    miniCube.name = $"Cubie {x}-{y}-{z}";
                    miniCube.transform.SetParent(this.transform);
                    miniCube.transform.position = new Vector3(x, y, -z);
                }
            }
        }
    }

    private GameObject MakeMiniCube(int y, int z, int x)
    {
        var go = new GameObject();
        var meshRenderer = go.AddComponent<MeshRenderer>();
        var meshFilter = go.AddComponent<MeshFilter>();
        var mesh = new Mesh();
        mesh.vertices = vertexList;
        mesh.uv = newUVs;
        mesh.subMeshCount = 6;

        for (int a = 0; a < 6; ++a)
        {
            var triangles = new int[6];
            //var uvs = new Vector2[6] { new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1)};
            for (int v = 0; v < 6; ++v)
            {
                triangles[v] = faceList[v + 6 * a];
            }
            mesh.SetTriangles(triangles, a);
        }

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        meshRenderer.materials = new Material[6] {
            z == 1 ? this.orangeMaterial : this.insideMaterial,
            x == 1 ? this.blueMaterial : this.insideMaterial,
            y == 1 ? this.yellowMaterial : this.insideMaterial,
            y == -1 ? this.whiteMaterial : this.insideMaterial,
            x == -1 ? this.greenMaterial : this.insideMaterial,
            z == -1 ? this.redMaterial: this.insideMaterial
        };        

        //{ this.redMaterial,this.blueMaterial,this.yellowMaterial,this.whiteMaterial,this.greenMaterial,this.orangeMaterial };

        return go;
    }
}
