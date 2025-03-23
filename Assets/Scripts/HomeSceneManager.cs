using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainButtons;
    [SerializeField]
    private GameObject difficultButtons;

    public void OnStartButtonClick()
    {
        mainButtons.SetActive(false);
        difficultButtons.SetActive(true);
    }

    public void OnBackClick()
    {
        mainButtons.SetActive(true);
        difficultButtons.SetActive(false);
    }

    public void StartGame(int width, int height, float scale,int mineCount)
    {
        GameSceneParameter.SetParameter(width, height, scale, mineCount);
        SceneManager.LoadScene("GameScene");
    }

    public void OnEasyButtonClick()
    {
        StartGame(5, 5, 1.5f, 5);
    }

    public void OnNormalButtonClick()
    {
        StartGame(7, 7, 1f, 10);
    }

    public void OnHardButtonClick()
    {
        StartGame(10, 10, 0.7f, 25);
    }

    public void OnExitGameClick()
    {
        Application.Quit();
    }
}
