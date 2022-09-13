using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    //Add stats to this and save upon game end and upon purchase

    /* POWER CARDS 
     * Stores PC Lvls and amount owned
     */
    public int UnlockLvlHP;
    public int UnlockLvlSpd;
    public int UnlockLvlGreen;
    public int UnlockLvlLifeSteal;
    public int UnlockLvlSkip;
    public int UnlockLvlFrozenChance;
    public int UnlockLvlFrozenAmount;

    public int CardsOwnedHP;
    public int CardsOwnedSpd;
    public int CardsOwnedGreen;
    public int CardsOwnedLifesteal;
    public int CardsOwnedSkip;
    public int CardsOwnedFrozenChance;
    public int CardsOwnedFrozenAmount;

    /* PLAYER STATS 
     * Stores player stats 
     */
    public int Shards;
    public int HighScore;
    public int GamesPlayed;
    public int GreenHits;
    public int OrangeHits;
    public int RedHits;
    public int ForzenShardsFound;
    public int IceBroken;
    public int StagesSkipped;
}