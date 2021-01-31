#if UNITY_EDITOR
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace User.Teramura
{
    public class TileDraw : MonoBehaviour
    {
        [SerializeField] Tile[] numTiles;
        [SerializeField] Tile[] fixNumTiles;
        [SerializeField] Tile[] plusTiles;
        [SerializeField] Tile equalTile;
        [SerializeField] Tile emptyTile;
        Block block;
        LevelData levelData;
        Tilemap tileMap;

        // Start is called before the first frame update
        void Start()
        {
            tileMap = GetComponent<Tilemap>();

            levelData = new LevelData(7, 3);
            levelData.SetAt(0, 0, Block.Num3);
            levelData.SetAt(1, 0, Block.Empty);
            levelData.SetAt(2, 0, Block.Num5);
            levelData.SetAt(3, 0, Block.Wall);
            levelData.SetAt(4, 0, Block.Wall);
            levelData.SetAt(5, 0, Block.Wall);
            levelData.SetAt(6, 0, Block.Wall);
            levelData.SetAt(0, 1, Block.Empty);
            levelData.SetAt(1, 1, Block.Empty);
            levelData.SetAt(2, 1, Block.Empty);
            levelData.SetAt(3, 1, Block.Fixed8);
            levelData.SetAt(4, 1, Block.FixedEqual);
            levelData.SetAt(5, 1, Block.Fixed4);
            levelData.SetAt(6, 1, Block.Fixed5);
            levelData.SetAt(0, 2, Block.Num4);
            levelData.SetAt(1, 2, Block.Plus);
            levelData.SetAt(2, 2, Block.Num7);
            levelData.SetAt(3, 2, Block.Wall);
            levelData.SetAt(4, 2, Block.Wall);
            levelData.SetAt(5, 2, Block.Wall);
            levelData.SetAt(6, 2, Block.Wall);
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < levelData.Height; i++)
            {
                for (int k = 0; k < levelData.Width; k++)
                {
                    block = levelData.GetAt(k, i);
                    if (Char.IsNumber(block.Value) == false)
                    {
                        if (block.Value == '+')
                        {
                            if (block.IsFixed == false)
                            {
                                tileMap.SetTile(new Vector3Int(k, i, 0), plusTiles[0]);
                            }
                            else
                            {
                                tileMap.SetTile(new Vector3Int(k, i, 0), plusTiles[1]);
                            }
                        }
                        else if(block.Value == '=')
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), equalTile);
                        }
                        else
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), emptyTile);
                        }
                    }
                    else if(Char.IsNumber(block.Value) == true)
                    {
                        int number = (int)Char.GetNumericValue(block.Value);
                        if(block.IsFixed == false)
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), numTiles[number]);
                        }
                        else
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), fixNumTiles[number]);
                        }
                    }
                }
            }
        }
    }
}
#endif //UNITY_EDITOR
