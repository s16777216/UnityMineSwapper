using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        GameOver
    }

    [SerializeField]
    private BlockGenerator blockGenerator;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject youWinPanel;
    [SerializeField]
    private UIFormItem clickTimeFormItem;
    [SerializeField]
    private int clickTime = 0;
    [SerializeField]
    private List<Block> blockList;

    public GameState gameState { get; private set; } = GameState.Playing;


    private void Awake()
    {
        void onBlockClick(Block block)
        {
            Debug.Log($"Block Clicked: {block.X}, {block.Y}");
            AddClickTime();
            var isAllBlockOpened = blockList.FindAll(b => b is not Mine).TrueForAll(b => b.IsOpened);
            if (isAllBlockOpened) OnYouWin();
        }
        void onMineClick(Block block)
        {
            Debug.Log($"Mine Clicked: {block.X}, {block.Y}");
            gameState = GameState.GameOver;
            blockList.ForEach(b => b.Open());
            gameOverPanel.SetActive(true);
        }
        void onEmptyBlockClick(Block block)
        {
            var (x, y, index) = (block.X, block.Y, block.index);

            Debug.Log($"Empty Block Clicked: {x}, {y}");
            blockList.ForEach(b => Destroy(b.gameObject));
            this.blockList = blockGenerator.GenerateBlocks(
                clickPosition: (x, y),
                onBlockClick,
                onMineClick
            );

            this.blockList[index].OnClick();
        }

        blockList = blockGenerator.GenerateEmptyBlocks(onEmptyBlockClick);
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void GameRestart()
    {
        Debug.Log("Game Restart");

        // reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnGameOver()
    {
        gameState = GameState.GameOver;
        blockList.ForEach(b => b.Open());
        gameOverPanel.SetActive(true);
    }

    public void OnYouWin()
    {
        gameState = GameState.GameOver;
        blockList.ForEach(b => b.Open());
        youWinPanel.SetActive(true);
    }

    public void AddClickTime()
    {
        clickTime++;
        clickTimeFormItem.SetValue(clickTime.ToString());
    }
}
