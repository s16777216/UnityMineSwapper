using UnityEngine;

public class NumberRender : MonoBehaviour
{
    [SerializeField]
    private Sprite[] numberSprites = new Sprite[10];
    [SerializeField]
    private int defaultNumber = 0;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNumber(defaultNumber);
    }

    public void SetNumber(int number)
    {
        spriteRenderer.sprite = numberSprites[number];
    }
}
