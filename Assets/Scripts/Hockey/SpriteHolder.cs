using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    
    public Sprite GetRandomSprite()
    {
        var randomIndex = Random.Range(0, _sprites.Length);

        return _sprites[randomIndex];
    }
}
