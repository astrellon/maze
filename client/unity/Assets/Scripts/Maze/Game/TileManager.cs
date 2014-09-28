using UnityEngine;
using System.Collections.Generic;

namespace Maze.Game 
{
    public class TileManager
    {
        public Dictionary<string, Tile> Tiles { get; protected set; }

        public TileManager()
        {
            Tiles = new Dictionary<string, Tile>();
        }

        public Tile FindTile(string name, bool returnNoTile)
        {
            if (Tiles.ContainsKey(name))
            {
                return Tiles[name];
            }
            if (returnNoTile)
            {
                return Tile.NoTile;
            }
            return null;
        }

        protected void DeserialiseTiles(Dictionary<string, object> obj)
        {
            if (obj == null)
            {
                return;
            }

            if (obj.ContainsKey("tiles"))
            {
                List<object> tiles = obj["tiles"] as List<object>;
                if (tiles == null)
                {
                    Debug.Log("Error deserialising tile manager tiles: " + obj["tiles"].GetType().ToString());
                    return;
                }

                foreach (object tileObj in tiles)
                {
                    Dictionary<string, object> tileHash = tileObj as Dictionary<string, object>;
                    if (tileHash == null)
                    {
                        Debug.Log("Error deserialising tile manager tile: " + tileObj.GetType().ToString());
                        continue;
                    }

                    Tile tile = new Tile();
                    if (tile.Deserialise(tileHash))
                    {
                        Debug.Log("- Adding tile: " + tile.Name);
                        Tiles[tile.Name] = tile;
                    }
                }
            }
            else
            {
                Debug.Log("- Tiles does not contain tile data");
            }
        }
        public void Deserialise(Dictionary<string, object> obj)
        {
            DeserialiseTiles(obj);
        }
    }
}
