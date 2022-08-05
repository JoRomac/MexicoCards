using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public GameObject cards;
    public Transform tfDeck;
    public Transform[] tfPlayer, tfOpponent1, tfOpponent2;
    public List<GameObject> deckOfCards = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        InitCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitCard(){
        for(int i = 0; i < SpriteGame.spGame.spCards.Length; ++i){
            GameObject _card = Instantiate(cards, tfDeck.position, Quaternion.identity);
            _card.transform.SetParent(tfDeck, true);
            _card.GetComponent<UICards>().cardImg.sprite = SpriteGame.spGame.spCards[i];
            deckOfCards.Add(_card);
        }
        StartCoroutine(DealCards());
    }
    IEnumerator DealCards() {

        for(int i = 0; i < 10; ++i){
            yield return new WaitForSeconds(0.1f);
            int rndCard = Random.Range(0, deckOfCards.Count);
            
            deckOfCards[rndCard].transform.SetParent(tfPlayer[i], true);
            iTween.MoveTo(deckOfCards[rndCard],
                    iTween.Hash("position", tfPlayer[i].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
		    iTween.RotateBy(deckOfCards[rndCard],
                    iTween.Hash("y", 0.5f, "easeType", "Linear", "loopType", "none", "time", 0.4f));
            yield return new WaitForSeconds(0.15f);
            deckOfCards[rndCard].GetComponent<UICards>().bckImage.SetActive(false);
            deckOfCards.RemoveAt(rndCard);


            yield return new WaitForSeconds(0.1f);
            int rndCardForPy1 = Random.Range(0, deckOfCards.Count);
            deckOfCards[rndCardForPy1].transform.SetParent(tfOpponent1[i], true);
            iTween.MoveTo(deckOfCards[rndCardForPy1],
                    iTween.Hash("position", tfOpponent1[i].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
            deckOfCards.RemoveAt(rndCardForPy1);
            
            yield return new WaitForSeconds(0.1f);
            int rndCardForPy2 = Random.Range(0, deckOfCards.Count);
            deckOfCards[rndCardForPy2].transform.SetParent(tfOpponent2[i], true);
            iTween.MoveTo(deckOfCards[rndCardForPy2],
                    iTween.Hash("position", tfOpponent2[i].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
            deckOfCards.RemoveAt(rndCardForPy2);

        }
    }
}
