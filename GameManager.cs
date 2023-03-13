using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //Public Variables.
    public enum DifficultySettings { Easy, Medium, Hard, OhNo }
    public enum LevelType { CryoLab, Crucible, ProcPlant }

    //Private Variables.
    private DifficultySettings difficulty = DifficultySettings.Medium;
    private LevelType levelType = LevelType.Crucible;

    private int coins = 200;
    private int health = 100;

    private Level currentLevel;

    //Singleton Pattern.
    protected static GameManager instance;

    /*
     * Public Interface.
     */
    public void DealDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, 100);
        if (health == 0) EndGame();
    }

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
    }

    public void ChargeCoins(int coinsToCharge)
    {
        if (coins < coinsToCharge)
            Debug.LogWarning("Charged more coins than were available");
        if (coins < 0)
            Debug.LogWarning("Charged a negative number of coins");
        coins -= coinsToCharge;
    }

    public void StartGame()
    {

    }

    public void EndGame()
    {
        if (SceneManager.sceneCount == 1)
            SceneManager.LoadScene("Menu");
    }
    public void transitionToLevel(LevelType level)
    {
        coins = 200;
        health = 100;
        if(SceneManager.sceneCount == 1)
            SceneManager.LoadScene("Level");
    }

    /*
     * Private Interface.
     */

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Attempted creating a second " + instance.name + "." +
                " Destroying second copy of element.");
            Destroy(gameObject);
        }
        instance = this;
        name = "GameManager";
        DontDestroyOnLoad(gameObject);
    }


    /*
     * Getters and Setters
     */

    public DifficultySettings getDifficultySetting()
    {
        return difficulty;
    }

    public void SetDificultySetting(DifficultySettings newDif) {
        difficulty = newDif;
    }

    public LevelType GetlevelType()
    {
        return levelType;
    }

    public void SetLevelType(LevelType newLevel)
    {
        levelType = newLevel;
        transitionToLevel(levelType);
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetCoins()
    {
        return coins;
    }

    public static GameManager GetInstance()
    {
        if (instance != null) return instance;
        instance = new GameObject().AddComponent<GameManager>();
        return instance;
    }

    public void SetCurrentLevel(Level _level)
    {
        currentLevel = _level;
    }

    public Level GetCurrentLevel()
    {
        return currentLevel;
    }

}
