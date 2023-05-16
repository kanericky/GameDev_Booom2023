using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    [Header("Reference")] public CanvasGroup menuCanvasGroup;

    private void Start()
    {
        menuCanvasGroup.alpha = 0;
    }

    public void MenuUIEnterReady()
    {
        menuCanvasGroup.DOFade(1, .2f);
    }

    public void QuitGame()
    {
        Debug.Log("Player quit game");
        Application.Quit();
    }
}
