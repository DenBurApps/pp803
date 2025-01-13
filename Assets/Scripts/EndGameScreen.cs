using System;
using System.Collections;
using System.Collections.Generic;
using RecordSystem;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class EndGameScreen : MonoBehaviour
{
   [SerializeField] private TMP_Text _scoreText;
   [SerializeField] private TMP_Text _recordText;
   
   public event Action RestartGame;
   public event Action ExitGame;

   private ScreenVisabilityHandler _screenVisabilityHandler;

   private void Awake()
   {
      _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
   }

   public void EnableScreen(int score, GameType gameType)
   {
      _screenVisabilityHandler.EnableScreen();
      _scoreText.text = score.ToString();
      _recordText.text = "Your record: " + RecordHolder.GetRecordByType(gameType);
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
