using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class TileDrawer : MonoBehaviour
    {
        [SerializeField] Tile[] numTiles;
        [SerializeField] Tile[] fixNumTiles;
        [SerializeField] Tile[] plusTiles;
        [SerializeField] Tile equalTile;
        [SerializeField] Tile emptyTile;
        [SerializeField] Tile wallTile;
        [SerializeField] Tilemap tileMap;

        void Awake()
        {
            Resolver.Register(this);
        }

        private void OnDestroy()
        {
            Resolver.Unregister<TileDrawer>();
        }

        public void UpdateTile(LevelData levelData)
        {
            var offsetX = (9 - levelData.Width) / 2;
            var offsetY = (6 - levelData.Height) / 2;

            for (int y = 0; y < levelData.Height; y++)
            {
                for (int x = 0; x < levelData.Width; x++)
                {
                    var block = levelData.GetAt(x, y);
                    var k = x + offsetX;
                    var i = y + offsetY;

                    if (block.IsDigit)
                    {
                        var number = block.Value - '0';

                        if (block.IsFixed)
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), fixNumTiles[number]);
                        }
                        else
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), numTiles[number]);
                        }
                    }
                    else if (block == Block.Plus)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), plusTiles[0]);
                    }
                    else if (block == Block.FixedPlus)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), plusTiles[1]);
                    }
                    else if (block == Block.FixedEqual)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), equalTile);
                    }
                    else if (block == Block.Wall)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), wallTile);
                    }
                    else
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), emptyTile);
                    }
                }
            }
        }
    }
}
