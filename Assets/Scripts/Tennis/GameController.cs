using System;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tennis
{
    public class GameController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text _recordText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TutorialWindow _howToPlayPlane;
        [SerializeField] private EndGameScreen _endGameScreen;
        [SerializeField] private NewRecordScreen _newRecordScreen;

        [Header("Gameplay Elements")]
        [SerializeField] private TennisBall _ball;
        [SerializeField] private Opponent _opponent;
        [SerializeField] private Player _player;
        [SerializeField] private BallCatcher _opponentCatcher;
        [SerializeField] private BallCatcher _playerCatcher;

        [Header("Game Settings")]
        [SerializeField] private GameType _gameType;

        private int _currentScore;

        private void OnEnable()
        {
            _opponentCatcher.GotBall += IncreaseScore;
            _playerCatcher.GotBall += EndGame;
            _player.CatchedBall += IncreaseScore;
            _exitButton.onClick.AddListener(CloseGame);

            _endGameScreen.ExitGame += CloseGame;
            _endGameScreen.RestartGame += StartNewGame;
            _newRecordScreen.ExitGame += CloseGame;
            _newRecordScreen.RestartGame += StartNewGame;

            _howToPlayPlane.PlayClicked += StartNewGame;
        }

        private void OnDisable()
        {
            _opponentCatcher.GotBall -= IncreaseScore;
            _playerCatcher.GotBall -= EndGame;
            _player.CatchedBall -= IncreaseScore;

            _endGameScreen.ExitGame -= CloseGame;
            _endGameScreen.RestartGame -= StartNewGame;
            _newRecordScreen.ExitGame -= CloseGame;
            _newRecordScreen.RestartGame -= StartNewGame;
            
            _exitButton.onClick.RemoveListener(CloseGame);
            
            _howToPlayPlane.PlayClicked -= StartNewGame;
        }

        private void Start()
        {
            _endGameScreen.DisableScreen();
            _newRecordScreen.DisableScreen();

            if (GameState.ShowTutorial)
            {
                SetGameElementState(_howToPlayPlane.gameObject, true);
                SetGameElementState(_ball.gameObject, false);
                SetGameElementState(_opponent.gameObject, false);
                return;
            }

            SetGameElementState(_howToPlayPlane.gameObject, false);
            SetGameElementState(_ball.gameObject, true);
            SetGameElementState(_opponent.gameObject, true);
            StartNewGame();
        }

        private void StartNewGame()
        {
            SetGameElementState(_ball.gameObject, true);
            SetGameElementState(_opponent.gameObject, true);
            ResetValues();
            UpdateRecordText();
            UpdateScoreUI();
            _player.EnableDetection();
            _opponent.StartMovement();
            _endGameScreen.DisableScreen();
            _newRecordScreen.DisableScreen();
            _ball.LaunchBall();
        }

        private void ResetValues()
        {
            _currentScore = 0;
            _ball.Reset();
            _opponent.ReturnToDefaultPosition();
        }

        private void StopGame()
        {
            _ball.Reset();
            _player.DisableDetection();
            _opponent.StopMovement();
        }

        private void EndGame()
        {
            StopGame();

            if (_currentScore > RecordHolder.GetRecordByType(_gameType))
            {
                RecordHolder.AddNewRecord(_gameType, _currentScore);
                _newRecordScreen.EnableScreen(_currentScore);
                return;
            }
            
            _endGameScreen.EnableScreen(_currentScore, _gameType);
        }

        private void IncreaseScore()
        {
            _currentScore++;
            UpdateScoreUI();
        }

        private void CloseGame()
        {
            GameState.ShowTutorial = false;
            SceneManager.LoadScene("MainScene");
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
                _scoreText.text = _currentScore.ToString();
        }

        private void UpdateRecordText()
        {
            if (_recordText != null)
                _recordText.text = RecordHolder.GetRecordByType(_gameType).ToString();
        }

        private void SetGameElementState(GameObject obj, bool state)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }
}
