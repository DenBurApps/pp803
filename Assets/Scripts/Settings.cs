using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _privacyCanvas;
    [SerializeField] private GameObject _termsCanvas;
    [SerializeField] private GameObject _contactCanvas;
    [SerializeField] private GameObject _versionCanvas;
    [SerializeField] private TMP_Text _versionText;
    [SerializeField] private Button _onButton;
    [SerializeField] private Button _offButton;
    [SerializeField] private StartMenuScreen _startMenuScreen;
    private string _version = "Application version:\n";

    private void Awake()
    {
        _settingsCanvas.SetActive(false);
        _privacyCanvas.SetActive(false);
        _termsCanvas.SetActive(false);
        _contactCanvas.SetActive(false);
        _versionCanvas.SetActive(false);
        SetVersion();
    }

    private void SetVersion()
    {
        _versionText.text = _version + Application.version;
    }

    public void ShowSettings()
    {
        _settingsCanvas.SetActive(true);

        if (!AudioListener.pause)
        {
            _onButton.gameObject.SetActive(false);
            _offButton.gameObject.SetActive(true);
        }
        else
        {
            _onButton.gameObject.SetActive(true);
            _offButton.gameObject.SetActive(false);
        }
    }

    public void DisableSounds()
    {
        AudioListener.pause = true;
        _onButton.gameObject.SetActive(true);
        _offButton.gameObject.SetActive(false);
    }

    public void EnableSounds()
    {
        AudioListener.pause = false;
        _onButton.gameObject.SetActive(false);
        _offButton.gameObject.SetActive(true);
    }

    public void RateUs()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}
