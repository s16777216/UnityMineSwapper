
using Unity.VisualScripting;

public class GameSceneParameter
{
    private static int width;
    private static int height;
    private static float scale;
    private static int mineCount;

    public static void SetParameter(int width, int height, float scale, int mineCount)
    {
        GameSceneParameter.width = width;
        GameSceneParameter.height = height;
        GameSceneParameter.scale = scale;
        GameSceneParameter.mineCount = mineCount;
    }

    public static (int, int, float, int) GetParameter()
    {
        return (width, height, scale, mineCount);
    }
}