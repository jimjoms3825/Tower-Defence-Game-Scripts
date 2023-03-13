using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectedTower : MonoBehaviour
{
    private string towerName;
    private Image image;
    public UnityEvent towerPlacedEvent;

    public void Initialize(string _towerName)
    {
        towerPlacedEvent = new UnityEvent();
        transform.parent = UIManager.GetInstance().transform;
        image = gameObject.AddComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Art/Towers/" + _towerName);
        towerName = _towerName;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PlaceTower();
        if(Input.GetMouseButtonDown(1))
            Destroy(this.gameObject);
    }

    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }

    private void PlaceTower()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject.GetComponent<Tile>()
            && hit.collider.gameObject.GetComponent<Tile>().GetTileType() == Tile.TileTypes.Buildable
            && hit.collider.gameObject.GetComponent<Tile>().GetTower() == null)
        {
            towerPlacedEvent.Invoke();

            Tower newTower = Instantiate(Resources.Load<GameObject>("Prefabs/Towers/" + towerName).GetComponent<Tower>());
            newTower.transform.position = hit.collider.transform.position;
            hit.collider.gameObject.GetComponent<Tile>().SetTower(newTower);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No Pllace");
            //play a bad sound or something
        }
    }
}
