using System;
using System.Collections;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FootballGame
{
    public class GameController : MonoBehaviour
    {
        private const float InitTimerValue = 30;
        
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _recordText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TutorialWindow _howToPlayPlane;
        [SerializeField] private Ball _ball;
        [SerializeField] private Field _field;
        [SerializeField] private EndGameScreen _endGameScreen;
        [SerializeField] private NewRecordScreen _newRecordScreen;
        [SerializeField] private GameType _gameType;
        
        private int _currentScore;
        private float _timerValue;

        private IEnumerator _timerCoroutine;

        private void OnEnable()
        {
            _field.Hit += IncreaseScore;
            _endGameScreen.RestartGame += StartNewGame;
            _endGameScreen.ExitGame += CloseGame;
            _newRecordScreen.RestartGame += StartNewGame;
            _newRecordScreen.ExitGame += CloseGame;
            _howToPlayPlane.PlayClicked += StartNewGame;
            _exitButton.onClick.AddListener(CloseGame);
        }

        private void OnDisable()
        {
            _field.Hit -= IncreaseScore;
            _endGameScreen.RestartGame -= StartNewGame;
            _endGameScreen.ExitGame -= CloseGame;
            _newRecordScreen.RestartGame -= StartNewGame;
            _newRecordScreen.ExitGame -= CloseGame;
            _howToPlayPlane.PlayClicked -= StartNewGame;
            _exitButton.onClick.RemoveListener(CloseGame);
        }

        private void Start()
        {
            _endGameScreen.DisableScreen();
            _newRecordScreen.DisableScreen();
            
            if (GameState.ShowTutorial)
            {
                _howToPlayPlane.gameObject.SetActive(true);
                _ball.gameObject.SetActive(false);
                _field.gameObject.SetActive(false);
                return;
            }
            
            _howToPlayPlane.gameObject.SetActive(false);
            _ball.gameObject.SetActive(true);
            _field.gameObject.SetActive(true);
            StartNewGame();
        }

        private void StartNewGame()
        {
            _ball.gameObject.SetActive(true);
            _field.gameObject.SetActive(true);
            ResetValues();
            UpdateScoreUI();
            UpdateTimerUI();
            StartTimer();
            _ball.EnableMovement();
            _field.EnableMovement();
            _endGameScreen.DisableScreen();
            _newRecordScreen.DisableScreen();
            UpdateRecordText();
        }

        private void StartTimer()
        {
            EndTimer();
            _timerCoroutine = TimerCountdown();
            StartCoroutine(_timerCoroutine);
        }

        private void EndTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        } 
            
        private IEnumerator TimerCountdown()
        {
            while (_timerValue > 0)
            {
                _timerValue -= Time.deltaTime;
                UpdateTimerUI();
                yield return null;
            }
            StopGame();
            EndGame();
        }
        
        private void UpdateTimerUI()
        {
            _timerText.text = Mathf.CeilToInt(_timerValue).ToString();
        }

        private void UpdateScoreUI()
        {
            _scoreText.text = _currentScore.ToString();
        }

        private void UpdateRecordText()
        {
            _recordText.text = RecordHolder.GetRecordByType(_gameType).ToString();
        }
        
        private void ResetValues()
        {
            _currentScore = 0;
            _timerValue = InitTimerValue;
            _ball.SetDefault();
            _field.SetDefault();
        }

        private void StopGame()
        {
            _ball.DisableMovement();
            _ball.SetDefault();
            _field.DisableMovement();
            _field.SetDefault();
            EndTimer();
        }

        private void EndGame()
        {
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
            _ball.SetDefault();
        }

        private void CloseGame()
        {
            GameState.ShowTutorial = false;
            SceneManager.LoadScene("MainScene");
        }
    }
}
