using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    Game game;

    public delegate void CardEffectDelegate();
    public delegate void ReverseCardEffectDelegate();

    // public for debug purpose
    public Dictionary<EffectType, Dictionary<CardEffect, CardEffectDelegate>> effectTypeRegistery = new();
    public Dictionary<EffectType, Dictionary<CardEffect, ReverseCardEffectDelegate>> effectReverseTypeRegistery = new();
    Dictionary<CardEffect, CardEffectDelegate> powerDict;
    Dictionary<CardEffect, ReverseCardEffectDelegate> reversePowerDict;
    List<CardEffect> activeEffects = new List<CardEffect>();

    private void Start()
    {
        AddEffectToRegistery();
        AddReverseEffectToRegistery();
        AddEffectTypeToRegistery();
        AddReverseEffectTypeToRegistery();
    }

    private void AddReverseEffectTypeToRegistery()
    {
        effectReverseTypeRegistery[EffectType.Power] = reversePowerDict;
    }

    private void AddReverseEffectToRegistery()
    {
        reversePowerDict = new()
        {
            { CardEffect.AddOneToPowerValue, CancelPowerValueOne },
            { CardEffect.AddThreeToPowerValue, CancelPowerValueThree },
            { CardEffect.AddFiveToPowerValue, CancelPowerValueFive },
            { CardEffect.AddTenToPowerValue, CancelPowerValueTen }
        };
    }

    internal void AddEffectToRegistery()
    {
        powerDict = new()
        {
            { CardEffect.AddOneToPowerValue, AddPowerValueOne },
            { CardEffect.AddThreeToPowerValue, AddPowerValueThree },
            { CardEffect.AddFiveToPowerValue, AddPowerValueFive },
            { CardEffect.AddTenToPowerValue, AddPowerValueTen },
        };
    }

    private void AddEffectTypeToRegistery()
    {
        effectTypeRegistery[EffectType.Power] = powerDict;
    }

    public void ApplyCardEffects(CardEffect effect)
    {
        Debug.Log(effect);
        foreach (EffectType effectType in effectTypeRegistery.Keys)
        {
            if (effectTypeRegistery[effectType].ContainsKey(effect))
            {
                effectTypeRegistery[effectType][effect].Invoke();
                break;
            }
            else
            {
                Debug.LogWarning($"Effect {effect} is not registered in the effectRegistry.");
            }
        }
    }

    void UnapplyCardEffects(CardEffect effect)
    {
        foreach (EffectType effectType in effectReverseTypeRegistery.Keys)
        {
            if (effectReverseTypeRegistery[effectType].ContainsKey(effect))
            {
                effectReverseTypeRegistery[effectType][effect].Invoke();
                break;
            }
            else
            {
                Debug.LogWarning($"Effect {effect} is not registered in the effectRegistry.");
            }
        }
    }

    internal void AddEffectToList(CardEffect cardEffect)
    {
        activeEffects.Add(cardEffect);
        ApplyCardEffects(cardEffect);
    }

    internal void RemoveEffectFromList(CardEffect cardEffect)
    {
        activeEffects.Remove(cardEffect);
        UnapplyCardEffects(cardEffect);
    }

    internal void ClearEffectList()
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
        game.BonusPowerValue += valueToAdd;
    }

    void CancelPowerValueOne() { CancelPowerValue(1); }
    void CancelPowerValueThree() { CancelPowerValue(3); }
    void CancelPowerValueFive() { CancelPowerValue(5); }
    void CancelPowerValueTen() { CancelPowerValue(10); }
    void CancelPowerValue(int value)
    {
        game.BonusPowerValue -= value;
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