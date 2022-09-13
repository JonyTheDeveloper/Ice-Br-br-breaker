using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Reference to GameManager
    GameManager GameScript;

    //Menu and Game UI Canvas
    public GameObject menu;
    public GameObject game;
    public GameObject pause;

    //Array of the sub menus on the main menu Canvas
    public GameObject[] menus;

    //Array of the gradients
    public Sprite[] gradiants;

    //HP Bar Variables
    public Slider HP;
    public Image HPFill;
    public Text hpText;
    public Gradient hpGradient;

    //Game Over PopUp Variables
    public GameObject goPanel;
    public GameObject newHighScoreText;
    public Text highscoreText;
    public Text scorethisgameText;
    public Text gamesPlayedText;

    //Frozen Shard
    public GameObject frozenShard;

    //Score Variables
    public Text scoreText;

    //Shard Text
    public Text shardText;

    //Stage Skipped Text
    public GameObject skippedText;
    //Reference to UIManager
    TweenManager tweenScript;

    //Array of stats Texts
    public Text[] stats;

    //Pause Screen Current Stats
    public Text[] currentStats;

    //Pointer background
    public Image pointerBG;

    //Shop Card Lvls
    public Text[] cardLvls;

    //Shop Card strengths
    public Text[] uicardstrength;
    public GameObject uicardPopUpPanel;
    public Text uicardName;
    public Text uicardDesc;
    public Text uicardLvl;
    public Text uicardAmount;
    public Text newcardCostText;

    //Card Values
    /*Edit these when card = 0 to ??? and ? image
     * 
     */
    public Image[] cardImages;
    public Sprite[] cardImagesSprites;
    public Text[] cardNamesText;
    public String[] cardNames = new String[7];
    public Image popupCardImage;
    public Sprite questionSprite;

    //Array of cards owned
    public int[] cardsOwned;

    //Stats pannel
    public GameObject statsPanel;

    //Settings
    public GameObject MusicSlider;
    public GameObject SFXSlider;
    public Text musicValue;
    public Text sfxValue;

    //Panel Changer 
    public GameObject MenuPanel;
    public Sprite basePanel;
    public Sprite topPanel;
    public Sprite bottomPanel;

    //GameOver AD Manager UI Variables
    public GameObject GOADPanel;
    public Button GOADButton;
    public GameObject GOBackgroundPanel;

    public void Start()
    {
        //Assign GameManager reference
        GameScript = GetComponent<GameManager>();

        //Assign TweenManager reference
        tweenScript = GetComponent<TweenManager>();

        shardText.text = "" + GameScript.SaveData.Shards;

        shopUpdate();
        loadSettings();
    }

    //Called when hitting the play button
    public void playBtn()
    {
        /*Smash ice
         * spawn next ice
         * move ice
         * wait 0.5 seconds
         * swap state
         */

        //Swap scenes from Menu Canvas to Game Canvas
        GameScript.iceBreak();
        //menuTweenOff();
        GameScript.state = GameManager.State.Game;
    }

    //Called when hitting the play button
    public void menuBtn()
    {
        //Swap scenes from Menu Canvas to Game Canvas
        GameScript.restartGame();
        GameScript.state = GameManager.State.Menu;
    }
    public void restartBtn()
    {
        //Swap scenes from Menu Canvas to Game Canvas
        GameScript.restartGame();
        GameScript.state = GameManager.State.Game;
    }

    //Controls the sub menu
    public void menuManager(int menu)
    {

        shopUpdate();

        //Saves the state of the current/clicked menu
        bool currentSatus = menus[menu].activeSelf;

        //Closes all menus
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].activeSelf == true)
            {
                menus[i].SetActive(false);
                closeStats();
                MenuPanel.GetComponent<Image>().sprite = basePanel;
            }
        }
        
        //If current menu is closed; Open it
        if (currentSatus == false)
        {
            menus[menu].SetActive(true);
            MenuPanel.GetComponent<Image>().sprite = topPanel;
        }

        if (menus[1].activeSelf == true)
        {
            //If settings menu is open, update stats page
            int totalcards = GameScript.SaveData.CardsOwnedFrozenAmount + GameScript.SaveData.CardsOwnedFrozenChance + GameScript.SaveData.CardsOwnedGreen + GameScript.SaveData.CardsOwnedHP + GameScript.SaveData.CardsOwnedLifesteal + GameScript.SaveData.CardsOwnedSkip + GameScript.SaveData.CardsOwnedSpd;
            
            stats[0].text = "Games played: " + GameScript.SaveData.GamesPlayed;
            stats[1].text = "HighScore: " + GameScript.SaveData.HighScore;
            stats[2].text = "Green hits: " + GameScript.SaveData.GreenHits;
            stats[3].text = "Orange hits: " + GameScript.SaveData.OrangeHits;
            stats[4].text = "Red hits: " + GameScript.SaveData.RedHits;
            stats[5].text = "Frozen Shards found: " + GameScript.SaveData.ForzenShardsFound;
            stats[6].text = "Ice blocks broken: " + GameScript.SaveData.GamesPlayed;
            stats[7].text = "Stages skipped: " + GameScript.SaveData.StagesSkipped;
            stats[8].text = "Total Cards Owned: " + totalcards + "/" + (31*7);
        }
    }

    //Update Score and HP text at the same time
    public void uiUpdate()
    {
        scoreText.text = "Score: " + GameScript.score;
        hpText.text = "HP: " + GameScript.healthPoints;
        shardText.text = "" + GameScript.SaveData.Shards;

        //Updates HP Slider and colour gradient
        HP.value = GameScript.healthPoints;
        HPFill.color = hpGradient.Evaluate(HP.normalizedValue);
    }

    //Update Shop UI
    public void shopUpdate()
    {
        shardText.text = "" + GameScript.SaveData.Shards;
        newcardCostText.text = GameScript.SaveData.Shards + "/25";

        //Update Shop stars
        cardLvls[0].text = new String('★', GameScript.SaveData.UnlockLvlHP);
        cardLvls[1].text = new String('★', GameScript.SaveData.UnlockLvlSpd);
        cardLvls[2].text = new String('★', GameScript.SaveData.UnlockLvlGreen);
        cardLvls[3].text = new String('★', GameScript.SaveData.UnlockLvlLifeSteal);
        cardLvls[4].text = new String('★', GameScript.SaveData.UnlockLvlSkip);
        cardLvls[5].text = new String('★', GameScript.SaveData.UnlockLvlFrozenChance);
        cardLvls[6].text = new String('★', GameScript.SaveData.UnlockLvlFrozenAmount);

        cardsOwned[0] = GameScript.SaveData.CardsOwnedHP;
        cardsOwned[1] = GameScript.SaveData.CardsOwnedSpd;
        cardsOwned[2] = GameScript.SaveData.CardsOwnedGreen;
        cardsOwned[3] = GameScript.SaveData.CardsOwnedLifesteal;
        cardsOwned[4] = GameScript.SaveData.CardsOwnedSkip;
        cardsOwned[5] = GameScript.SaveData.CardsOwnedFrozenChance;
        cardsOwned[6] = GameScript.SaveData.CardsOwnedFrozenAmount;

        cardNames[0] = GameScript.cards.hpCard.name;
        cardNames[1] = GameScript.cards.speedCard.name;
        cardNames[2] = GameScript.cards.greenCard.name;
        cardNames[3] = GameScript.cards.lifestealCard.name;
        cardNames[4] = GameScript.cards.skipCard.name;
        cardNames[5] = GameScript.cards.frozenChanceCard.name;
        cardNames[6] = GameScript.cards.frozenAmountCard.name;

        //Set card amount for next lvl
        for (int i = 0; i < cardsOwned.Length; i++)
        {
            cardLvls[i].text += "\n" + cardsOwned[i] + "/";

            if (cardsOwned[i] > 0)
            {
                //If i own the card: update text and image
                cardNamesText[i].text = cardNames[i];
                cardImages[i].sprite = cardImagesSprites[i];
            }

            if (cardsOwned[i] < 1)
            {
                cardLvls[i].text += 1;
            }
            else if (cardsOwned[i] < 3)
            {
                cardLvls[i].text += 3;
            }
            else if (cardsOwned[i] < 7)
            {
                cardLvls[i].text += 7;
            }
            else if (cardsOwned[i] < 15)
            {
                cardLvls[i].text += 15;
            }else if (cardsOwned[i] <= 31)
            {
                cardLvls[i].text += 31;
            }
        }
    }

    //Card pop up
    public void cardPopUp(int ID)
    {
        //Variables for the current card
        string cardName = "???";
        string cardDescription = "A misterious card cointaining a unique power yet to be uncovered!";
        int cardLvl = 0;
        int cardsOwned = 0;
        int maxCards = 0;
        string[] cardStrength = new string[5];

        uicardPopUpPanel.SetActive(true);

        //Collect the necessary data on the card selected
        switch (ID)
        {
            case 0:
                if (GameScript.SaveData.CardsOwnedHP == 0)
                {
                    popupCardImage.sprite = questionSprite;
                }
                else
                {
                    cardName = GameScript.cards.hpCard.name;
                    cardDescription = GameScript.cards.hpCard.description;
                    cardLvl = GameScript.SaveData.UnlockLvlHP;
                    cardsOwned = GameScript.SaveData.CardsOwnedHP;
                    popupCardImage.sprite = cardImagesSprites[0];
                    cardStrength[0] = "+" + GameScript.cards.hpCard.Lvl1Stength + " HP";
                    cardStrength[1] = "+" + GameScript.cards.hpCard.Lvl2Stength + " HP";
                    cardStrength[2] = "+" + GameScript.cards.hpCard.Lvl3Stength + " HP";
                    cardStrength[3] = "+" + GameScript.cards.hpCard.Lvl4Stength + " HP";
                    cardStrength[4] = "+" + GameScript.cards.hpCard.Lvl5Stength + " HP";
                }
                break;
            case 1:
                if (GameScript.SaveData.CardsOwnedSpd == 0)
                {
                    popupCardImage.sprite = questionSprite;
                }
                else
                {
                    cardName = GameScript.cards.speedCard.name;
                    cardDescription = GameScript.cards.speedCard.description;
                    cardLvl = GameScript.SaveData.UnlockLvlSpd;
                    cardsOwned = GameScript.SaveData.CardsOwnedSpd;
                    popupCardImage.sprite = cardImagesSprites[1];
                    cardStrength[0] = "-" + (GameScript.cards.speedCard.Lvl1Stength / 100) + "%";
                    cardStrength[1] = "-" + (GameScript.cards.speedCard.Lvl2Stength / 100) + "%";
                    cardStrength[2] = "-" + (GameScript.cards.speedCard.Lvl3Stength / 100) + "%";
                    cardStrength[3] = "-" + (GameScript.cards.speedCard.Lvl4Stength / 100) + "%";
                    cardStrength[4] = "-" + (GameScript.cards.speedCard.Lvl5Stength / 100) + "%";
                }
                break;
            case 2:
                if (GameScript.SaveData.CardsOwnedGreen == 0)
                {
                    popupCardImage.sprite = questionSprite;
                }
                else
                {
                    cardName = GameScript.cards.greenCard.name;
                    cardDescription = GameScript.cards.greenCard.description;
                    cardLvl = GameScript.SaveData.UnlockLvlGreen;
                    cardsOwned = GameScript.SaveData.CardsOwnedGreen;
                    popupCardImage.sprite = cardImagesSprites[2];
                    cardStrength[0] = "+" + GameScript.cards.greenCard.Lvl1Stength + "%";
                    cardStrength[1] = "+" + GameScript.cards.greenCard.Lvl2Stength + "%";
                    cardStrength[2] = "+" + GameScript.cards.greenCard.Lvl3Stength + "%";
                    cardStrength[3] = "+" + GameScript.cards.greenCard.Lvl4Stength + "%";
                    cardStrength[4] = "+" + GameScript.cards.greenCard.Lvl5Stength + "%";
                }
                break;
            case 3:
                if (GameScript.SaveData.CardsOwnedLifesteal == 0)
                {
                    popupCardImage.sprite = questionSprite;
                }
                else
                {
                    cardName = GameScript.cards.lifestealCard.name;
                    cardDescription = GameScript.cards.lifestealCard.description;
                    cardLvl = GameScript.SaveData.UnlockLvlLifeSteal;
                    cardsOwned = GameScript.SaveData.CardsOwnedLifesteal;
                    popupCardImage.sprite = cardImagesSprites[3];
                    cardStrength[0] = "+" + GameScript.cards.lifestealCard.Lvl1Stength + "%";
                    cardStrength[1] = "+" + GameScript.cards.lifestealCard.Lvl2Stength + "%";
                    cardStrength[2] = "+" + GameScript.cards.lifestealCard.Lvl3Stength + "%";
                    cardStrength[3] = "+" + GameScript.cards.lifestealCard.Lvl4Stength + "%";
                    cardStrength[4] = "+" + GameScript.cards.lifestealCard.Lvl5Stength + "%";
                }
                break;
            case 4:
                if (GameScript.SaveData.CardsOwnedSkip == 0)
                {
                    popupCardImage.sprite = questionSprite;
                }
                else
                {
                    cardName = GameScript.cards.skipCard.name;
                    cardDescription = GameScript.cards.skipCard.description;
                    cardLvl = GameScript.SaveData.UnlockLvlSkip;
                    cardsOwned = GameScript.SaveData.CardsOwnedSkip;
                    popupCardImage.sprite = cardImagesSprites[4];
                    cardStrength[0] = "+" + GameScript.cards.skipCard.Lvl1Stength + "%";
                    cardStrength[1] = "+" + GameScript.cards.skipCard.Lvl2Stength + "%";
                    cardStrength[2] = "+" + GameScript.cards.skipCard.Lvl3Stength + "%";
                    cardStrength[3] = "+" + GameScript.cards.skipCard.Lvl4Stength + "%";
                    cardStrength[4] = "+" + GameScript.cards.skipCard.Lvl5Stength + "%";
                }
                break;
            case 5:
                if (GameScript.SaveData.CardsOwnedFrozenChance == 0)
                {
                    popupCardImage.sprite = questionSprite;
                }
                else
                {
                    cardName = GameScript.cards.frozenChanceCard.name;
                    cardDescription = GameScript.cards.frozenChanceCard.description;
                    cardLvl = GameScript.SaveData.UnlockLvlFrozenChance;
                    cardsOwned = GameScript.SaveData.CardsOwnedFrozenChance;
                    popupCardImage.sprite = cardImagesSprites[5];
                    cardStrength[0] = "+" + GameScript.cards.frozenChanceCard.Lvl1Stength + "%";
                    cardStrength[1] = "+" + GameScript.cards.frozenChanceCard.Lvl2Stength + "%";
                    cardStrength[2] = "+" + GameScript.cards.frozenChanceCard.Lvl3Stength + "%";
                    cardStrength[3] = "+" + GameScript.cards.frozenChanceCard.Lvl4Stength + "%";
                    cardStrength[4] = "+" + GameScript.cards.frozenChanceCard.Lvl5Stength + "%";
                }
                break;
            case 6:
                if (GameScript.SaveData.CardsOwnedFrozenAmount == 0)
                {
                    popupCardImage.sprite = questionSprite;
                }
                else
                {
                    cardName = GameScript.cards.frozenAmountCard.name;
                    cardDescription = GameScript.cards.frozenAmountCard.description;
                    cardLvl = GameScript.SaveData.UnlockLvlFrozenAmount;
                    cardsOwned = GameScript.SaveData.CardsOwnedFrozenAmount;
                    popupCardImage.sprite = cardImagesSprites[6];
                    cardStrength[0] = "+" + GameScript.cards.frozenAmountCard.Lvl1Stength + "%";
                    cardStrength[1] = "+" + GameScript.cards.frozenAmountCard.Lvl2Stength + "%";
                    cardStrength[2] = "+" + GameScript.cards.frozenAmountCard.Lvl3Stength + "%";
                    cardStrength[3] = "+" + GameScript.cards.frozenAmountCard.Lvl4Stength + "%";
                    cardStrength[4] = "+" + GameScript.cards.frozenAmountCard.Lvl5Stength + "%";
                }
                break;
        }

        //Set UI based on colected data
        uicardName.text = cardName;
        uicardDesc.text = cardDescription;
        uicardLvl.text = "Level " + cardLvl.ToString();
        
        if (cardsOwned < 1)
        {
            maxCards = 1;
        }
        else if (cardsOwned < 3)
        {
            maxCards = 3;
        }
        else if (cardsOwned < 7)
        {
            maxCards = 7;
        }
        else if (cardsOwned < 15)
        {
            maxCards = 15;
        }
        else if (cardsOwned <= 31)
        {
            maxCards = 31;
        }

        uicardAmount.text = cardsOwned.ToString() + "/" + maxCards.ToString();

        for (int i = 0; i < cardStrength.Length; i++)
        {
            uicardstrength[i].text = cardStrength[i];
        }
    }

    //Hide card info pop up
    public void hideCardPopUp()
    {
        uicardPopUpPanel.SetActive(false);
    }

    //Show stats page
    public void showStats()
    {
        statsPanel.SetActive(true);
    }

    //Hide stats page
    public void closeStats()
    {
        statsPanel.SetActive(false);
    }

    //Load settings values from Prefs
    public void loadSettings()
    {
        MusicSlider.GetComponent<Slider>().value = PlayerPrefs.GetInt(GameScript.prefs[0]);
        SFXSlider.GetComponent<Slider>().value = PlayerPrefs.GetInt(GameScript.prefs[1]);

        musicValue.text = "" + PlayerPrefs.GetInt(GameScript.prefs[0]);
        sfxValue.text = "" + PlayerPrefs.GetInt(GameScript.prefs[1]);
    }

    //Save changes to music
    public void saveMusic()
    {
        musicValue.text = "" + MusicSlider.GetComponent<Slider>().value;
        PlayerPrefs.SetInt(GameScript.prefs[0], (int)MusicSlider.GetComponent<Slider>().value);
        PlayerPrefs.Save();
    }

    //Save changes to SFX
    public void saveSFX()
    {
        sfxValue.text = "" + SFXSlider.GetComponent<Slider>().value;
        PlayerPrefs.SetInt(GameScript.prefs[1], (int)SFXSlider.GetComponent<Slider>().value);
        PlayerPrefs.Save();
    }

    //Tweens the "Skipped" text for added effect
    public void SkipTweenIn()
    {
        LeanTween.scale(skippedText, new Vector3(1.5f, 1.5f, 1.5f), 0.25f).setOnComplete(SkipTweenOut).setEase(LeanTweenType.easeOutSine);
    }
    public void SkipTweenOut()
    {
        LeanTween.scale(skippedText, new Vector3(0, 0, 0), 0.5f).setEase(LeanTweenType.easeInSine);
    }

    public void menuTweenOff()
    {
        /*LeanTween.moveY(MenuPanel, -500f, 0.5f).setOnComplete(swaptogamestate);
        LeanTween.moveY(MenuPanel, -500f, 0.5f).setOnComplete(swaptogamestate);
        LeanTween.moveY(MenuPanel, -500f, 0.5f).setOnComplete(swaptogamestate);*/

        MenuPanel.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Vector3 force = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5));
        MenuPanel.GetComponent<Rigidbody2D>().AddForce(force * 1000);
        MenuPanel.GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(-5, 5) * 100);

        StartCoroutine("swaptogamestate");
    }

    public IEnumerator swaptogamestate()
    {
        yield return new WaitForSeconds(60);
        GameScript.state = GameManager.State.Game;
    }

    //Open leaderboards
    public void viewLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkImNymn4cbEAIQFA");
    }

    //Open acievements list
    public void viewAchievements()
    {
        Social.ShowAchievementsUI();
    }
}