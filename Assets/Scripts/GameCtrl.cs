using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameCtrl : MonoBehaviour, IPointerClickHandler
{
    public Text text, text2, trumpS;
    public int bidValue, myBid, indexPerson, indexCom1, indexCom2;
    public int personScore, com1Score, com2Score = 0;
    private int dropZoneIndex;
    private string trumpSuit;
    public string bidWinner;
    public GameObject cards;
    public Transform tfDeck;
    public Transform[] tfPlayer, tfOpponent1, tfOpponent2, dropZone, twoCardDrop;
    public List<GameObject> deckOfCards = new List<GameObject>();
    public List<GameObject> handOfPerson = new List<GameObject>();
    public List<GameObject> handOfPl1 = new List<GameObject>();
    public List<GameObject> handOfPl2 = new List<GameObject>();
    public List<GameObject> trickZone = new List<GameObject>();
    Dictionary<string, int> numOfSuitsPl1 = new Dictionary<string, int>(){{"c", 0}, {"h", 0}, {"s", 0}, {"d", 0}};
    Dictionary<string, int> numOfSuitsPl2 = new Dictionary<string, int>(){{"c", 0}, {"h", 0}, {"s", 0}, {"d", 0}};
    public GameObject clickedCard;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void getMyBid(int val)
    {

        if (val == 1){
            myBid = 5;
        }
        else if (val == 2){
            myBid = 6;
        }
        else if (val == 3){
            myBid = 7;
        }
        else if (val == 4){
            myBid = 8;
        }
        else if (val == 5){
            myBid = 9;
        }
        else if (val == 6){
            myBid = 10;
        }
        else if (val == 7){
            myBid = 0;
        }
        FirstToStart();
    }

    public void GetTrumpSuit(int val){
        if(val == 1){
            trumpSuit = "Club";
        }
        else if(val == 2){
            trumpSuit = "Spade";
        }
        else if(val == 3){
            trumpSuit = "Diamond";
        }
        else if(val == 4){
            trumpSuit = "Heart";
        }
        trumpS.text = trumpSuit;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (trickZone.Count == 3){
            ComputeTrick();
        }
    }
    public void RemoveTwoCards(List<GameObject> hand, Dictionary<string,int> leastSuit){
        string s = "";
        int twoCards = 0;
        //sortiranje dict-a 
        foreach(KeyValuePair<string, int> kv in leastSuit.OrderBy(kv => kv.Value)){
                                }

        foreach (KeyValuePair<string,int> kv in leastSuit){
            if (kv.Value == 0){
                continue;
            }
            else if(kv.Value == 1){
                s = kv.Key;
                for (int i = 0;i < hand.Count;++i){
                    if (hand[i].GetComponent<UICards>().cardSuit.Equals(s)){
                        if (twoCards < 2){
                            Destroy(hand[i]);
                            hand.RemoveAt(i);
                            ++twoCards;
                        }
                    }
                }
            }
            else if (kv.Value >= 2){
                s = kv.Key;
                for (int i = 0;i < hand.Count;++i){
                    if (hand[i].GetComponent<UICards>().cardSuit.Equals(s)){
                        if(twoCards < 2){
                            Destroy(hand[i]);
                            hand.RemoveAt(i);
                            ++twoCards;
                        }
                    }
                }
            }
        }
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
            handOfPerson.Add(deckOfCards[rndCard]);
            deckOfCards.RemoveAt(rndCard);

            yield return new WaitForSeconds(0.1f);
            int rndCardForPy1 = Random.Range(0, deckOfCards.Count);
            deckOfCards[rndCardForPy1].transform.SetParent(tfOpponent1[i], true);
            iTween.MoveTo(deckOfCards[rndCardForPy1],
                    iTween.Hash("position", tfOpponent1[i].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
            handOfPl1.Add(deckOfCards[rndCardForPy1]);
            deckOfCards.RemoveAt(rndCardForPy1);
            
            yield return new WaitForSeconds(0.1f);
            int rndCardForPy2 = Random.Range(0, deckOfCards.Count);
            deckOfCards[rndCardForPy2].transform.SetParent(tfOpponent2[i], true);
            iTween.MoveTo(deckOfCards[rndCardForPy2],
                    iTween.Hash("position", tfOpponent2[i].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
            handOfPl2.Add(deckOfCards[rndCardForPy2]);        
            deckOfCards.RemoveAt(rndCardForPy2);

        }
    }
    IEnumerator DealTalon(int pl)
    {
        for(int i = 0; i < 2; ++i){
            yield return new WaitForSeconds(0.1f);
            if (pl == 1)//dvije karte za bi pobjednika
            {
                deckOfCards[i].transform.SetParent(tfPlayer[i+10], true);
                iTween.MoveTo(deckOfCards[i],
                    iTween.Hash("position", tfPlayer[i+10].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
		        iTween.RotateBy(deckOfCards[i],
                    iTween.Hash("y", 0.5f, "easeType", "Linear", "loopType", "none", "time", 0.4f));
                    yield return new WaitForSeconds(0.15f);
            deckOfCards[i].GetComponent<UICards>().bckImage.SetActive(false);
            handOfPerson.Add(deckOfCards[i]);
            }
            else if(pl == 2)
            {
                deckOfCards[i].transform.SetParent(tfOpponent1[i+10], true);
            iTween.MoveTo(deckOfCards[i],
                    iTween.Hash("position", tfOpponent1[i+10].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
                    yield return new WaitForSeconds(0.15f); 
            handOfPl1.Add(deckOfCards[i]);    
            }
            else
            {
                deckOfCards[i].transform.SetParent(tfOpponent2[i+10], true);
            iTween.MoveTo(deckOfCards[i],
                    iTween.Hash("position", tfOpponent2[i+10].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
                    yield return new WaitForSeconds(0.15f);
            handOfPl2.Add(deckOfCards[i]);
            }
        }
        deckOfCards.Clear();
    }
    private string StartBid()
    {
        int res1 = BiddingRes(handOfPl1, numOfSuitsPl1);
        int res2 = BiddingRes(handOfPl2, numOfSuitsPl2);
        if(myBid >= res1 && myBid >= res2){
            res1 = 0;
            res2 = 0;
        }
        text.text = res1.ToString();
        if (res1 != 0 && res2 != 0){
            if (res2 <= res1){
                res2 = 0;
            }
        }
        text2.text = res2.ToString();
        if (myBid == 0 && res1 == 0 && res2 == 0){
            SceneManager.LoadScene("Game");
        }
        if (myBid > res1 && myBid > res2){
            StartCoroutine(DealTalon(1));
            return "person";
        }
        else if(res1 > myBid && res1 > res2){
            StartCoroutine(DealTalon(2));
            return "com1";
        }
        else{
            StartCoroutine(DealTalon(3));
            return "com2";
        }
    }

        
    public int BiddingRes(List<GameObject> hand, Dictionary<string,int> numOfSuits){
        int countClubs =0, countDiam = 0, countSpade = 0, countHeart = 0;
        for (int i = 0; i <hand.Count;++i){
                if (hand[i].GetComponent<UICards>().cardSuit.Equals("c")){
                    ++countClubs;
                    numOfSuits["c"]++;
                    if (countClubs >= 5){
                        bidValue = countClubs;
                        trumpSuit = "Club";
                        break;
                    }
                }
                else if (hand[i].GetComponent<UICards>().cardSuit.Equals("h")){
                    ++countHeart;
                    numOfSuits["h"]++;
                    if (countHeart >= 5){
                        bidValue = countHeart;
                        trumpSuit = "Heart";
                        break;
                    }
                }
                else if (hand[i].GetComponent<UICards>().cardSuit.Equals("s")){
                    ++countDiam;
                    numOfSuits["s"]++;
                    if (countDiam >= 5){
                        bidValue = countDiam;
                        trumpSuit = "Spade";
                        break;
                    }
                }
                else if (hand[i].GetComponent<UICards>().cardSuit.Equals("d")){
                    ++countSpade;
                    numOfSuits["d"]++;
                    if (countSpade >= 5){
                        bidValue = countSpade;
                        trumpSuit = "Diamond";
                        break;
                    }
                }
        
        }
        if (countClubs < 5 && countDiam < 5 && countHeart < 5 && countSpade < 5){
            bidValue = 0;
        }
        return bidValue;
    }
    
    public void FirstToStart(){

        bidWinner = StartBid();
        if (bidWinner.Equals("person")){
            indexPerson = 0;
            indexCom1 = 1;
            indexCom2 = 2;
        }
        if (bidWinner.Equals("com2")){
            RemoveTwoCards(handOfPl2, numOfSuitsPl2);
            indexPerson = 1;
            indexCom1 = 2;
            indexCom2 = 0;
            PlayTrickC2();
        }
        else if(bidWinner.Equals("com1"))
        {
            RemoveTwoCards(handOfPl1, numOfSuitsPl1);
            indexPerson = 2;
            indexCom1 = 0;
            indexCom2 = 1;
            PlayTrickC1();
        }

    }

    public void FirstToContinue(){

    }

    public void ComputeTrick(){
        int winCard;
        int numOfTrumps = 0;
        int indexOfWinCard = 0;
        var curTrumpSuit = trumpSuit.ToLower().Substring(0,1);
        List<int> trumpIndex = new List<int>();
        List<int> values = new List<int>();
        List<string> suits = new List<string>();
        // Iz drop zone po indexima razdvoji boju i vrijednost pojedine karte
        for (int i = 0; i < trickZone.Count;++i){
            values.Add(trickZone[i].GetComponent<UICards>().cardValue);
            suits.Add(trickZone[i].GetComponent<UICards>().cardSuit);
        }
        //za svaku kartu u drop zoni prebroj koliko je aduta i dodaj u listu "trumpIndex" index na kojem se nalazi pojedini adut
        for (int i = 0; i < 3;++i){
            var s = trickZone[i].GetComponent<UICards>().cardSuit;
            var v = trickZone[i].GetComponent<UICards>().cardValue;
            if (s.Equals(curTrumpSuit)){
                trumpIndex.Add(i);
               ++numOfTrumps;
            }
        }
        //ako nema aduta u odigranoj ruci provjeri najaču kartu s obzirom na prvu koja je odigrana
        if (numOfTrumps == 0){
            var domSuit = suits[0];
            winCard = values[0];
            for (int i = 1; i < trickZone.Count; ++i){
                var s = suits[i];
                if (domSuit.Equals(s)){
                    winCard = Mathf.Max(winCard, values[i]);
                }
            }
        }
        //ako je broj aduta 3 u drop zoni uzmi najvecu vrijednost
        else if (numOfTrumps == 3){
            winCard = Mathf.Max(values.ToArray());
        }
        //ako je broj aduta 2 u drop zoni dohvati na kojim indexima se nalaze i spremi vecu vrijednost
        else if (numOfTrumps == 2){
            var c1Val = values[trumpIndex[0]];
            var c2Val = values[trumpIndex[1]];
            winCard = Mathf.Max(c1Val, c2Val);
        }
        //ako je jedan adut u drop zoni ta kart nosi
        else {
            winCard = values[trumpIndex[0]];
        }
        Debug.Log(winCard);
        for (int i = 0; i <trickZone.Count;++i){
            if (winCard == values[i]){
                indexOfWinCard = i;
            }
        }
        if(indexOfWinCard == indexPerson){
            ++personScore;
        }
        else if(indexOfWinCard == indexCom1){
            ++com1Score;
            indexCom1 = 0;
            indexCom2 = 1;
            indexPerson = 2;
            PlayTrickC1();

        }
        else{
            ++com2Score;
            indexCom2 = 0;
            indexPerson = 1;
            indexCom2 = 2;
            PlayTrickC2();
        }
        Debug.Log(personScore);
        Debug.Log(com1Score);
        Debug.Log(com2Score);
    }
    public void PlayTrickC1(){
        int cardToPlay = -1;
        if (indexCom1 == 0){
                cardToPlay = Random.Range(0, handOfPl1.Count);
        }
        if (indexCom1 != 0){
            for (int i = 0; i<handOfPl1.Count;++i){
                // ako je ista boja kao u prvoj bacenoj karti
                if (handOfPl1[i].GetComponent<UICards>().cardSuit.Equals(trickZone[0].GetComponent<UICards>().cardSuit)){
                    cardToPlay = i;
                }
            }
            if (cardToPlay < 0){
                for (int j = 0; j<handOfPl1.Count;++j){
                // ako je ista boja kao u prvoj bacenoj karti
                    if (handOfPl1[j].GetComponent<UICards>().cardSuit.Equals(trumpSuit)){
                        cardToPlay = j;
                    }
                }
            if (cardToPlay < 0){
                cardToPlay = Random.Range(0, handOfPl1.Count);
                }
            }
        }
        
            handOfPl1[cardToPlay].transform.SetParent(dropZone[indexCom1], true);
        iTween.MoveTo(handOfPl1[cardToPlay],
            iTween.Hash("position", dropZone[indexCom1].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
        iTween.RotateBy(handOfPl1[cardToPlay], iTween.Hash("y", 0.5f, "easeType", "Linear", "loopType", "none", "time", 0.4f));
        handOfPl1[cardToPlay].GetComponent<UICards>().bckImage.SetActive(false);
        trickZone.Add(handOfPl1[cardToPlay]);
        handOfPl1.RemoveAt(cardToPlay);
        new WaitForSeconds(4f);
        PlayTrickC2();
    }
    public void PlayTrickC2(){
        int cardToPlay = -1;
        if (indexCom2 == 0){
                cardToPlay = Random.Range(0, handOfPl2.Count);
        }
        if (indexCom2 != 0){
            for (int i = 0; i<handOfPl2.Count;++i){
                // ako je ista boja kao u prvoj bacenoj karti
                if (handOfPl2[i].GetComponent<UICards>().cardSuit.Equals(trickZone[0].GetComponent<UICards>().cardSuit)){
                    cardToPlay = i;
                }
            }
            if (cardToPlay < 0){
                for (int j = 0; j<handOfPl2.Count;++j){
                // ako je ista boja kao u prvoj bacenoj karti
                    if (handOfPl2[j].GetComponent<UICards>().cardSuit.Equals(trumpSuit)){
                        cardToPlay = j;
                    }
                }
            if (cardToPlay < 0){
                cardToPlay = Random.Range(0, handOfPl2.Count);
                }
            }
        }
        handOfPl2[cardToPlay].transform.SetParent(dropZone[indexCom2], true);
        iTween.MoveTo(handOfPl2[cardToPlay],
                iTween.Hash("position", dropZone[indexCom2].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
		iTween.RotateBy(handOfPl2[cardToPlay],
                iTween.Hash("y", 0.5f, "easeType", "Linear", "loopType", "none", "time", 0.4f));
        handOfPl2[cardToPlay].GetComponent<UICards>().bckImage.SetActive(false);
        trickZone.Add(handOfPl2[cardToPlay]);
        handOfPl2.RemoveAt(cardToPlay);
        //ComputeTrick();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickedCard = eventData.pointerCurrentRaycast.gameObject;
        int index = GameObjectToIndex(clickedCard);
        if (handOfPerson.Count > 10){
            Destroy(handOfPerson[index]);
            handOfPerson.RemoveAt(index);
        }
        else{
            handOfPerson[index].transform.SetParent(dropZone[indexPerson], true);
            iTween.MoveTo(handOfPerson[index],
                    iTween.Hash("position", dropZone[indexPerson].position, "easeType", "Linear", "loopType", "none", "time", 0.4f));
		            handOfPerson[index].GetComponent<UICards>().bckImage.SetActive(false);
            trickZone.Add(handOfPerson[index]);
            handOfPerson.RemoveAt(index);
            PlayTrickC1();
        }
            
    }
    int GameObjectToIndex(GameObject targetObj)
    {
        var value = targetObj.transform.parent.GetComponent<UICards>();
        int v = value.cardValue;
        var suit = targetObj.transform.parent.GetComponent<UICards>();
        string s = suit.cardSuit;
        for (int i = 0; i < handOfPerson.Count; i++)
        {
            string s1 =handOfPerson[i].GetComponent<UICards>().cardSuit;
            int v1 =handOfPerson[i].GetComponent<UICards>().cardValue;
            if (s.Equals(s1) && v == v1)
            {
                return i;
            }
        }
        return -1;
    }
}
