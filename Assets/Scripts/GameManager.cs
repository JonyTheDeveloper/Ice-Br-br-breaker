using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class GameManager : MonoBehaviour
{
    //Reference to UIManager
    UIManager UIScript;
    //Reference to SaveManager
    SaveManager SaveScript;

    //Game stat manager
    public enum State
    {
        Menu,
        Game,
        Pause,
        GameOver,
    }
    public State state;

    //Pointer slider
    public Slider Arrow;

    //Score Variables
    public int score;

    //Current Stat Gains
    public int currentGreenHits;
    public int currentOrangeHits;
    public int currentRedHits;
    public int currentIceBroken;
    public int currentStagesSkipped;
    public int currentFrozenShards;

    //Array of PlayerPrefs keys for quick preperation
    public string[] prefs;

    //Player Stats
    public bool gameStarted;
    public int healthPoints;
    public int hpIncrease;
    public int skipChance;
    public int frozenChance;
    public float frozenAmount;
    public float lifeStealAmount;
    public float lifeStolen;
    public int greenSize;
    public float speedReduction;
    public float extraShards;
    public float currentShardsGained;
    public int pointerLvl;

    //Pointer values
    private float R1min;
    private float R1max;
    private float R2min;
    private float R2max;
    private float O1min;
    private float O1max;
    private float O2min;
    private float O2max;
    private float Gmin;
    private float Gmax;

    //Timer for pointer
    private float timer;

    //Save and load data
    public SaveData SaveData = new SaveData();
    public PowerCardData cards;
    public int[] cardLevels;

    //Shattered Ice Prefabs
    public GameObject[] shatter;

    //GameOver AD Manager Variables
    private int ADChance;

    //Bool to check if connected to google play services
    public bool isConnectedToGooglePlayServices;

    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        //Set starting state
        state = State.Menu;

        //Assign UIManager script reference
        UIScript = GetComponent<UIManager>();

        //Assign SaveManager script reference
        SaveScript = GetComponent<SaveManager>();

        //Set Health points
        healthPoints = 100;

        gameStarted = false;

        //Create a local list of card data
        cards = new PowerCardData();

        //Setup Prefs
        prefsSetup();

        //Sign in to Play Games Services
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    void Update()
    {
        //Sate Manager
        switch (state)
        {
            case State.Menu:
                GetComponent<ADManager>().HideBanner();

                Time.timeScale = 1;

                gameStarted = false;

                //Set Menu UI as active
                if (UIScript.game.activeSelf)
                {   
                    UIScript.menu.SetActive(true);
                    UIScript.game.SetActive(false);
                }
                break;
            case State.Game:
                GetComponent<ADManager>().HideBanner();

                Time.timeScale = 1;
                timer += Time.deltaTime;

                if (gameStarted == false)
                {
                    //Chech stats based on card levels
                    statCheck();
                    //Update UI with the current stats
                    UIScript.uiUpdate();
                    gameStarted = true;
                }

                //Set Game UI as active
                if (UIScript.menu.activeSelf)
                {
                    UIScript.menu.SetActive(false);
                    UIScript.game.SetActive(true);
                }

                //Hide Pause UI if necessary
                if (UIScript.pause.activeSelf)
                {
                    UIScript.pause.SetActive(false);
                }

                //Moves arrow up and down
                //Speed increases per score aquired
                timer += (score / (25 * (1 + speedReduction))) * Time.deltaTime;
                if (timer > 1f * 2)
                    timer -= 1f * 2;
                Arrow.value = Mathf.PingPong(timer, 1f);

                //Handle click/tap inputs
                if (Input.GetMouseButtonDown(0) && healthPoints > 0)
                {
                    //Check pointer position and worth
                    pointerLogic(Arrow.value);

                    //Update score and HP text
                    UIScript.uiUpdate();
                    
                    //Check current health for a GameOver
                    if (healthPoints <= 0)
                    {
                        gameOver();
                        state = State.GameOver;
                    }
                }

                //Handle the back button
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    // Check if Back was pressed this frame
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        state = State.Pause;
                    }
                }

                break;
            case State.Pause:
                //Freezes pointer in place
                Time.timeScale = 0;

                //Update Pause UI
                UIScript.currentStats[0].text = "" + SaveData.HighScore;
                UIScript.currentStats[1].text = "" + score / 10;
                UIScript.currentStats[2].text = skipChance + "%";
                UIScript.currentStats[3].text = (lifeStealAmount * 100) + "%";
                UIScript.currentStats[4].text = frozenChance + "%";
                UIScript.currentStats[5].text = frozenAmount + "%";
                UIScript.currentStats[6].text = "" + Mathf.Floor(extraShards);

                //Set Pause panel as active
                if (UIScript.pause.activeSelf == false)
                {
                    UIScript.pause.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //If back is pressed again, exit to menu
                    restartGame();
                    state = State.Menu;
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    //Return to game state if tapped
                    state = State.Game;
                }
                break;
            case State.GameOver:
                gameStarted = false;
                StartCoroutine(GetComponent<ADManager>().ShowBannerWhenInitialized());
                //gameOver(); //causes infinte loop of increasing games played amount
                break;
        }
    }

    public void GooglePlayLogIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            isConnectedToGooglePlayServices = true;
            Debug.Log("Logged in");
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            isConnectedToGooglePlayServices = false;
            Debug.Log("Log in failed due to ");
            Debug.Log(status);
        }
    }

    //Setup PlayerPrefs for settings
    public void prefsSetup()
    {
        for (int i = 0; i < prefs.Length; i++)
        {
            if (!PlayerPrefs.HasKey(prefs[i]))
            {
                PlayerPrefs.SetInt(prefs[i], 100);
                PlayerPrefs.Save();
            }
        }
    }

    //Calculates Pointer logic and position and edits Score/HP gain/loss
    public void pointerLogic(float pointerPosition)
    {
        if ((pointerPosition > Gmin) && (pointerPosition < Gmax))
        {
            //Green Hit
            iceBreak();

            if (Random.Range(0,100) < skipChance)
            {
                //Stage Skipped
                UIScript.SkipTweenIn();

                score += 2;

                currentGreenHits += 1;
                currentIceBroken += 2;
                currentStagesSkipped += 2;
            }
            else
            {
                //No skip
                score += 1;

                currentGreenHits += 1;
                currentIceBroken += 1;
            }

            /*Frozen Shards
                check if there curerntly is a shard
                setup shard for next ice block
            */

            if (UIScript.frozenShard.activeSelf)
            {
                if (((score / 10)) <= 0)
                {
                    extraShards += 1 * frozenAmount; //Forces a min ammount gained
                }
                extraShards += (score / 10) * frozenAmount;

                UIScript.frozenShard.SetActive(false);

                if (Random.Range(0, 100) < frozenChance)
                {
                    //Calculate frozen shard for next ice block
                    UIScript.frozenShard.SetActive(true);
                }

                currentFrozenShards += 1;
            }
            else if (Random.Range(0,100) < frozenChance)
            {
                //Calculate frozen shard for next ice block
                UIScript.frozenShard.SetActive(true);
            }

            //Calculate lifesteal
            if (healthPoints < UIScript.HP.maxValue)
            {
                lifeStolen += lifeStealAmount;
            }
            if (Mathf.Floor(lifeStolen) >= 1 && (healthPoints+(int)Mathf.Floor(lifeStolen) < UIScript.HP.maxValue))
            {
                healthPoints += (int)Mathf.Floor(lifeStolen);
                lifeStolen -= Mathf.Floor(lifeStolen);
            }

        }
        else if ((pointerPosition > O1min) && (pointerPosition < O1max))
        {
            //Orange Hit
            healthPoints -= 1;

            currentOrangeHits += 1;
        }
        else if ((pointerPosition > O2min) && (pointerPosition < O2max))
        {
            //Orange Hit
            healthPoints -= 2;

            currentOrangeHits += 1;
        }
        else if ((pointerPosition > R1min) && (pointerPosition < R1max))
        {
            //Red Hit
            healthPoints -= 5;

            currentRedHits += 1;
        }
        else if ((pointerPosition > R2min) && (pointerPosition < R2max))
        {
            //Red Hit
            healthPoints -= 10;

            currentRedHits += 1;
        }
    }

    /*Stat checker
     * Sets stats based on Card Lvls found in Save Data
     */
    public void statCheck()
    {
        //HP Card
        switch (SaveData.UnlockLvlHP)
        {
            case 0:
                hpIncrease = 0;
                break;
            case 1:
                hpIncrease = (int)cards.hpCard.Lvl1Stength;
                break;
            case 2:
                hpIncrease = (int)cards.hpCard.Lvl2Stength;
                break;
            case 3:
                hpIncrease = (int)cards.hpCard.Lvl3Stength;
                break;
            case 4:
                hpIncrease = (int)cards.hpCard.Lvl4Stength;
                break;
            case 5:
                hpIncrease = (int)cards.hpCard.Lvl5Stength;
                break;
        }

        healthPoints = 100 + hpIncrease;

        if (healthPoints > 100)
        {
            UIScript.HP.maxValue = healthPoints;
        }

        //Pointer Speed Card
        switch (SaveData.UnlockLvlSpd)
        {
            case 0:
                speedReduction = 0;
                break;
            case 1:
                speedReduction = cards.speedCard.Lvl1Stength / 100;
                break;
            case 2:
                speedReduction = cards.speedCard.Lvl2Stength / 100;
                break;
            case 3:
                speedReduction = cards.speedCard.Lvl3Stength / 100;
                break;
            case 4:
                speedReduction = cards.speedCard.Lvl4Stength / 100;
                break;
            case 5:
                speedReduction = cards.speedCard.Lvl5Stength / 100;
                break;
        }

        //Green Card
        switch (SaveData.UnlockLvlGreen)
        {
            case 0:
                greenSize = 20;
                break;
            case 1:
                greenSize = (int)cards.greenCard.Lvl1Stength;
                break;
            case 2:
                greenSize = (int)cards.greenCard.Lvl2Stength;
                break;
            case 3:
                greenSize = (int)cards.greenCard.Lvl3Stength;
                break;
            case 4:
                greenSize = (int)cards.greenCard.Lvl4Stength;
                break;
            case 5:
                greenSize = (int)cards.greenCard.Lvl5Stength;
                break;
        }

        //LifeSteal Card
        switch (SaveData.UnlockLvlLifeSteal)
        {
            case 0:
                lifeStealAmount = 0;
                break;
            case 1:
                lifeStealAmount = cards.lifestealCard.Lvl1Stength;
                break;
            case 2:
                lifeStealAmount = cards.lifestealCard.Lvl2Stength;
                break;
            case 3:
                lifeStealAmount = cards.lifestealCard.Lvl3Stength;
                break;
            case 4:
                lifeStealAmount = cards.lifestealCard.Lvl4Stength;
                break;
            case 5:
                lifeStealAmount = cards.lifestealCard.Lvl5Stength;
                break;
        }

        //Stage Skip Chance Card
        switch (SaveData.UnlockLvlSkip)
        {
            case 0:
                skipChance = 0;
                break;
            case 1:
                skipChance = (int)cards.skipCard.Lvl1Stength;
                break;
            case 2:
                skipChance = (int)cards.skipCard.Lvl2Stength;
                break;
            case 3:
                skipChance = (int)cards.skipCard.Lvl3Stength;
                break;
            case 4:
                skipChance = (int)cards.skipCard.Lvl4Stength;
                break;
            case 5:
                skipChance = (int)cards.skipCard.Lvl5Stength;
                break;
        }

        //Frozen Shard Chance Card
        switch (SaveData.UnlockLvlFrozenChance)
        {
            case 0:
                frozenChance = 1;
                break;
            case 1:
                frozenChance = (int)cards.frozenChanceCard.Lvl1Stength;
                break;
            case 2:
                frozenChance = (int)cards.frozenChanceCard.Lvl2Stength;
                break;
            case 3:
                frozenChance = (int)cards.frozenChanceCard.Lvl3Stength;
                break;
            case 4:
                frozenChance = (int)cards.frozenChanceCard.Lvl4Stength;
                break;
            case 5:
                frozenChance = (int)cards.frozenChanceCard.Lvl5Stength;
                break;
        }

        //Frozen Shard Ammount Card
        switch (SaveData.UnlockLvlFrozenAmount)
        {
            case 0:
                frozenAmount = 0.10f;
                break;
            case 1:
                frozenAmount = cards.frozenAmountCard.Lvl1Stength / 100;
                break;
            case 2:
                frozenAmount = cards.frozenAmountCard.Lvl2Stength / 100;
                break;
            case 3:
                frozenAmount = cards.frozenAmountCard.Lvl3Stength / 100;
                break;
            case 4:
                frozenAmount = cards.frozenAmountCard.Lvl4Stength / 100;
                break;
            case 5:
                frozenAmount = cards.frozenAmountCard.Lvl5Stength / 100;
                break;
        }

        //Check pointer type
        pointerLvl = SaveData.UnlockLvlGreen;

        switch (pointerLvl)
        {
            case 0:
                UIScript.pointerBG.sprite = UIScript.gradiants[0];
                R2min = 0f;
                R1min = 0.10f;
                O2min = 0.20f;
                O1min = 0.30f;
                Gmin = 0.40f;
                Gmax = 0.60f;
                O1max = 0.70f;
                O2max = 0.80f;
                R1max = 0.90f;
                R2max = 1f;
                break;
            case 1:
                UIScript.pointerBG.sprite = UIScript.gradiants[1];
                R2min = 0f;
                R1min = 0.9f;
                O2min = 0.19f;
                O1min = 0.29f;
                Gmin = 0.39f;
                Gmax = 0.61f;
                O1max = 0.71f;
                O2max = 0.81f;
                R1max = 0.91f;
                R2max = 1f;
                break;
            case 2:
                UIScript.pointerBG.sprite = UIScript.gradiants[2];
                R2min = 0f;
                R1min = 0.8f;
                O2min = 0.18f;
                O1min = 0.28f;
                Gmin = 0.38f;
                Gmax = 0.62f;
                O1max = 0.72f;
                O2max = 0.82f;
                R1max = 0.92f;
                R2max = 1f;
                break;
            case 3:
                UIScript.pointerBG.sprite = UIScript.gradiants[3];
                R2min = 0f;
                R1min = 0.7f;
                O2min = 0.17f;
                O1min = 0.27f;
                Gmin = 0.37f;
                Gmax = 0.63f;
                O1max = 0.73f;
                O2max = 0.83f;
                R1max = 0.93f;
                R2max = 1f;
                break;
            case 4:
                UIScript.pointerBG.sprite = UIScript.gradiants[4];
                R2min = 0f;
                R1min = 0.6f;
                O2min = 0.16f;
                O1min = 0.26f;
                Gmin = 0.36f;
                Gmax = 0.64f;
                O1max = 0.74f;
                O2max = 0.84f;
                R1max = 0.94f;
                R2max = 1f;
                break;
            case 5:
                UIScript.pointerBG.sprite = UIScript.gradiants[5];
                R2min = 0f;
                R1min = 0.5f;
                O2min = 0.15f;
                O1min = 0.25f;
                Gmin = 0.35f;
                Gmax = 0.65f;
                O1max = 0.75f;
                O2max = 0.85f;
                R1max = 0.95f;
                R2max = 1f;
                break;
        }
    }

    //Restarts the game by setting all values back to original values
    public void restartGame()
    {

        gameStarted = false;

        healthPoints = 100;
        score = 0;
        currentGreenHits = 0;
        currentOrangeHits = 0;
        currentRedHits = 0;
        currentIceBroken = 0;
        currentStagesSkipped = 0;
        extraShards = 0;
        currentShardsGained = 0;

        //Calls this to reset UI back to original values
        UIScript.uiUpdate();

        //Hides "New High Score" text
        UIScript.newHighScoreText.SetActive(false);
        //Hides Game Over Pop Up
        UIScript.goPanel.SetActive(false);
        //Hide frozen shard
        UIScript.frozenShard.SetActive(false);

        UIScript.GOADPanel.SetActive(false);
        UIScript.GOBackgroundPanel.GetComponent<Image>().sprite = UIScript.basePanel;

        //Enable ad button to be ready for a future game
        UIScript.GOADButton.interactable = true;
    }

    //Game Over
    public void gameOver()
    {
        //Set 2x shards ad chance
        ADChance = 50; //option will show 50% of the time

        //ScoreManager
        shardUpdate(score); //Updates Shard value
        highscoreUpdate(score); //Updates HighScore value
        gamesPlayed(); //Updates games played

        //Save Player Stats
        SaveData.GreenHits += currentGreenHits;
        SaveData.OrangeHits += currentOrangeHits;
        SaveData.RedHits += currentRedHits;
        SaveData.IceBroken += currentIceBroken;
        SaveData.StagesSkipped += currentStagesSkipped;
        SaveData.ForzenShardsFound += currentFrozenShards;
        SaveScript.SaveData();

        //Prepares GameOver Pop Up with values
        UIScript.highscoreText.GetComponent<Text>().text = "HighScore: " + SaveData.HighScore;
        UIScript.goPanel.SetActive(true);
        UIScript.scorethisgameText.text = "Score this game: " + score;

        //AD manager: 2x shards
        int i = Random.Range(0,100);
        if (i <= ADChance)
        {
            //Show pannel
            UIScript.GOADPanel.SetActive(true);
            UIScript.GOBackgroundPanel.GetComponent<Image>().sprite = UIScript.bottomPanel;
        }

        //Sends analytics of score gained per game
        AnalyticsResult analyticsResult = Analytics.CustomEvent(
            "FinalScore",
            new Dictionary<string, object>
            {
                { "Score", score }
            }
        );
    }

    //Updates total Shards
    public void shardUpdate(int endScore)
    {
        currentShardsGained = (int)Mathf.Round((endScore / 10)) + (int)Mathf.Floor(extraShards);
        SaveData.Shards += (int)currentShardsGained;
        SaveScript.SaveData();
    }

    //Update highscore
    public void highscoreUpdate(int endScore)
    {
        int highScore = SaveData.HighScore;
        //Check if new score is higher than the previous High Score
        if (endScore > highScore)
        {
            SaveData.HighScore = endScore;
            SaveScript.SaveData();

            UIScript.highscoreText.GetComponent<Text>().text = "HighScore: " + endScore;
            UIScript.newHighScoreText.SetActive(true);

            ADChance = 80;
        }
    }

    //Counts total games played
    public void gamesPlayed()
    {
        SaveData.GamesPlayed += 1;
        SaveScript.SaveData();
        UIScript.gamesPlayedText.text = "Shards Gained: " + currentShardsGained;
    }

    //Card pull logic
    public void cardPull()
    {
        //Check if player owns enough shards
        if (SaveData.Shards >= 25 && SaveData.CardsOwnedHP>31 && SaveData.CardsOwnedSpd > 31 && SaveData.CardsOwnedGreen > 31 && SaveData.CardsOwnedLifesteal > 31 && SaveData.CardsOwnedSkip > 31 && SaveData.CardsOwnedFrozenChance > 31 && SaveData.CardsOwnedFrozenAmount > 31)
        {
            //Reduce and save shards
            SaveData.Shards -= 25;
            SaveScript.SaveData();

            //pull Card and save
            pullLogic();
            SaveScript.SaveData();

            //Update UI and Shop
            UIScript.uiUpdate();
            UIScript.shopUpdate();
        }
    }

    public void pullLogic()
    {
        int cardID;

        //Randomly Selects a card
        cardID = Random.Range(0, 7);

        switch (cardID)
        {
            case 0:
                if (SaveData.CardsOwnedHP == 31)
                {
                    //If card is maxed pull again
                    pullLogic();
                }else
                {
                    //Add to cards owned and show pop up for that card
                    SaveData.CardsOwnedHP += 1;

                    UIScript.cardPopUp(cardID);
                }

                //Check amount of cards owned to update lvl if needed
                if (SaveData.CardsOwnedHP >= 31)
                {
                    SaveData.UnlockLvlHP = 5;
                }
                else if (SaveData.CardsOwnedHP >= 15)
                {
                    SaveData.UnlockLvlHP = 4;
                }
                else if (SaveData.CardsOwnedHP >= 7)
                {
                    SaveData.UnlockLvlHP = 3;
                }
                else if (SaveData.CardsOwnedHP >= 3)
                {
                    SaveData.UnlockLvlHP = 2;
                }
                else if (SaveData.CardsOwnedHP >= 1)
                {
                    SaveData.UnlockLvlHP = 1;
                }
                break;
            case 1:
                if (SaveData.CardsOwnedSpd == 31)
                {
                    pullLogic();
                }
                else
                {
                    SaveData.CardsOwnedSpd += 1;

                    UIScript.cardPopUp(cardID);
                }

                if (SaveData.CardsOwnedSpd >= 31)
                {
                    SaveData.UnlockLvlSpd = 5;
                }
                else if (SaveData.CardsOwnedSpd >= 15)
                {
                    SaveData.UnlockLvlSpd = 4;
                }
                else if (SaveData.CardsOwnedSpd >= 7)
                {
                    SaveData.UnlockLvlSpd = 3;
                }
                else if (SaveData.CardsOwnedSpd >= 3)
                {
                    SaveData.UnlockLvlSpd = 2;
                }
                else if (SaveData.CardsOwnedSpd >= 1)
                {
                    SaveData.UnlockLvlSpd = 1;
                }
                break;
            case 2:
                if (SaveData.CardsOwnedGreen == 31)
                {
                    pullLogic();
                }
                else
                {
                    SaveData.CardsOwnedGreen += 1;

                    UIScript.cardPopUp(cardID);
                }

                if (SaveData.CardsOwnedGreen >= 31)
                {
                    SaveData.UnlockLvlGreen = 5;
                }
                else if (SaveData.CardsOwnedGreen >= 15)
                {
                    SaveData.UnlockLvlGreen = 4;
                }
                else if (SaveData.CardsOwnedGreen >= 7)
                {
                    SaveData.UnlockLvlGreen = 3;
                }
                else if (SaveData.CardsOwnedGreen >= 3)
                {
                    SaveData.UnlockLvlGreen = 2;
                }
                else if (SaveData.CardsOwnedGreen >= 1)
                {
                    SaveData.UnlockLvlGreen = 1;
                }
                break;
            case 3:
                if(SaveData.CardsOwnedLifesteal == 31)
                {
                    pullLogic();
                }
                else
                {
                    SaveData.CardsOwnedLifesteal += 1;

                    UIScript.cardPopUp(cardID);
                }

                if (SaveData.CardsOwnedLifesteal >= 31)
                {
                    SaveData.UnlockLvlLifeSteal = 5;
                }
                else if (SaveData.CardsOwnedLifesteal >= 15)
                {
                    SaveData.UnlockLvlLifeSteal = 4;
                }
                else if (SaveData.CardsOwnedLifesteal >= 7)
                {
                    SaveData.UnlockLvlLifeSteal = 3;
                }
                else if (SaveData.CardsOwnedLifesteal >= 3)
                {
                    SaveData.UnlockLvlLifeSteal = 2;
                }
                else if (SaveData.CardsOwnedLifesteal >= 1)
                {
                    SaveData.UnlockLvlLifeSteal = 1;
                }
                break;
            case 4:
                if (SaveData.CardsOwnedSkip == 31)
                {
                    pullLogic();
                }
                else
                {
                    SaveData.CardsOwnedSkip += 1;

                    UIScript.cardPopUp(cardID);
                }

                if (SaveData.CardsOwnedSkip >= 31)
                {
                    SaveData.UnlockLvlSkip = 5;
                }
                else if (SaveData.CardsOwnedSkip >= 15)
                {
                    SaveData.UnlockLvlSkip = 4;
                }
                else if (SaveData.CardsOwnedSkip >= 7)
                {
                    SaveData.UnlockLvlSkip = 3;
                }
                else if (SaveData.CardsOwnedSkip >= 3)
                {
                    SaveData.UnlockLvlSkip = 2;
                }
                else if (SaveData.CardsOwnedSkip >= 1)
                {
                    SaveData.UnlockLvlSkip = 1;
                }
                break;
            case 5:
                if (SaveData.CardsOwnedFrozenChance == 31)
                {
                    pullLogic();
                }
                else
                {
                    SaveData.CardsOwnedFrozenChance += 1;

                    UIScript.cardPopUp(cardID);
                }

                if (SaveData.CardsOwnedFrozenChance >= 31)
                {
                    SaveData.UnlockLvlFrozenChance = 5;
                }
                else if (SaveData.CardsOwnedFrozenChance >= 15)
                {
                    SaveData.UnlockLvlFrozenChance = 4;
                }
                else if (SaveData.CardsOwnedFrozenChance >= 7)
                {
                    SaveData.UnlockLvlFrozenChance = 3;
                }
                else if (SaveData.CardsOwnedFrozenChance >= 3)
                {
                    SaveData.UnlockLvlFrozenChance = 2;
                }
                else if (SaveData.CardsOwnedFrozenChance >= 1)
                {
                    SaveData.UnlockLvlFrozenChance = 1;
                }
                break;
            case 6:
                if (SaveData.CardsOwnedFrozenAmount == 31)
                {
                    pullLogic();
                }
                else
                {
                    SaveData.CardsOwnedFrozenAmount += 1;

                    UIScript.cardPopUp(cardID);
                }

                if (SaveData.CardsOwnedFrozenAmount >= 31)
                {
                    SaveData.UnlockLvlFrozenAmount = 5;
                }
                else if (SaveData.CardsOwnedFrozenAmount >= 15)
                {
                    SaveData.UnlockLvlFrozenAmount = 4;
                }
                else if (SaveData.CardsOwnedFrozenAmount >= 7)
                {
                    SaveData.UnlockLvlFrozenAmount = 3;
                }
                else if (SaveData.CardsOwnedFrozenAmount >= 3)
                {
                    SaveData.UnlockLvlFrozenAmount = 2;
                }
                else if (SaveData.CardsOwnedFrozenAmount >= 1)
                {
                    SaveData.UnlockLvlFrozenAmount = 1;
                }
                break;
        }
    }

    public void iceBreak()
    {
        Instantiate(shatter[Random.Range(0, shatter.Length)]);
    }

    public void doubleShards()
    {
        //save the shards again, effectively doubling them
        SaveData.Shards += (int)currentShardsGained;
        SaveScript.SaveData();


    }
}