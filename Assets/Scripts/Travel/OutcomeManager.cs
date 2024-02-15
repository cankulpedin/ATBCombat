using System;
using System.Collections.Generic;
using UnityEngine;

public class OutcomeManager : MonoBehaviour
{
    public static OutcomeManager instance;

    private InventoryMenuManager inventoryMenuManager;

    public Dictionary<Outcome, bool> worldEvents = new Dictionary<Outcome, bool>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Outcome outcome in Enum.GetValues(typeof(Outcome)))
        {
            worldEvents[outcome] = false;
        }

        inventoryMenuManager = GameObject.Find("MenuManager").GetComponent<InventoryMenuManager>();
    }

    private void HandleOutcome(Outcome outcome, bool status)
    {
        if (status == true)
        {
            switch (outcome)
            {
                case Outcome.Get_Item_BroadSword:
                    Weapon broadSword = Resources.Load<Weapon>("Items/BroadSword");
                    inventoryMenuManager.TakeItem(broadSword);
                    break;

            }
        }
    }

    public void SetWorldEvent(Outcome outcome, bool status)
    {
        if (outcome != Outcome.NULL)
        {
            worldEvents[outcome] = status;
            HandleOutcome(outcome, status);
        }
    }
}
