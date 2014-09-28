using System;
using System.Collections.Generic;

namespace Maze.Game 
{
    public class TileInstance
    {
        public Tile BaseTile { get; set; }
        public float Height { get; set; }

        public TileInstance()
        {
            BaseTile = Tile.NoTile;
            Height = 0.0f;
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

            if (obj.ContainsKey("height"))
            {
                Height = (float)Convert.ToDouble(obj["height"]);
            }
        }
    }
}
