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
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip OnClickClip;
    [SerializeField]
    private AudioClip OnRightClickClip;
    [SerializeField]
    private AudioClip OnExplodeClip;

    public GameState gameState { get; private set; } = GameState.Playing;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        void onRightClick(Block block)
        {
            Debug.Log($"Right Clicked: {block.X}, {block.Y}");
            audioSource.PlayOneShot(OnRightClickClip);
        }
        void onBlockClick(Block block)
        {
            Debug.Log($"Block Clicked: {block.X}, {block.Y}");
            audioSource.PlayOneShot(OnClickClip);
            AddClickTime();
            var isAllBlockOpened = blockList.FindAll(b => b is not Mine).TrueForAll(b => b.IsOpened);
            if (isAllBlockOpened) OnYouWin();
        }
        void onMineClick(Block block)
        {
            Debug.Log($"Mine Clicked: {block.X}, {block.Y}");
            audioSource.PlayOneShot(OnClickClip);
            audioSource.PlayOneShot(OnExplodeClip);
            gameState = GameState.GameOver;
            blockList.ForEach(b => b.Open());
            gameOverPanel.SetActive(true);
        }
        void onEmptyBlockClick(Block block)
        {
            var (x, y, index) = (block.X, block.Y, block.index);

            Debug.Log($"Empty Block Clicked: {x}, {y}");
            audioSource.PlayOneShot(OnClickClip);
            blockList.ForEach(b => Destroy(b.gameObject));
            this.blockList = blockGenerator.GenerateBlocks(
                clickPosition: (x, y),
                onBlockClick,
                onMineClick,
                onRightClick
            );

            this.blockList[index].OnClick();
        }

        blockList = blockGenerator.GenerateEmptyBlocks(onEmptyBlockClick, onRightClick);
    }

    public void BackToHome()
    {
        Debug.Log("Back to home.");
        audioSource.PlayOneShot(OnClickClip);
        SceneManager.LoadScene("HomeScene");
    }

    public void GameRestart()
    {
        Debug.Log("Game restart.");
        audioSource.PlayOneShot(OnClickClip);

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
