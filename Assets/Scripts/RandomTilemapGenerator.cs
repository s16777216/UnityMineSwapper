using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap; // 拖曳你的 Tilemap 物件到這裡
    [SerializeField] private TileBase[] tiles; // 隨機選擇的 Tile，例如地板、牆壁等
    [SerializeField] private int width = 10; // 地圖寬度
    [SerializeField] private int height = 10; // 地圖高度

    void Start()
    {
        GenerateRandomMap();
    }

    void GenerateRandomMap()
    {
        // 清空 Tilemap
        tilemap.ClearAllTiles();

        // 遍歷地圖的每個格子
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 隨機選擇一個 Tile
                TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                // 在指定位置設置 Tile
                tilemap.SetTile(new Vector3Int(x - (width / 2), y - (height / 2), 0), randomTile);
            }
        }
    }
}