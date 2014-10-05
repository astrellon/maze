using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game 
{
    public class Map
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public World OwnerWorld { get; protected set; }

        public TileInstance[,] Tiles { get; protected set; }
        public string BaseName { get; protected set; }

        public Map(string baseName, World world)
        {
            BaseName = baseName;
            OwnerWorld = world;
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;

            Tiles = new TileInstance[Width, Height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tiles[x, y] = new TileInstance();
                }
            }
        }

        public bool Deserialise(Dictionary<string, object> obj)
        {
            if (obj == null)
            {
                Debug.Log("Unable to deserialise null for map");
                return false;
            }

            int width = 0;
            int height = 0;
            if (obj.ContainsKey("width"))
            {
                width = Convert.ToInt32(obj["width"]);
            }
            if (obj.ContainsKey("height"))
            {
                height = Convert.ToInt32(obj["height"]);
            }
            if (width == 0 || height == 0)
            {
                Debug.Log("Unable to read width and height data from map data");
                return false;
            }
            
            List<object> data = null;
            if (obj.ContainsKey("data"))
            {
                data = obj["data"] as List<object>;
            }

            if (data == null)
            {
                Debug.Log("Map does not contain map data");
                return false;
            }
            if (data.Count != width * height)
            {
                Debug.Log("Map data size mismatch");
                return false;
            }
            SetSize(width, height);


            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Dictionary<string, object> tileObj = data[i++] as Dictionary<string, object>;
                    if (tileObj == null || !tileObj.ContainsKey("name"))
                    {
                        Debug.Log("Error deserialising map tile instance");
                        continue;
                    }

                    Tiles[x, y].BaseTile = OwnerWorld.Tiles.FindTile(tileObj["name"].ToString(), false);
                    Tiles[x, y].Deserialise(tileObj);
                }
            }

            return true;
        }
    }
}

