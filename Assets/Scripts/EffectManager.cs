using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    Game game;

    public delegate void CardEffectDelegate();

    // public for debug purpose
    public Dictionary<EffectType, Dictionary<CardEffect, CardEffectDelegate>> effectTypeRegistry = new();
    List<CardEffect> activeEffects = new List<CardEffect>();

    private void Start()
    {

    }

    internal void AddEffectToRegistery()
    {
        Dictionary<CardEffect, CardEffectDelegate> powerDict = new()
        {
            { CardEffect.AddOneToPowerValue, AddPowerValueOne },
            { CardEffect.AddThreeToPowerValue, AddPowerValueThree },
            { CardEffect.AddFiveToPowerValue, AddPowerValueFive },
            { CardEffect.AddTenToPowerValue, AddPowerValueTen },
        };
    }

    public void ApplyCardEffects()
    {
        foreach (CardEffect effect in activeEffects)
        {
            foreach (EffectType effectType in effectTypeRegistry.Keys)
            {
                if (effectTypeRegistry[effectType].ContainsKey(effect))
                {
                    effectTypeRegistry[effectType][effect].Invoke();
                    break;
                }
                else
                {
                    Debug.LogWarning($"Effect {effect} is not registered in the effectRegistry.");
                }
            }
        }
    }

    internal void AddEffectToList(CardEffect cardEffect)
    {
        activeEffects.Add(cardEffect);
    }    
    
    internal void RemoveEffectFromList(CardEffect cardEffect)
    {
        activeEffects.Remove(cardEffect);
    }

    void ClearEffectList()
    {
        activeEffects.Clear();
    }


    #region CardEffectFunctions

    void AddPowerValueOne() { AddPowerValue(1); }

    void AddPowerValueThree() { AddPowerValue(3); }

    void AddPowerValueFive() { AddPowerValue(5); }

    void AddPowerValueTen() { AddPowerValue(10); }

    void AddPowerValue(int valueToAdd)
    {
        game.BonusPowerValue = valueToAdd;
    }

    #endregion
}


public enum CardEffect
{
    AddOneToPowerValue,
    AddThreeToPowerValue,
    AddFiveToPowerValue,
    AddTenToPowerValue,
}

public enum EffectType
{
    Power,
    Dig,
    Pass,
    Attack,
    Block,
}