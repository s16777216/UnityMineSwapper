using UnityEngine;

public class Mine : Block
{
    [SerializeField]
    private Animator shadowAnimator;
    private void Start()
    {
        shadow.TryGetComponent<Animator>(out shadowAnimator);
    }
    public override void Open()
    {
        if (IsOpened) return;
        IsOpened = true;

        flag.gameObject.SetActive(false);
        shadow.gameObject.SetActive(true);
        shadowAnimator.SetTrigger("Exploded");

        Invoke(nameof(DisableShadow), 1f);
    }
    private void DisableShadow()
    {
        shadow.gameObject.SetActive(false);
    }
    public override void OnSurroundBlockOpen(){
        Debug.Log("Surround Block Clicked, Mine nothing happened!");
    }
}
