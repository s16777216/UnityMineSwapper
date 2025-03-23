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
        // �p�G�ƹ����ʨ�@�ӷs������
        if (clickable != currentHover)
        {
            // �p�G���e���a������AĲ�o���}�ƥ�
            if (Util.IsValid(currentHover))
                currentHover.OnMouseExit();

            if (Util.IsValid(clickable))
            {
                // ��s��e�a�������Ĳ�o�i�J�ƥ�
                currentHover = clickable;
                currentHover.OnMouseEnter();
            }
            else
            {
                currentHover = null;
            }
        }
        // �p�G�ƹ����d�b�P�@�Ӫ���W�A���򳣤���
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
