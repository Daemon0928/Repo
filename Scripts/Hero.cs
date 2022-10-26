using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class Hero : ScriptableObject
{
    public int heroID;
    public Sprite heroArtwork;
    public float health;
    public float baseHealth;
    public string passive;
    public float manaRegen;
}
