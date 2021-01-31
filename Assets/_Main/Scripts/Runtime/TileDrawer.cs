using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class TileDrawer : MonoBehaviour
    {
        [SerializeField] Tile[] numTiles;
        [SerializeField] Tile[] fixNumTiles;
        [SerializeField] Tile[] plusTiles;
        [SerializeField] Tile[] minusTiles;
        [SerializeField] Tile[] mulTiles;
        [SerializeField] Tile[] divTiles;
        [SerializeField] Tile equalTile;
        [SerializeField] Tile emptyTile;
        [SerializeField] Tile wallTile;
        [SerializeField] Tilemap tileMap;

        Camera m_Camera;
        LevelData m_LevelData;
        Vector2Int m_Offset;
        Vector2Int? m_SelectedBlock;

        void Awake()
        {
            Resolver.Register(this);
            m_Camera = Camera.main;
            m_Offset = new Vector2Int();
        }

        void OnDestroy()
        {
            Resolver.Unregister<TileDrawer>();
        }

        void Update()
        {
            if (m_SelectedBlock.HasValue)
            {
                var pos = m_SelectedBlock.Value;

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m_LevelData.TryMoveUp(pos.x, pos.y);
                    UpdateTile(m_LevelData);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    m_LevelData.TryMoveRight(pos.x, pos.y);
                    UpdateTile(m_LevelData);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    m_LevelData.TryMoveDown(pos.x, pos.y);
                    UpdateTile(m_LevelData);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    m_LevelData.TryMoveLeft(pos.x, pos.y);
                    UpdateTile(m_LevelData);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                var pos = tileMap.WorldToCell(m_Camera.ScreenToWorldPoint(Input.mousePosition));
                if (tileMap.HasTile(pos))
                {
                    var x = pos.x - m_Offset.x;
                    var y = pos.y - m_Offset.y;
                    if (!m_LevelData.GetAt(x, y).IsFixed)
                    {
                        m_SelectedBlock = new Vector2Int(x, y);
                        return;
                    }
                }
                m_SelectedBlock = null;
            }
        }

        public void UpdateTile(LevelData levelData)
        {
            m_LevelData = levelData;
            m_Offset.x = (9 - m_LevelData.Width) / 2;
            m_Offset.y = (6 - m_LevelData.Height) / 2;

            for (int y = 0; y < m_LevelData.Height; y++)
            {
                for (int x = 0; x < m_LevelData.Width; x++)
                {
                    var block = m_LevelData.GetAt(x, y);
                    var k = x + m_Offset.x;
                    var i = y + m_Offset.y;

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
                    else if (block == Block.Minus)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), minusTiles[0]);
                    }
                    else if (block == Block.FixedMinus)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), minusTiles[1]);
                    }
                    else if (block == Block.Mul)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), mulTiles[0]);
                    }
                    else if (block == Block.FixedMul)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), mulTiles[1]);
                    }
                    else if (block == Block.Div)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), divTiles[0]);
                    }
                    else if (block == Block.FixedDiv)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), divTiles[1]);
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
