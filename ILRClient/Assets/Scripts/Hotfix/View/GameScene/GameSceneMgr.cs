using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMgr
{
    static GameSceneMgr _Instance;
    public GameSceneMgr Instance 
    { 
        get 
        {
            if (_Instance == null)
            {
                _Instance = new GameSceneMgr();
            }

            return _Instance;
        } 
    }

    private GameSceneMgr() { }


    private IGameScene Current;

    public void OnTick()
    {

    }
}
