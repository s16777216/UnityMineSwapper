using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private IClickable currentHover;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState is GameManager.GameState.GameOver) return;

        // Check if the mouse is hovering on the clickable object
        if (!Util.TryGetComponentOnPointer<IClickable>(out var clickable))
        {
            if (Util.IsValid(currentHover))
            {
                currentHover.OnMouseExit();
                currentHover = null;
            }
            return;
        };
        HandleMouseEnter(clickable);

        // Check if mouse button is clicked
        var keyCode = OnMouseClick();
        if (keyCode == KeyCode.None) return;

        
        switch (keyCode)
        {
            case KeyCode.Mouse0:
                clickable.OnClick();
                break;
            case KeyCode.Mouse1:
                clickable.OnRightClick();
                break;
        }
    }

    private void HandleMouseEnter(IClickable clickable)
    {
        // 如果滑鼠移動到一個新的物件
        if (clickable != currentHover)
        {
            // 如果之前有懸停物件，觸發離開事件
            if (Util.IsValid(currentHover))
                currentHover.OnMouseExit();

            if (Util.IsValid(clickable))
            {
                // 更新當前懸停物件並觸發進入事件
                currentHover = clickable;
                currentHover.OnMouseEnter();
            }
            else
            {
                currentHover = null;
            }
        }
        // 如果滑鼠停留在同一個物件上，什麼都不做
    }

    KeyCode OnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return KeyCode.Mouse0;
        }
        if (Input.GetMouseButtonDown(1))
        {
            return KeyCode.Mouse1;
        }
        return KeyCode.None;
    }
}
