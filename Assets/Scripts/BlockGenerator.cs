using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private int width, height;
    [SerializeField]
    private float scale;
    private int size => width * height;
    [SerializeField]
    private int mineCount;

    [SerializeField]
    private Block emptyBlockPrefab;
    [SerializeField]
    private Block blockPrefab;
    [SerializeField]
    private Block minePrefab;
    [SerializeField]
    private Tilemap blockTilemap;
    [SerializeField]
    private Tilemap waterTilemap;
    [SerializeField]
    private TileBase waterTileBase;

    private void Awake()
    {
        var (width, height, scale, mineCount) = GameSceneParameter.GetParameter();
        if (width == 0 || height == 0 || scale == 0 || mineCount == 0)
        {
            return;
        }

        this.width = width;
        this.height = height;
        this.scale = scale;
        this.mineCount = mineCount;
    }

    public List<Block> GenerateEmptyBlocks(Action<Block> onBlockClick, Action<Block> onRightClick)
    {
        var tempBlocks = new List<Block>(size);
        for (int i = 0; i < size; i++)
        {
            var x = i / width;
            var y = i % width;
            var block = Instantiate(emptyBlockPrefab, Vector2.zero, Quaternion.identity, blockTilemap.transform);
            block.index = i;
            block.SetPosition(new Vector2(x, y));
            block.SetOnClick(onBlockClick);
            block.SetOnRightClick(onRightClick);
            blockTilemap.SetTile(new Vector3Int(x, y, 0), block.tile);

            tempBlocks.Add(block);
        }
        return tempBlocks;
    }

    public List<Block> GenerateBlocks(
        (int, int) clickPosition, 
        Action<Block> onBlockClick,
        Action<Block> onMineClick,
        Action<Block> onRightClick
        )
    {
        var (firstX, firstY) = clickPosition;

        // 1. 計算出第一次點擊及周圍的位置
        var notMinePosition = new List<int>(9);
        var firstClickIndex = firstX * width + firstY;
        // a - width - 1, a - width, a - width + 1
        // a-1 , a, a + 1
        // a + width - 1, a + width, a + width + 1
        notMinePosition.Add(firstClickIndex);
        notMinePosition.Add(firstClickIndex - width - 1);
        notMinePosition.Add(firstClickIndex - width);
        notMinePosition.Add(firstClickIndex - width + 1);
        notMinePosition.Add(firstClickIndex - 1);
        notMinePosition.Add(firstClickIndex + 1);
        notMinePosition.Add(firstClickIndex + width - 1);
        notMinePosition.Add(firstClickIndex + width);
        notMinePosition.Add(firstClickIndex + width + 1);

        notMinePosition = notMinePosition.FindAll(index => index >= 0 && index < size);

        // 2. 生成剩餘的地雷與方塊
        var remainingSize = size - notMinePosition.Count;
        var remainingBlocks = new List<Block>(remainingSize);
        for (int i = 0; i < mineCount; i++)
        {
            var mine = Instantiate(minePrefab, Vector2.zero, Quaternion.identity, blockTilemap.transform);
            mine.gameObject.SetActive(false);
            mine.SetOnClick(onMineClick);
            mine.SetOnRightClick(onRightClick);
            remainingBlocks.Add(mine);
        }
        for (int i = mineCount; i < remainingSize; i++)
        {
            var block = Instantiate(blockPrefab, Vector2.zero, Quaternion.identity, blockTilemap.transform);
            block.gameObject.SetActive(false);
            block.SetOnClick(onBlockClick);
            block.SetOnRightClick(onRightClick);
            remainingBlocks.Add(block);
        }

        // 3. 洗牌
        for (int i = 0; i < remainingBlocks.Count; i++)
        {
            var temp = remainingBlocks[i];
            var randomIndex = UnityEngine.Random.Range(0, size - 9);
            remainingBlocks[i] = remainingBlocks[randomIndex];
            remainingBlocks[randomIndex] = temp;
        }

        var remainingBlockStack = new Stack<Block>(remainingBlocks);

        // 4. 合併第一次點擊的方塊
        var blocks = new List<Block>(size);
        for (int i = 0; i < size; i++)
        {
            if (notMinePosition.Contains(i))
            {
                var block = Instantiate(blockPrefab, Vector2.zero, Quaternion.identity, blockTilemap.transform);
                block.gameObject.SetActive(false);
                block.SetOnClick(onBlockClick);
                block.SetOnRightClick(onRightClick);
                blocks.Add(block);
            }
            else
            {
                blocks.Add(remainingBlockStack.Pop());
            }
        }

        // 5. 設定方塊的位置
        for (int i = 0; i < size; i++)
        {
            var x = i / width;
            var y = i % width;
            new Vector2Int(x, y);
            var position = new Vector2(x, y);
            var block = blocks[i];
            block.index = i;
            block.SetPosition(position);
            blockTilemap.SetTile(new Vector3Int(x, y, 0), block.tile);
        }

        // 6. 設定方塊的周圍方塊
        blocks = SetSurroundedBlocks(blocks);

        // 7. 方塊顯現
        for (int i = 0; i < size; i++)
        {
            blocks[i].gameObject.SetActive(true);
        }

        // 8. 開啟第一次點擊的方塊
        //blocks[firstClickIndex].OnClick();

        return blocks;
    }

    private List<Block> SetSurroundedBlocks(List<Block> blocks)
    {
        foreach (var block in blocks) {
            var x = block.X;
            var y = block.Y;
            // 設定八個方位的方塊
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var surroundX = x + i;
                    var surroundY = y + j;
                    if (surroundX < 0 || surroundX >= width || surroundY < 0 || surroundY >= height)
                    {
                        continue;
                    }
                    var surroundBlock = blocks[surroundX * width + surroundY];
                    if (surroundBlock == block)
                    {
                        continue;
                    }
                    block.AddSurroundBlock(surroundBlock);
                }
            }
        }

        return blocks;
    }

    private void SetContainerPosition()
    {
        var position = (new Vector2(-width / 2f, -height / 2f) + new Vector2(1f / 2f, 1f / 2f)) * scale;
        blockTilemap.transform.parent.position = position;
        blockTilemap.transform.parent.localScale = new Vector3(scale, scale, scale);

        // generate water tile
        for (int i = 0; i < size; i++)
        {
            var x = i / width;
            var y = i % width;
            waterTilemap.SetTile(new Vector3Int(x, y, 0), waterTileBase);
        }
    }

    private void Start()
    {
        SetContainerPosition();
    }
}
