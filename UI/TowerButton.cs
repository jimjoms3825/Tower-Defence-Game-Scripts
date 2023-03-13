using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private string towerName = "";
    [SerializeField] private int towerCost = 50;
    [SerializeField] private string towerDescription = "";
    [SerializeField] private string flavourText = "";

    [SerializeField] private Image image;

    private Sprite sprite;


    private void Awake()
    {
        sprite = Resources.Load<Sprite>("Art/Towers/" + towerName);
        image.sprite = sprite;
    }

    public void SelectTower()
    {
        if (GameManager.GetInstance().GetCoins() < towerCost)
        {
            //Play a noise
            return;
        }
        SelectedTower newTower = new GameObject().AddComponent<SelectedTower>();
        newTower.Initialize(towerName);
        newTower.towerPlacedEvent.AddListener(OnTowerPlaced);
    }

    private void LateUpdate()
    {
        GetComponent<Button>().interactable = GameManager.GetInstance().GetCoins() >= towerCost;
    }

    private void OnTowerPlaced()
    {
        GameManager.GetInstance().ChargeCoins(towerCost);
    }
}
