using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PowerCardData
{
    public PowerCard hpCard = new PowerCard
    {
        name = "Bonus HP",
        description = "Increases max healthpoints by a flat amount",
        statAffected = "healthPoints",
        Lvl1Stength = 10,
        Lvl2Stength = 20,
        Lvl3Stength = 30,
        Lvl4Stength = 40,
        Lvl5Stength = 50
    };

    public PowerCard speedCard = new PowerCard
    {
        name = "Pointer Speed",
        description = "Decreases pointer speed by a percent amount",
        statAffected = "speedReduction",
        Lvl1Stength = 10,
        Lvl2Stength = 20,
        Lvl3Stength = 50,
        Lvl4Stength = 75,
        Lvl5Stength = 100
    };

    public PowerCard greenCard = new PowerCard
    {
        name = "Green Size",
        description = "Increases the size of the Green Zone by a percent amount while decreasing the Red Zone",
        statAffected = "pointerLvl",
        Lvl1Stength = 22,
        Lvl2Stength = 24,
        Lvl3Stength = 26,
        Lvl4Stength = 28,
        Lvl5Stength = 30
    };

    public PowerCard lifestealCard = new PowerCard
    {
        name = "LifeSteal",
        description = "Gain health points back for every green hit",
        statAffected = "lifeStealAmount",
        Lvl1Stength = 0.2f,
        Lvl2Stength = 0.4f,
        Lvl3Stength = 0.6f,
        Lvl4Stength = 0.8f,
        Lvl5Stength = 1f
    };

    public PowerCard skipCard = new PowerCard
    {
        name = "Stage Skip",
        description = "Increases the chance to skip a stage by breaking two ice at a time",
        statAffected = "skipChance",
        Lvl1Stength = 5,
        Lvl2Stength = 10,
        Lvl3Stength = 15,
        Lvl4Stength = 20,
        Lvl5Stength = 25
    };

    public PowerCard frozenChanceCard = new PowerCard
    {
        name = "Frozen Shard Chance",
        description = "Increases the chance to find Frozen Shards",
        statAffected = "frozenChance",
        Lvl1Stength = 2,
        Lvl2Stength = 5,
        Lvl3Stength = 10,
        Lvl4Stength = 12,
        Lvl5Stength = 15
    };

    public PowerCard frozenAmountCard = new PowerCard
    {
        name = "Frozen Shard Value",
        description = "Increases the amount of Shards in a Frozen Shard",
        statAffected = "frozenAmount",
        Lvl1Stength = 12,
        Lvl2Stength = 15,
        Lvl3Stength = 17,
        Lvl4Stength = 20,
        Lvl5Stength = 25
    };
}