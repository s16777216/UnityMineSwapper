using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Block : MonoBehaviour, IClickable
{
    public int index;
    private Vector2 position = new(0, 0);
    public int X => (int)position.x;
    public int Y => (int)position.y;
    public int SurroundMineCount { get; private set; } = 0;

    public TileBase tile;
    [SerializeField]
    private List<Block> surroundBlockList = new();
    [SerializeField]
    protected NumberRender numberRender;
    [SerializeField]
    protected SpriteRenderer shadow;
    [SerializeField]
    protected SpriteRenderer flag;
    [SerializeField]
    protected SpriteRenderer cursor;

    public bool IsOpened { get; protected set; } = false;

    public delegate void OnClicked(Block block);
    private OnClicked onClicked;

    private void Awake()
    {
        TryGetChildComponentByName<NumberRender>("SurroundedMineCount", out numberRender);
        TryGetChildComponentByName<SpriteRenderer>("Shadow", out shadow);
        TryGetChildComponentByName<SpriteRenderer>("Flag", out flag);
        TryGetChildComponentByName<SpriteRenderer>("Cursor", out cursor);
    }

    public void SetPosition(Vector2 position)
    {
        this.position = position;
        this.transform.localPosition = position;
    }

    public void AddSurroundBlock(Block block)
    {
        surroundBlockList.Add(block);

        if (block is Mine) SurroundMineCount++;
    }

    // 1. �����Q�I����
    // 2. click count + 1
    // 3. �p�G����w�g�Q���}�A�h���������
    // 4. �p�G�P��S���a�p�A�hĲ�o�|�誺����� OnSurroundBlockOpen (3 -> 5)
    // 5. �p�G�P�򦳦a�p�A�h��ܩP��a�p���ƶq
    // 6. �p�G����Q�k���I���A�h��ܺX�l

    public void OnClick()
    {
        Open();
        onClicked?.Invoke(this);
    }

    public void OnRightClick()
    {
        if (IsOpened) return;
        flag.gameObject.SetActive(!flag.gameObject.activeSelf);
    }

    public void OnMouseEnter()
    {
        cursor.gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        cursor.gameObject.SetActive(false);
    }

    public void SetOnClick(OnClicked onClick)
    {
        onClicked = onClick;
    }

    public virtual void Open()
    {
        if (IsOpened) return;
        IsOpened = true;

        flag.gameObject.SetActive(false);
        shadow.gameObject.SetActive(true);
        numberRender.gameObject.SetActive(true);
        numberRender.SetNumber(SurroundMineCount);

        if (SurroundMineCount != 0) return;
        surroundBlockList
            .FindAll(block => Vector2.Distance(this.position, block.position) == 1)
            .ForEach(block => block.OnSurroundBlockOpen());
    }

    public virtual void OnSurroundBlockOpen()
    {
        Open();
    }

    private void TryGetChildComponentByName<T>(string name, out T component)
    {
        var childTransform = this.transform.Find(name);
        if (!childTransform) component = default;
        else
        {
            childTransform.TryGetComponent<T>(out component);
        }
    }
}
