using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text manaCostText;
    public Text description;
    public Image artwork;
    public float speed;
    public Vector3 targetPos;
    public Vector3 startingPos;
    public GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        nameText.text = card.cardName;
        manaCostText.text = card.cost.ToString();
        description.text = card.description;
        artwork.sprite = card.artwork;
        startingPos = GameObject.Find("StartingPoint").GetComponent<Transform>().position;
        targetPos = GameObject.Find("EndingPoint").GetComponent<Transform>().position;
    }

    void Update()
    {
        if (!gameManager.gameHasEnded)
        {
            if (this.gameObject.GetComponent<CardPlay>().stolen == false)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

                if (Vector3.Distance(transform.position, targetPos) < 0.001f)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
