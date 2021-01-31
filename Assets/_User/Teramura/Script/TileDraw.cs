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
        LevelData levelData = new LevelData(10,10);
        Tilemap tileMap;

        // Start is called before the first frame update
        void Start()
        {
            tileMap = GetComponent<Tilemap>();
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < levelData.Height; i++)
            {
                for (int k = 0; k < levelData.Width; k++)
                {
                    block = levelData.GetAt(i, k);
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
                            tileMap.SetTile(new Vector3Int(k, i, 0), emptyTile);
                        }
                        else if(block.Value == '=')
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), equalTile);
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
