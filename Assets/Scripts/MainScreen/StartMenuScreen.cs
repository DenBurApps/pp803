using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class StartMenuScreen : MonoBehaviour
{
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action GameSelectionOpened;
    public event Action SettingsOpened;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void OpenGameSelection()
    {
        GameSelectionOpened?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    public void OpenSettings()
    {
        SettingsOpened?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
}
