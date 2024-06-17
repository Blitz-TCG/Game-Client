using System;

public class EndGame : Card
{
    public int attackToBeIncreased = 2;
    public int healthToBeIncreased = 2;

    public void SetAbility(int attackVal = 2, int healthVal = 2)  
    {
        attackToBeIncreased = attackVal;
        healthToBeIncreased = healthVal; 
    }

    public Tuple<int, int> SetAttackAndHealth(Card currentCard)
    {
        int cardAttackValue = currentCard.SetCardAttack(attackToBeIncreased, 99);
        int cardHealthValue = currentCard.HealCard(healthToBeIncreased, 99);
        return new Tuple<int, int>(cardAttackValue, cardHealthValue);
    }
}
