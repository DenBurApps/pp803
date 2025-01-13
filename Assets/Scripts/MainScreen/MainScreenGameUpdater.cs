using System;
using System.Collections.Generic;
using System.Linq;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreenGameUpdater : MonoBehaviour
{
    [SerializeField] private SimpleScrollSnap _scrollSnap;
    [SerializeField] private StartMenuScreen _startMenuScreen;
    [SerializeField] private List<GameObject> _panels;
    [SerializeField] private Button _footballGameButton;
    [SerializeField] private Button _footballInfoButton;
    [SerializeField] private Button _tennisGameButton;
    [SerializeField] private Button _tennisInfoButton;
    [SerializeField] private Button _basketballGameButton;
    [SerializeField] private Button _basketballInfoButton;
    [SerializeField] private Button _hockeyGameButton;
    [SerializeField] private Button _hockeyInfoButton;
    [SerializeField] private Button _poleGameButton;
    [SerializeField] private Button _poleInfoButton;
    //[SerializeField] private Button _settingsButton;
    //[SerializeField] private SettingsScreen _settingsScreen;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action SettingsClicked;
    
    private void Awake()
    {
        _scrollSnap.OnPanelCentered.AddListener(UpdateInfo);
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        UpdateInfo(0, 0);
    }

    private void OnEnable()
    {
        _tennisGameButton.onClick.AddListener(RunTennis);
        _footballGameButton.onClick.AddListener(RunGolf);
        _basketballGameButton.onClick.AddListener(RunStreetball);
       //_settingsButton.onClick.AddListener(OnSettingsClicked);
        _startMenuScreen.GameSelectionOpened += _screenVisabilityHandler.EnableScreen;

        //  _settingsScreen.BackButtonClicked += _screenVisabilityHandler.EnableScreen;
    }

    private void OnDisable()
    {
        _tennisGameButton.onClick.RemoveListener(RunTennis);
        _footballGameButton.onClick.RemoveListener(RunGolf);
        _basketballGameButton.onClick.RemoveListener(RunStreetball);
       // _settingsButton.onClick.RemoveListener(OnSettingsClicked);
        _startMenuScreen.GameSelectionOpened -= _screenVisabilityHandler.EnableScreen;
        
      //  _settingsScreen.BackButtonClicked -= _screenVisabilityHandler.EnableScreen;
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void UpdateInfo(int start, int end)
    {
        foreach (var item in _panels)
        {
            item.SetActive(false);
        }

        _panels[start].SetActive(true);
    }

    private void RunTennis() => SceneManager.LoadScene("TennisScene");
    private void RunGolf() => SceneManager.LoadScene("GolfScene");
    private void RunStreetball() => SceneManager.LoadScene("StreetballScene");

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
        _screenVisabilityHandler.SetTransperent();
    }
}

public static class GameState
{
    public static bool ShowTutorial { get; set; } = false;
}