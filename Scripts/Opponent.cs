using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Opponent", menuName = "Opponent")]
public class Opponent : ScriptableObject
{
    public int opponentID;
    public string opponentName;
    public Sprite opponentArtwork;
    public float health;
    public float attack;
    public string passive;
    public float attackSpeed;
}
