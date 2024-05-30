using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _Instance;
    public static CharacterManager Instance
    {
        get
        {
            if( _Instance == null)
            {
                _Instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _Instance;
        }
    }
    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        if(_Instance == null) 
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(_Instance != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
