using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private BattleStateMachine BSM;

    public Button AttackButton;
    public List<Button> MagicButtons = new List<Button>();

    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();

        AttackButton.onClick.AddListener(() => SelectTarget(ActionTypes.Attack));
        foreach (Button magicButton in MagicButtons)
        {
            MagicTypes buttonType = (MagicTypes)Variables.Object(magicButton).Get("magicType");
            magicButton.onClick.AddListener(()=>SelectTarget(ActionTypes.Magic, buttonType));
        }
    }

    private void OnDestroy()
    {
        AttackButton.onClick.RemoveAllListeners();
        foreach (Button magicButton in MagicButtons)
        {
            magicButton.onClick.RemoveAllListeners();
        }
    }

    void SelectTarget(ActionTypes actionType, MagicTypes magicType = MagicTypes.Null)
    {
        BSM.HeroQueue[0].GetComponent<CharacterStateMachine>().SetNewState(ActionTypes.Attack, magicType);
    }
}
