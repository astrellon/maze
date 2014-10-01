using UnityEngine;
using System;

namespace Maze.Game 
{
    public class MapRenderer : MonoBehaviour
    {
        public Map CurrentMap = null;
        public float GridSize = 1.0f;
        protected bool reRenderMap = false;

        void Start()
        {

        }

        void Update()
        {
            if (reRenderMap)
            {
                reRenderMap = false;
                RenderMap();
            }
        }

        public void FlagRenderMap()
        {
            reRenderMap = true;
        }

        public void RenderMap()
        {
            if (CurrentMap == null)
            {
                Debug.Log("No map set onto map renderer.");
                return;
            }
            reRenderMap = false;

            Mesh mesh = new Mesh();
            int width = CurrentMap.Width;
            int height = CurrentMap.Height;

            Vector3[] vertices = new Vector3[width * height * 4];
            Vector2[] uvs = new Vector2[vertices.Length];
            int[] triangles = new int[width * height * 6];

            int vCount = 0;
            int tCount = 0;
            for (int y = 0; y < height; y++)
            {
                float yTop = y * GridSize;
                float yBottom = yTop + GridSize;
                
                for (int x = 0; x < width; x++)
                {
                    TileInstance inst = CurrentMap.Tiles[x, y];
                    float xLeft = x * GridSize;
                    float xRight = xLeft + GridSize;

                    vertices[vCount    ] = new Vector3(xLeft,  inst.Heights[0], yBottom);
                    vertices[vCount + 1] = new Vector3(xRight, inst.Heights[1], yBottom);
                    vertices[vCount + 2] = new Vector3(xLeft,  inst.Heights[2], yTop);
                    vertices[vCount + 3] = new Vector3(xRight, inst.Heights[3], yTop);

                    uvs[vCount    ] = new Vector2(0, 0);
                    uvs[vCount + 1] = new Vector2(1, 0);
                    uvs[vCount + 2] = new Vector2(0, 1);
                    uvs[vCount + 3] = new Vector2(1, 1);

                    triangles[tCount    ] = vCount;
                    triangles[tCount + 1] = vCount + 1;
                    triangles[tCount + 2] = vCount + 2;
                    
                    triangles[tCount + 3] = vCount + 3;
                    triangles[tCount + 4] = vCount + 2;
                    triangles[tCount + 5] = vCount + 1;

                    vCount += 4;
                    tCount += 6;
                }
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}
