/********************************
 * Author: Flynn Duniho
 * Date: 12/18/2020
 * Purpose: Similar to Rule Tiles, but since I didn't know about that, I made this instead
********************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetter : MonoBehaviour
{
    public Tile[] Flat;
    public Tile[] OuterCorner;
    public Tile[] InnerCorner;
    public Tile Middle;


    private Tilemap tiles;
    // Start is called before the first frame update
    void Start()
    {
        tiles = GetComponent<Tilemap>();

        foreach (Tile t in Flat)
        {
            t.transform.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }

        foreach (Tile t in OuterCorner)
        {
            t.transform.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }

        foreach (Tile t in InnerCorner)
        {
            t.transform.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }

        Middle.transform.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);


        var size = tiles.cellBounds;

        for (int x = size.xMin; x <= size.xMax; x++)
        {
            for (int y = size.yMin; y <= size.yMax; y++)
            {
                var t = tiles.GetTile(new Vector3Int(x, y, 0));
                if (t != null && t.name == "Spike")
                {
                    tiles.SetTile(new Vector3Int(x, y, 0), null);
                    tiles.FloodFill(new Vector3Int(x, y, 0), Middle);
                }
            }
        }

        for (int x = size.xMin; x <= size.xMax; x++)
        {
            for (int y = size.yMin; y <= size.yMax; y++)
            {
                var t = tiles.GetTile(new Vector3Int(x, y, 0));
                int surround = 0;
                if (t != null)
                {
                    surround += tiles.GetTile(new Vector3Int(x - 1, y + 0, 0)) != null ? (1 << 7) : 0;
                    surround += tiles.GetTile(new Vector3Int(x + 1, y + 0, 0)) != null ? (1 << 6) : 0;
                    surround += tiles.GetTile(new Vector3Int(x + 0, y - 1, 0)) != null ? (1 << 5) : 0;
                    surround += tiles.GetTile(new Vector3Int(x + 0, y + 1, 0)) != null ? (1 << 4) : 0;
                    surround += tiles.GetTile(new Vector3Int(x - 1, y - 1, 0)) != null ? (1 << 3) : 0;
                    surround += tiles.GetTile(new Vector3Int(x + 1, y + 1, 0)) != null ? (1 << 2) : 0;
                    surround += tiles.GetTile(new Vector3Int(x + 1, y - 1, 0)) != null ? (1 << 1) : 0;
                    surround += tiles.GetTile(new Vector3Int(x - 1, y + 1, 0)) != null ? (1 << 0) : 0;

                    switch (surround)
                    {
                        case 0b11101110:
                        case 0b11101011:
                        case 0b11101111:
                        case 0b11101010:
                            // TopFlat
                            //Flat.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 0), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), Flat[0]);
                            break;
                        case 0b11011101:
                        case 0b11011111:
                        case 0b11010111:
                        case 0b11010101:
                            // BottomFlat
                            //Flat.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 180), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), Flat[2]);
                            break;
                        case 0b10111011:
                        case 0b10111111:
                        case 0b10111101:
                        case 0b10111001:
                            // RightFlat
                            //Flat.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 90f), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), Flat[1]);
                            break;
                        case 0b01111110:
                        case 0b01111111:
                        case 0b01110111:
                        case 0b01110110:
                            // LeftFlat
                            //Flat.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, -90f), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), Flat[3]);
                            break;
                        case 0b10101010:
                        case 0b10101011:
                        case 0b10101001:
                        case 0b10101000:
                            // UR out
                            //OuterCorner.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), OuterCorner[1]);
                            break;
                        case 0b10011001:
                        case 0b10011101:
                        case 0b10010101:
                        case 0b10010001:
                            // DR out
                            //OuterCorner.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 180), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), OuterCorner[2]);
                            break;
                        case 0b01101010:
                        case 0b01101110:
                        case 0b01100110:
                        case 0b01100010:
                            // UL out
                            //OuterCorner.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 0), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), OuterCorner[0]);
                            break;
                        case 0b01010101:
                        case 0b01010111:
                        case 0b01010110:
                        case 0b01010100:
                            // DL out
                            //OuterCorner.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, -90), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), OuterCorner[3]);
                            break;
                        case 0b11111011:
                            // UR in
                            //InnerCorner.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), InnerCorner[1]);
                            break;
                        case 0b11111101:
                            // DR in
                            //InnerCorner.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 180), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), InnerCorner[2]);
                            break;
                        case 0b11111110:
                            // UL in
                            //InnerCorner.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 0), Vector3.one);
                            tiles.SetTile(new Vector3Int(x, y, 0), InnerCorner[0]);
                            break;
                        case 0b11110111:
                            // DL in
                            //InnerCorner.transform.SetTRS(Vector3.zero, Quaternion.Euler(0, 0, 0), new Vector3(1, -1, 1));
                            tiles.SetTile(new Vector3Int(x, y, 0), InnerCorner[3]);
                            break;
                        default:
                            // Middle
                            tiles.SetTile(new Vector3Int(x, y, 0), Middle);
                            break;
                    }

                }
            }
        }
    }
}
