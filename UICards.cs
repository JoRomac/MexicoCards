using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICards : MonoBehaviour
{
    public Image cardImg;
    public GameObject bckImage;
    // Start is called before the first frame update
    public int cardValue;
    public string cardSuit;
    void Start()
    {
        string cardValueString = cardImg.sprite.name;
        cardValue = int.Parse(cardValueString.Substring(1));
        string cardSuitName = cardImg.sprite.name;
        cardSuit = cardSuitName.Substring(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
