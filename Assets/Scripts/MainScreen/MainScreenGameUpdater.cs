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
    private const string FootballSceneName = "FootballScene";
    private const string TennisSceneName = "TennisScene";
    private const string HockeySceneName = "HockeyScene";
    private const string BasketballSceneName = "BasketballScene";
    private const string PoleSceneName = "PoleScene";
    
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

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    private void Awake()
    {
        _scrollSnap.OnPanelCentered.AddListener(UpdateInfo);
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        UpdateInfo(0, 0);
    }

    private void OnEnable()
    {
        _tennisGameButton.onClick.AddListener(RunTennis);
        _footballGameButton.onClick.AddListener(RunFootball);
        _basketballGameButton.onClick.AddListener(RunBasketball);
        _hockeyGameButton.onClick.AddListener(RunHockey);
        _poleGameButton.onClick.AddListener(RunPole);
        
        _tennisInfoButton.onClick.AddListener(() => RunWithTutorial(TennisSceneName));
        _footballInfoButton.onClick.AddListener((() => RunWithTutorial(FootballSceneName)));
        _basketballInfoButton.onClick.AddListener((() => RunWithTutorial(BasketballSceneName)));
        _hockeyInfoButton.onClick.AddListener((() => RunWithTutorial(HockeySceneName)));
        _poleInfoButton.onClick.AddListener((() => RunWithTutorial(PoleSceneName)));
       
        _startMenuScreen.GameSelectionOpened += _screenVisabilityHandler.EnableScreen;
    }

    private void OnDisable()
    {
        _tennisGameButton.onClick.RemoveListener(RunTennis);
        _footballGameButton.onClick.RemoveListener(RunFootball);
        _basketballGameButton.onClick.RemoveListener(RunBasketball);
        _hockeyGameButton.onClick.RemoveListener(RunHockey);
        _poleGameButton.onClick.RemoveListener(RunPole);
        
        _tennisInfoButton.onClick.RemoveListener(() => RunWithTutorial(TennisSceneName));
        _footballInfoButton.onClick.RemoveListener((() => RunWithTutorial(FootballSceneName)));
        _basketballInfoButton.onClick.RemoveListener((() => RunWithTutorial(BasketballSceneName)));
        _hockeyInfoButton.onClick.RemoveListener((() => RunWithTutorial(HockeySceneName)));
        _poleInfoButton.onClick.RemoveListener((() => RunWithTutorial(PoleSceneName)));
       
        _startMenuScreen.GameSelectionOpened -= _screenVisabilityHandler.EnableScreen;
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

    private void RunTennis() => SceneManager.LoadScene(TennisSceneName);
    private void RunFootball() => SceneManager.LoadScene(FootballSceneName);
    private void RunBasketball() => SceneManager.LoadScene(BasketballSceneName);
    private void RunHockey() => SceneManager.LoadScene(HockeySceneName);
    private void RunPole() => SceneManager.LoadScene(PoleSceneName);

    private void RunWithTutorial(string sceneName)
    {
        GameState.ShowTutorial = true;
        SceneManager.LoadScene(sceneName);
    }
}

public static class GameState
{
    public static bool ShowTutorial { get; set; } = false;
}