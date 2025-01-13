using System;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class MainScreenRecordShower : MonoBehaviour
    {
        [SerializeField] private TMP_Text _record;
        [SerializeField] private GameType _gameType;
        
        private void Start()
        {
            _record.text = "Record: " + RecordSystem.RecordHolder.GetRecordByType(_gameType);
        }
    }
}
