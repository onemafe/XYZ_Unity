﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindow : AnimatedWindow
{
    private Action _closeAction;


    public void OnShowSettings()
    {
        WindowUtils.CreateWindow("UI/SettingsWindow");
    }

    public void OnStartGame()
    {
        _closeAction = () =>
        { SceneManager.LoadScene("Scene_1"); };
        Close();

    }

    public void OnLanguages()
    {
        WindowUtils.CreateWindow("UI/LocalizationWindow");
    }

    public void OnExit()
    {
        _closeAction = () =>
        {
            Application.Quit();


#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        };
        Close();
    }



    public override void OnCloseAnimationComplete()
    {
        _closeAction?.Invoke();
        base.OnCloseAnimationComplete();
    }
}
