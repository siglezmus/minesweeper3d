using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool visible = false;

    public bool mine = false;

    public int number;

    public int coins = 3;

    public int row;

    public int col;

    public void Open(Player p)
    {
        p.IncreaseCoins(coins);
        if(mine && !p.currentMovingType)
            p.DecreaseHealth();
        p.SpendOneTurnStamina();

        visible = true;
        mine = false;
        coins = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        
    }
}
