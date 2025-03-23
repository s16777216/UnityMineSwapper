using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap; // �즲�A�� Tilemap �����o��
    [SerializeField] private TileBase[] tiles; // �H����ܪ� Tile�A�Ҧp�a�O�B�����
    [SerializeField] private int width = 10; // �a�ϼe��
    [SerializeField] private int height = 10; // �a�ϰ���

    void Start()
    {
        GenerateRandomMap();
    }

    void GenerateRandomMap()
    {
        // �M�� Tilemap
        tilemap.ClearAllTiles();

        // �M���a�Ϫ��C�Ӯ�l
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // �H����ܤ@�� Tile
                TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                // �b���w��m�]�m Tile
                tilemap.SetTile(new Vector3Int(x - (width / 2), y - (height / 2), 0), randomTile);
            }
        }
    }
}