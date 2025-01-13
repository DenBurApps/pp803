using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class NewRecordScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _record;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action RestartGame;
    public event Action ExitGame;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    public void EnableScreen(int record)
    {
        _screenVisabilityHandler.EnableScreen();
        _record.text = record.ToString();
    }
    
    public void DisableScreen()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void OnRestartGame()
    {
        RestartGame?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    public void OnExitGame()
    {
        ExitGame?.Invoke();
    }
}
