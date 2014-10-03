using UnityEngine;
using System;
using System.Collections.Generic;

namespace Maze.Game 
{
    public class MapRenderer : MonoBehaviour
    {
        public struct Position
        {
            public int x;
            public int y;

            public Position(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        [System.Serializable]
        public struct TileMaterial
        {
            [SerializeField]
            public string TileName;
            [SerializeField]
            public Material Mat;
        }

        public List<TileMaterial> TileMaterials;

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

        public Material FindTileMaterial(Tile tile)
        {
            foreach (TileMaterial tileMat in TileMaterials)
            {
                if (tileMat.TileName == tile.Name)
                {
                    return tileMat.Mat;
                }
            }
            return null;
        }

        protected void RenderTiles(Tile tile, List<Position> tilePositions)
        {
            GameObject baseObj = new GameObject();
            baseObj.AddComponent<MeshFilter>();
            baseObj.AddComponent<MeshRenderer>();

            MeshRenderer renderer = baseObj.GetComponent<MeshRenderer>();
            Material tileMat = FindTileMaterial(tile);
            if (tileMat != null)
            {
                renderer.material = tileMat;
            }
            baseObj.transform.parent = transform;

            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[tilePositions.Count * 4];
            Vector2[] uvs = new Vector2[vertices.Length];
            int[] triangles = new int[tilePositions.Count * 6];

            int vCount = 0;
            int tCount = 0;
            foreach (Position pos in tilePositions)
            {
                TileInstance inst = CurrentMap.Tiles[pos.x, pos.y];
                float yTop = pos.y * GridSize;
                float yBottom = yTop + GridSize;
                float xLeft = pos.x * GridSize;
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
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            baseObj.GetComponent<MeshFilter>().mesh = mesh;
        }

        public void RenderMap()
        {
            if (CurrentMap == null)
            {
                Debug.Log("No map set onto map renderer.");
                return;
            }
            reRenderMap = false;

            Dictionary<Tile, List<Position>> tileToPositions = new Dictionary<Tile, List<Position>>();
            int width = CurrentMap.Width;
            int height = CurrentMap.Height;

            Tile currentTile = null;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    TileInstance inst = CurrentMap.Tiles[x, y];
                    if (inst.BaseTile != currentTile)
                    {
                        currentTile = inst.BaseTile;
                        if (!tileToPositions.ContainsKey(currentTile))
                        {
                            tileToPositions[currentTile] = new List<Position>();
                        }
                    }
                    tileToPositions[currentTile].Add(new Position(x, y));
                }
            }

            foreach (var pairs in tileToPositions)
            {
                RenderTiles(pairs.Key, pairs.Value);
            }

        }
    }
}
