using System.Collections;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hockey
{
    public class GameController : MonoBehaviour
    {
        private const int Interval = 1;
        
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _recordText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Player _player;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private TutorialWindow _tutorialWindow;
        [SerializeField] private NewRecordScreen _newRecordScreen;
        [SerializeField] private EndGameScreen _endGameScreen;
        [SerializeField] private GameType _gameType;
        
        private int _currentScore;
        private IEnumerator _scoreIncreasingCoroutine;
        
        private void OnEnable()
        {
            _player.HitObsticle += EndGame;
            
            _endGameScreen.RestartGame += StartNewGame;
            _endGameScreen.ExitGame += CloseGame;
            _newRecordScreen.RestartGame += StartNewGame;
            _newRecordScreen.ExitGame += CloseGame;
            _tutorialWindow.PlayClicked += StartNewGame;
            
            _backButton.onClick.AddListener(CloseGame);
        }

        private void OnDisable()
        {
            _player.HitObsticle -= EndGame;
            
            _endGameScreen.RestartGame -= StartNewGame;
            _endGameScreen.ExitGame -= CloseGame;
            _newRecordScreen.RestartGame -= StartNewGame;
            _newRecordScreen.ExitGame -= CloseGame;
            _tutorialWindow.PlayClicked -= StartNewGame;
            
            _backButton.onClick.RemoveListener(CloseGame);
        }
        
        private void Start()
        {
            _endGameScreen.DisableScreen();
            _newRecordScreen.DisableScreen();
            
            if (GameState.ShowTutorial)
            {
                _tutorialWindow.gameObject.SetActive(true);
                _player.gameObject.SetActive(false);
                return;
            }
            
            _tutorialWindow.gameObject.SetActive(false);
            _player.gameObject.SetActive(true);
            StartNewGame();
        }

        private void StartNewGame()
        {
            _tutorialWindow.gameObject.SetActive(false);
            _player.gameObject.SetActive(true);
            ResetValues();
            UpdateScoreText();
            UpdateRecordScoreText();
            StartIncreaseScore();
            _player.EnableMovement();
            _obstacleSpawner.StartSpawning();
            _endGameScreen.DisableScreen();
            _newRecordScreen.DisableScreen();
        }

        private void StartIncreaseScore()
        {
            StopIncreaseScore();

            _scoreIncreasingCoroutine = IncreaseScore();
            StartCoroutine(_scoreIncreasingCoroutine);
        }

        private void StopIncreaseScore()
        {
            if (_scoreIncreasingCoroutine != null)
            {
                StopCoroutine(_scoreIncreasingCoroutine);
                _scoreIncreasingCoroutine = null;
            }
        }
        
        private IEnumerator IncreaseScore()
        {
            var interval = new WaitForSeconds(Interval);

            while (true)
            {
                _currentScore++;
                UpdateScoreText();
                
                yield return interval;
            }
        }

        private void UpdateScoreText()
        {
            _scoreText.text = _currentScore + "m";
        }

        private void UpdateRecordScoreText()
        {
            _recordText.text = RecordHolder.GetRecordByType(_gameType) + "m";
        }

        private void ResetValues()
        {
            _currentScore = 0;
        }

        private void StopGame()
        {
            StopIncreaseScore();
            _obstacleSpawner.StopSpawning();
            _obstacleSpawner.ReturnAllObjectsToPool();
            _player.DisableMovement();
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
        
        private void CloseGame()
        {
            GameState.ShowTutorial = false;
            SceneManager.LoadScene("MainScene");
        }
    }
}
