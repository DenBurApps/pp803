using System.Collections;
using System.Collections.Generic;
using FootballGame;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Basketball
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
        [SerializeField] private Pole _pole;
        [SerializeField] private BasketballNet _net;
        [SerializeField] private EndGameScreen _endGameScreen;
        [SerializeField] private NewRecordScreen _newRecordScreen;
        [SerializeField] private GameType _gameType;
        [SerializeField] private GameObject _scoreHolder;

        private int _timerIncreaseValue = 2;
        private int _currentScore;
        private float _timerValue;

        private IEnumerator _timerCoroutine;
        
        private void OnEnable()
        {
            _net.BallCatched += IncreaseScore;
            _endGameScreen.RestartGame += StartNewGame;
            _endGameScreen.ExitGame += CloseGame;
            _newRecordScreen.RestartGame += StartNewGame;
            _newRecordScreen.ExitGame += CloseGame;
            _howToPlayPlane.PlayClicked += StartNewGame;
            _exitButton.onClick.AddListener(CloseGame);
        }

        private void OnDisable()
        {
            _net.BallCatched -= IncreaseScore;
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
                _scoreHolder.gameObject.SetActive(false);
                return;
            }
            
            _howToPlayPlane.gameObject.SetActive(false);
            _ball.gameObject.SetActive(true);
            _scoreHolder.gameObject.SetActive(true);
            StartNewGame();
        }
        
        private void StartNewGame()
        {
            _ball.gameObject.SetActive(true);
            _scoreHolder.gameObject.SetActive(true);
            ResetValues();
            UpdateScoreUI();
            UpdateTimerUI();
            StartTimer();
            _ball.EnableMovement();
            _endGameScreen.DisableScreen();
            _newRecordScreen.DisableScreen();
            UpdateRecordText();
            _pole.SetRandomPosition();
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
            _pole.SetRandomPosition();
        }

        private void StopGame()
        {
            _ball.DisableMovement();
            _ball.SetDefault();
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
            _timerValue += _timerIncreaseValue;
            UpdateScoreUI();
            _ball.SetDefault();
            _pole.SetRandomPosition();
        }

        private void CloseGame()
        {
            GameState.ShowTutorial = false;
            SceneManager.LoadScene("MainScene");
        }
    }
}
