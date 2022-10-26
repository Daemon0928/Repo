using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public int id;
    public string cardName;
    public string description;
    public int cost;
    public string effect;
    public string rarity;
    public Sprite artwork;
}
