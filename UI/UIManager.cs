using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;

    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Button startWaveButton;

    private SelectedTower selectedTower;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Attempted creating a second " + instance.name + "." +
                " Destroying second copy of element.");
            Destroy(gameObject);
        }
        instance = this;
        name = "UIManager";
    }

    private void LateUpdate()
    {
        coinsText.text = "Coins: " + GameManager.GetInstance().GetCoins().ToString();
        healthSlider.value = GameManager.GetInstance().GetHealth();
        healthText.text = healthSlider.value + " / 100";

        startWaveButton.gameObject.SetActive(!GameManager.GetInstance().GetCurrentLevel().GetInWave());
    }

    public void StartWave()
    {
        GameManager.GetInstance().GetCurrentLevel().StartRound();
    }

    public void SetSelectedTower(SelectedTower newSelectedTower)
    {
        if(selectedTower != null)
            Destroy(selectedTower.gameObject);

        selectedTower = newSelectedTower;
    }

    public static UIManager GetInstance()
    {
        if (instance != null) return instance;
        instance = new GameObject().AddComponent<UIManager>();
        return instance;
    }

}
