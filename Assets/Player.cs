using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int safeTurnStaminaCost = 10;

    public int unsafeTurnStaminaCost = 5;

    public bool currentMovingType = true;

    public bool movingTypeSwitched = false;

    public int coins = 0;

    public int currentHealth = 3;

    public int maxHealth = 3;

    public int currentStamina = 100;

    public int maxStamina = 100;

    public int abilityOne = 1;// those ones are id of ability

    public int abilityTwo = 2;

    public int abilitiyOneCooldown = 0;

    public int abilitiyTwoCooldown = 0;
    
    // Start is called before the first frame update


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
        if (!movingTypeSwitched)
        {
            currentMovingType = !currentMovingType;
            movingTypeSwitched = true;
        }
        
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
        movingTypeSwitched = false;

        if (abilitiyOneCooldown != 0)
            abilitiyOneCooldown -= 1;

        if (abilitiyTwoCooldown != 0)
            abilitiyTwoCooldown -= 1;
    }

    public void Reset()
    {
        currentStamina = 100;
        currentHealth = 3;
        currentMovingType = true;
        coins = 0;
        movingTypeSwitched = false;
    }

    public void UseAbility1(Board board)
    {
        if(abilitiyOneCooldown == 0)
            UseAbility(board,1, abilityOne);
    }
    public void UseAbility2(Board board)
    {
        if (abilitiyTwoCooldown == 0)
            UseAbility(board,2, abilityTwo);
    }

    public void UseAbility(Board board,int num, int id)
    {

        switch (num)
        {
            case 1:
                if (num == 1)
                { 
                    abilitiyOneCooldown = Ability1();
                }
                else
                {
                    abilitiyTwoCooldown = Ability1();
                }
                
                break;
            case 2:
                if (num == 1)
                {
                    abilitiyOneCooldown = Ability2(board);
                }
                else
                {
                    abilitiyTwoCooldown = Ability2(board);
                }
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }

    public int Ability1()
    {
        currentHealth = maxHealth;
        return 10;
    }
    public int Ability2(Board board)
    {
        board.swapPlayers();
        return 10;
    }




}
