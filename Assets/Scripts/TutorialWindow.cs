using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindow : MonoBehaviour
{
    public event Action PlayClicked;

    public void OnPlayClicked()
    {
        PlayClicked?.Invoke();
        gameObject.SetActive(false);
    }
}
