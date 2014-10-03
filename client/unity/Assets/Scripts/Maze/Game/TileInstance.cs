using UnityEngine;
using System;
using System.Collections.Generic;

namespace Maze.Game 
{
    public class TileInstance
    {
        public Tile BaseTile { get; set; }
        public float[] Heights { get; set; }

        public float Height
        {
            set
            {
                Heights[0] = value;
                Heights[1] = value;
                Heights[2] = value;
                Heights[3] = value;
            }
        }

        public TileInstance()
        {
            BaseTile = Tile.NoTile;
            Heights = new float[4]{ 0.0f, 0.0f, 0.0f, 0.0f };
        }
        public TileInstance(Tile tile, float height)
        {
            BaseTile = tile;
            Height = height;
        }

        public void Deserialise(Dictionary<string, object> obj)
        {
            if (obj == null)
            {
                return;
            }

            if (obj.ContainsKey("heights"))
            {
                List<object> heights = obj["heights"] as List<object>;
                if (heights == null)
                {
                    Debug.Log("Error deserialising tile instance, invalid height data");
                }
                else
                {
                    for (int i = 0; i < Mathf.Min(4, heights.Count); i++)
                    {
                        float value = (float)Convert.ToDouble(heights[i]);
                        Heights[i] = value;
                    }
                }
            }
        }
    }
}
