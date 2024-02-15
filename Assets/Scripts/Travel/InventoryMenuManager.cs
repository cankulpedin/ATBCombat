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
                weapon.label = $"test {i}";
                Armor armor = new Armor();
                armor.label = $"test {i}";
                Consumable consumable = new Consumable();
                consumable.label = $"test {i}";

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
            AddItemToMenu(weapon);
        }

        foreach (Armor armor in armors)
        {
            AddItemToMenu(armor);
        }

        foreach (Consumable consumable in consumables)
        {
            AddItemToMenu(consumable);
        }
    }

    private void AddItemToMenu(Weapon weapon)
    {
        GameObject inst = Instantiate(menuItemObject);
        inst.transform.SetParent(menuObjects[0].transform);
        inst.transform.GetChild(1).GetComponent<TMP_Text>().text = weapon.label;

        inst.gameObject.GetComponent<ItemButtonHelper>().weapon = weapon;
    }

    private void AddItemToMenu(Armor armor)
    {
        GameObject inst = Instantiate(menuItemObject);
        inst.transform.SetParent(menuObjects[1].transform);
        inst.transform.GetChild(1).GetComponent<TMP_Text>().text = armor.label;

        inst.gameObject.GetComponent<ItemButtonHelper>().armor = armor;

    }

    private void AddItemToMenu(Consumable consumable)
    {
        GameObject inst = Instantiate(menuItemObject);
        inst.transform.SetParent(menuObjects[2].transform);
        inst.transform.GetChild(1).GetComponent<TMP_Text>().text = consumable.label;

        inst.gameObject.GetComponent<ItemButtonHelper>().consumable = consumable;

    }

    private void Update()
    {
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
        Transform itemButton = prevHoveredMenu.transform.GetChild(currentIndex);
        
        Image prevHoveredItem = itemButton.GetComponent<Image>();
        prevHoveredItem.color = transparent;
    }

    private int FindNextFilledMenuType(bool decrease)
    {
        int checkingIndex = decrease ?  currentMenu + 3 - 1 : currentMenu + 1; // next index
        int normalizedIndex = Mathf.Abs(checkingIndex % 3);
        while (normalizedIndex != currentMenu)
        {
            if(menuObjects[normalizedIndex].transform.childCount > 0)
            {
                return normalizedIndex;
            }
            if (decrease) normalizedIndex = Mathf.Abs((checkingIndex + 3 - 1) % 3);
            else normalizedIndex = Mathf.Abs((checkingIndex + 1) % 3);
        }
        return currentMenu;
    }

    private void NavigateOnMainMenu()
    {

        // TODO Add shift+arrow down/up jump 10 item
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            int currentMenuChildCount = menuObjects[currentMenu].transform.childCount;

            if (currentIndex == currentMenuChildCount - 1)
            {
                return;
            }

            SetTransparent();
            if ( currentIndex != 9)
            {
                currentIndex++; // TODO scroll to next 10
            }
            SetSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SetTransparent();
            currentMenu = FindNextFilledMenuType(false);
            SetSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SetTransparent();
            currentMenu = FindNextFilledMenuType(true);
            SetSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SetTransparent();
            if (currentIndex != 0)
            {
                currentIndex--;
            }
            SetSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {

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

    public void TakeItem(Weapon weapon)
    {
        weapons.Add(weapon);
        AddItemToMenu(weapon);
    }

    public void TakeItem(Armor armor)
    {
        armors.Add(armor);
        AddItemToMenu(armor);
    }

    public void TakeItem(Consumable consumable)
    {
        consumables.Add(consumable);
        AddItemToMenu(consumable);
    }
}
