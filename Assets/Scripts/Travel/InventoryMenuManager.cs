using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class InventoryMenuManager : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weapons = new List<Weapon>();
    [SerializeField]
    private List<Armor> armors = new List<Armor>();
    [SerializeField]
    private List<Consumable> consumables = new List<Consumable>();

    public Weapon equipedWeapon;
    public Armor equipedArmor;
    public Consumable[] equipedConsumables = new Consumable[6];

    public bool isActive = false;

    public int currentMenu = 0; // weapons, armors, consumables
    public int currentIndex = 0;

    public GameObject[] menuObjects = new GameObject[3];
    public GameObject menuItemObject;

    private Color selectionColor = new Color(67f / 255f, 53f / 255f, 159f / 255f, 160f / 255f);
    private Color transparent = new Color(255f, 255f, 255f, 0f);

    [SerializeField]
    private bool testFill = false;

    private void Awake()
    {
        if(testFill)
        {
            for(int i = 0; i < 10; i++) {
                Weapon weapon = new Weapon();
                weapon.name = $"test {i}";
                Armor armor = new Armor();
                armor.name = $"test {i}";
                Consumable consumable = new Consumable();
                consumable.name = $"test {i}";

                weapons.Add(weapon);
                armors.Add(armor);
                consumables.Add(consumable);
            }
        }
    }

    private void Start()
    {
        foreach(Weapon weapon in weapons)
        {
            GameObject inst = Instantiate(menuItemObject);
            inst.transform.SetParent(menuObjects[0].transform);
            inst.transform.GetChild(1).GetComponent<TMP_Text>().text = weapon.name;
        }

        foreach (Armor armor in armors)
        {
            GameObject inst = Instantiate(menuItemObject);
            inst.transform.SetParent(menuObjects[1].transform);
            inst.transform.GetChild(1).GetComponent<TMP_Text>().text = armor.name;
        }

        foreach (Consumable consumable in consumables)
        {
            GameObject inst = Instantiate(menuItemObject);
            inst.transform.SetParent(menuObjects[2].transform);
            inst.transform.GetChild(1).GetComponent<TMP_Text>().text = consumable.name;
        }
    }

    private void Update()
    {
        Debug.Log(isActive);

        if (isActive)
        {
            SetSelectedItem();
            NavigateOnMainMenu();
        }
    }

    private void SetSelectedItem()
    {
        GameObject hoveredMenu = menuObjects[currentMenu];
        Image hoveredItem = hoveredMenu.transform.GetChild(currentIndex).GetComponent<Image>();
        hoveredItem.color = selectionColor;
    }

    private void SetTransparent()
    {
        GameObject prevHoveredMenu = menuObjects[currentMenu];
        Image prevHoveredItem = prevHoveredMenu.transform.GetChild(currentIndex).GetComponent<Image>();
        prevHoveredItem.color = transparent;
    }

    private void NavigateOnMainMenu()
    {
        // TODO Add shift+arrow down/up jump 10 item
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SetTransparent();
            if (currentIndex != 9)
            {
                currentIndex++; // TODO scroll to next 10
            }
            SetSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SetTransparent();
            if(currentMenu != 2)
            {
                currentMenu++;
            }
            SetSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SetTransparent();
            if (currentMenu != 0)
            { 
                currentMenu--;
            }
            SetSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SetTransparent();
            if(currentIndex != 0)
            {
                currentIndex--;
            }
            SetSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetTransparent();
            isActive = false;
            currentMenu = 0;
            currentIndex = 0;
            SetTransparent();
        }
    }
}
