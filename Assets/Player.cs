using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int safeTurnStaminaCost = 10;

    public int unsafeTurnStaminaCost = 5;

    public bool currentMovingType = true;

    public int coins = 0;

    public int currentHealth = 3;

    public int maxHealth = 3;

    public int currentStamina = 100;

    public int maxStamina = 100;

    public int abilityOne = 0;// those ones are id of ability

    public int abilityTwo = 1;

    int abilitiyOneCooldown = 0;

    int abilitiyTwoCooldown = 0;

    private Board board; // required for abilities to affect board state

    // Start is called before the first frame update

    public Player(Board board)
    {
        this.board = board;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPlayerAlive()
    {
        if (currentHealth > 0)
            return true;
        else
            return false;
    }

    public bool IsThereEnoughStaminaToMove()
    {
        if (currentMovingType && currentStamina >= 10)
            return true;
        else if (!currentMovingType && currentStamina >= 5)
            return true;
        else
            return false;
    }

    public bool IsThereUnusedAbility()
    {
        if (abilitiyOneCooldown == 0 || abilitiyTwoCooldown == 0)
            return true;
        else
            return false;
    }

    public void IncreaseCoins(int amount)
    {
        coins += amount;
    }

    public void SwitchMovingType()
    {
        currentMovingType = !currentMovingType;
    }

    public void SpendOneTurnStamina()
    {
        currentStamina -= currentMovingType ? safeTurnStaminaCost : unsafeTurnStaminaCost;
    }

    public void DecreaseHealth()
    {
        currentHealth -= 1;
    }

    public void EndTurnReset()
    {
        currentStamina = maxStamina;

        if (abilitiyOneCooldown != 0)
            abilitiyOneCooldown -= 1;

        if (abilitiyTwoCooldown != 0)
            abilitiyTwoCooldown -= 1;
    }

    
}
