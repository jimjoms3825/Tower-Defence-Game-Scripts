using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playbutton;

    [SerializeField] private TMP_Dropdown difficulty;
    [SerializeField] private TMP_Dropdown level;

    private void Awake()
    {
        difficulty.value = 1;
        level.value = 1;
    }

    public void Play()
    {
        GameManager.GetInstance().SetDificultySetting((GameManager.DifficultySettings)difficulty.value);
        GameManager.GetInstance().SetLevelType((GameManager.LevelType)level.value);
    }

}
