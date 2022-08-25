using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameCtrl : MonoBehaviour, IPointerClickHandler
{
    public Text text, text2, trumpS, plSocer, op1Score, op2Score;

    public int bidValue, myBid, indexPerson, gameNum, indexCom1, indexCom2;
    public int personScore, com1Score, firstBidder, com2Score = 0;
    private int dropZoneIndex;
    private string bidWinner, trumpSuit;
    public GameObject cards;
    public Transform tfDeck;
    public Transform[] tfPlayer, tfOpponent1, tfOpponent2, dropZone, scores;
    public List<GameObject> deckOfCards = new List<GameObject>();
    public List<GameObject> handOfPerson = new List<GameObject>();
    public List<GameObject> handOfPl1 = new List<GameObject>();
    public List<GameObject> handOfPl2 = new List<GameObject>();
    public List<GameObject> trickZone = new List<GameObject>();
    Dictionary<string, int> numOfSuitsPl1 = new Dictionary<string, int>(){ {"c",0},{"h",0},{"s",0},{"d",0} };
    Dictionary<string, int> numOfSuitsPl2 = new Dictionary<string, int>(){ {"c",0},{"h",0},{"s",0},{"d",0} };
    public GameObject clickedCard;
    public Dropdown dpSuit, dpBid;
    public Button btn;
    private bool roundIsFinished= false;
    // Start is called before the first frame update
    void Start()
    {
        //btn.gameObject.SetActive(false);
        dpSuit.interactable = false;
        dpBid.interactable = false;
        InitCard();
        
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
    }

    public void GetTrumpSuit(int val){
        if(val == 1){
            trumpSuit = "c";
        }
        else if(val == 2){
            trumpSuit = "s";
        }
        else if(val == 3){
            trumpSuit = "d";
        }
        else if(val == 4){
            trumpSuit = "h";
        }
        trumpS.text = trumpSuit;
        dpSuit.interactable = false;
        EnableClickOnCards(true);
    }
    
    // Update is called once per frame
    void Update()
    {

        if (dpBid.value > 0){
            FirstToStart();
            dpBid.value = 0;
        }
        if (dpSuit.value > 0){
            dpSuit.value = 0;
        }
        if (roundIsFinished){
            //dpBid.value = 0;
            dpBid.interactable = true;
        }
        roundIsFinished = false;
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

        numOfSuitsPl1["c"] = 0;
        numOfSuitsPl1["h"] = 0;
        numOfSuitsPl1["s"] = 0;
        numOfSuitsPl1["d"] = 0;
        numOfSuitsPl2["c"] = 0;
        numOfSuitsPl2["h"] = 0;
        numOfSuitsPl2["s"] = 0;
        numOfSuitsPl2["d"] = 0;
    }

    IEnumerator DealCards() {

        for(int i = 0; i < 10; ++i){
            yield return new WaitForSeconds(0.1f);
            int rndCard = Random.Range(0, deckOfCards.Count);
            
            deckOfCards[rndCard].transform.SetParent(tfPlayer[i], true);
            iTween.MoveTo(deckOfCards[rndCard],
                    iTween.Hash("position", tfPlayer[i].position, "easeType", "Linear", "time", 0.4f));
		    iTween.RotateBy(deckOfCards[rndCard],
                    iTween.Hash("y", 0.5f, "easeType", "Linear", "time", 0.4f));//"loopType", "none"
            yield return new WaitForSeconds(0.15f);
            deckOfCards[rndCard].GetComponent<UICards>().bckImage.SetActive(false);
            handOfPerson.Add(deckOfCards[rndCard]);
            deckOfCards.RemoveAt(rndCard);

            yield return new WaitForSeconds(0.1f);
            int rndCardForPy1 = Random.Range(0, deckOfCards.Count);
            deckOfCards[rndCardForPy1].transform.SetParent(tfOpponent1[i], true);
            iTween.MoveTo(deckOfCards[rndCardForPy1],
                    iTween.Hash("position", tfOpponent1[i].position, "easeType", "Linear", "time", 0.4f));
            handOfPl1.Add(deckOfCards[rndCardForPy1]);
            deckOfCards.RemoveAt(rndCardForPy1);
            
            yield return new WaitForSeconds(0.1f);
            int rndCardForPy2 = Random.Range(0, deckOfCards.Count);
            deckOfCards[rndCardForPy2].transform.SetParent(tfOpponent2[i], true);
            iTween.MoveTo(deckOfCards[rndCardForPy2],
                    iTween.Hash("position", tfOpponent2[i].position, "easeType", "Linear", "time", 0.4f));
            handOfPl2.Add(deckOfCards[rndCardForPy2]);        
            deckOfCards.RemoveAt(rndCardForPy2);

        }
        EnableClickOnCards(false);
    }
    public void EnableClickOnCards(bool tf){
         for(int i = 0;i < handOfPerson.Count; ++i){
            handOfPerson[i].GetComponent<UICards>().cardImg.raycastTarget = tf;
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
                    iTween.Hash("position", tfPlayer[i+10].position, "easeType", "Linear", "time", 0.4f));
		        iTween.RotateBy(deckOfCards[i],
                    iTween.Hash("y", 0.5f, "easeType", "Linear", "time", 0.4f));
                    yield return new WaitForSeconds(0.15f);
            deckOfCards[i].GetComponent<UICards>().bckImage.SetActive(false);
            handOfPerson.Add(deckOfCards[i]);
            }
            else if(pl == 2)
            {
                deckOfCards[i].transform.SetParent(tfOpponent1[i+10], true);
            iTween.MoveTo(deckOfCards[i],
                    iTween.Hash("position", tfOpponent1[i+10].position, "easeType", "Linear", "time", 0.4f));
                    yield return new WaitForSeconds(0.15f); 
            handOfPl1.Add(deckOfCards[i]);    
            }
            else
            {
                deckOfCards[i].transform.SetParent(tfOpponent2[i+10], true);
            iTween.MoveTo(deckOfCards[i],
                    iTween.Hash("position", tfOpponent2[i+10].position, "easeType", "Linear", "time", 0.4f));
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
            handOfPerson.Clear(); handOfPl1.Clear(); handOfPl2.Clear(); deckOfCards.Clear();
            InitCard();
        }
        if (myBid > res1 && myBid > res2){
            StartCoroutine(DealTalon(1));
            dpSuit.interactable = true;
            return "person";
        }
        else if(res1 > myBid && res1 > res2){
            StartCoroutine(DealTalon(2));
            var firstEl = numOfSuitsPl1.OrderByDescending(kvp => kvp.Value).First();
            trumpSuit = firstEl.Key;
            trumpS.text = trumpSuit;
            return "com1";
        }
        else{
            StartCoroutine(DealTalon(3));
            var firstEl = numOfSuitsPl2.OrderByDescending(kvp => kvp.Value).First();
            trumpSuit = firstEl.Key;
            trumpS.text = trumpSuit;
            return "com2";
        }
    }

    public int BiddingRes(List<GameObject> hand, Dictionary<string,int> numOfSuits){
        for (int i = 0; i <hand.Count;++i){
                if (hand[i].GetComponent<UICards>().cardSuit.Equals("c")){
                    numOfSuits["c"]++;
                }
                else if (hand[i].GetComponent<UICards>().cardSuit.Equals("h")){
                    numOfSuits["h"]++;
                }
                else if (hand[i].GetComponent<UICards>().cardSuit.Equals("s")){
                    numOfSuits["s"]++;
                }
                else{
                    numOfSuits["d"]++;
                }
        
        }
        var firstEl = numOfSuits.OrderByDescending(kvp => kvp.Value).First();
        bidValue = firstEl.Value;
        if (bidValue < 4){
            return 0;
        }
        else if (bidValue == 4){
            return bidValue += 1;
        }
        else{
            return bidValue;
        }
    }
    
    public void FirstToStart(){
        bidWinner = StartBid();
        ++firstBidder;
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
            StartCoroutine(PlayTrickC2());
        }
        else if(bidWinner.Equals("com1"))
        {
            RemoveTwoCards(handOfPl1, numOfSuitsPl1);
            indexPerson = 2;
            indexCom1 = 0;
            indexCom2 = 1;
            StartCoroutine(PlayTrickC1());
        }

    }

    public void ComputeTrick(){
        int winCard;
        int numOfTrumps = 0;
        int indexOfWinCard = 0;
        var curTrumpSuit = trumpSuit;
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
        Destroy(trickZone[0]);
        Destroy(trickZone[1]);
        Destroy(trickZone[2]);
        trickZone.Clear();
        
        
        if(indexOfWinCard == indexPerson){
            ++personScore;
            indexPerson = 0;
            indexCom1 = 1;
            indexCom2 = 2;
            if(handOfPerson.Count == 0){
                goto end;
            }
        }
        else if(indexOfWinCard == indexCom1){
            ++com1Score;
            indexCom1 = 0;
            indexCom2 = 1;
            indexPerson = 2;
            if(handOfPl1.Count == 0){
                goto end;
            }
            else{
                StartCoroutine(PlayTrickC1());
            }
        }
        else{
            ++com2Score;
            indexCom2 = 0;
            indexPerson = 1;
            indexCom1 = 2;
            if(handOfPl2.Count == 0){
                goto end;
            }
            else{
                StartCoroutine(PlayTrickC2());
            }
            
        }
        Debug.Log(personScore);
        Debug.Log(com1Score);
        Debug.Log(com2Score);
    end:
        if(handOfPerson.Count == 0 && handOfPl1.Count == 0 && handOfPl2.Count ==0){
            bool isLastRound = CalculateScores();
            if(!isLastRound){
                InitCard();
                roundIsFinished = true;
            }
        }

    }
   
    public bool CalculateScores(){
         if(bidWinner.Equals("person")){
                if (personScore < myBid){
                    personScore = -(myBid *2);
                }
                if (com1Score == 0){
                    com1Score = -myBid;
                }
                if (com2Score == 0){
                    com2Score = -myBid;
                }
            }
            else if(bidWinner.Equals("com1")){
                if(com1Score < bidValue){
                    com1Score = -(bidValue*2);
                }
                if(com2Score == 0){
                    com2Score = -bidValue;
                }
                if(personScore == 0){
                    personScore = -bidValue;
                }
            }
            else if(bidWinner.Equals("com2")){
                if(com2Score < bidValue){
                    com2Score = -(bidValue*2);
                }
                if(com1Score == 0){
                    com1Score = -bidValue;
                }
                if(personScore == 0){
                    personScore = -bidValue;
                }
            }
            for (int i = 0; i < gameNum+1;++i){
                if (i == gameNum-1){
                    var prevS = scores[gameNum-1].transform.GetChild(0).gameObject.GetComponent<Text>();
                    personScore+= int.Parse(prevS.text);
                    var prevS1 = scores[gameNum-1].transform.GetChild(1).gameObject.GetComponent<Text>();
                    com1Score += int.Parse(prevS1.text);
                    var prevS2 = scores[gameNum-1].transform.GetChild(2).gameObject.GetComponent<Text>();
                    com2Score += int.Parse(prevS2.text);
                }
                if( i == gameNum){
                    var s = scores[i].transform.GetChild(0).gameObject.GetComponent<Text>();
                    s.text = personScore.ToString();
                    var s1 = scores[i].transform.GetChild(1).gameObject.GetComponent<Text>();
                    s1.text = com1Score.ToString();
                    var s2 = scores[i].transform.GetChild(2).gameObject.GetComponent<Text>();
                    s2.text = com2Score.ToString();
                }
                
                
            }
            ResetValues();
            ++gameNum;
            if (gameNum == 9){
                btn.gameObject.SetActive(true);
                return true;
            }
            return false;
    }
    public void ResetValues(){
        personScore = 0;
            com1Score = 0;
            com2Score = 0;
            myBid = 0;
            bidValue = 0;
            trumpS.text = "";
    }
    public void PlayAgain(){
        SceneManager.LoadScene("mexico");
        btn.gameObject.SetActive(false);
    }

    IEnumerator PlayTrickC1(){
        EnableClickOnCards(false);
        int cardToPlay = -1;
        if (indexCom1 == 0){
                cardToPlay = Random.Range(0, handOfPl1.Count);
        }
        else {
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
            }
            if (cardToPlay < 0){
                cardToPlay = Random.Range(0, handOfPl1.Count);
            }
        }
        
            handOfPl1[cardToPlay].transform.SetParent(dropZone[indexCom1], true);
        iTween.MoveTo(handOfPl1[cardToPlay],
            iTween.Hash("position", dropZone[indexCom1].position, "easeType", "Linear", "time", 0.9f));
        iTween.RotateBy(handOfPl1[cardToPlay], iTween.Hash("y", 0.5f, "easeType", "Linear", "time", 0.4f));
        handOfPl1[cardToPlay].GetComponent<UICards>().bckImage.SetActive(false);
        trickZone.Add(handOfPl1[cardToPlay]);
        handOfPl1.RemoveAt(cardToPlay);
        yield return new WaitForSeconds(2);
        if (trickZone.Count == 3){
            ComputeTrick();
            Cursor.lockState =CursorLockMode.Confined;
        }
        else{
            StartCoroutine(PlayTrickC2());
        }

    }
    IEnumerator PlayTrickC2(){
        
        int cardToPlay = -1;
        //ako je prvi na potezu, nasumično odigrati kartu
        if (indexCom2 == 0){
                cardToPlay = Random.Range(0, handOfPl2.Count);
        }
        else {
            for (int i = 0; i<handOfPl2.Count;++i){
                // karta koja se igra mora biti odgovarajuće boje prvoj bacenoj
                if (handOfPl2[i].GetComponent<UICards>().cardSuit.Equals(trickZone[0].GetComponent<UICards>().cardSuit)){
                    cardToPlay = i;
                }
            }
            //u slucaju da nema odgovarajuće boje, provjeriti dali ima aduta
            if (cardToPlay < 0){
                for (int j = 0; j<handOfPl2.Count;++j){
                // ako bacena karta odgovara adut boji ta karta se igra
                    if (handOfPl2[j].GetComponent<UICards>().cardSuit.Equals(trumpSuit)){
                        cardToPlay = j;
                    }
                }
            }
            //ako ne postoji niti adut niti odgovarajća boja prvoj bačenoj karti odigrati bilo koju kartu
            if (cardToPlay < 0){
                cardToPlay = Random.Range(0, handOfPl2.Count);
            }
            
        }
        handOfPl2[cardToPlay].transform.SetParent(dropZone[indexCom2], true);
        iTween.MoveTo(handOfPl2[cardToPlay],
                iTween.Hash("position", dropZone[indexCom2].position, "easeType", "Linear", "time", 0.9f));
		iTween.RotateBy(handOfPl2[cardToPlay],
                iTween.Hash("y", 0.5f, "easeType", "Linear", "time", 0.4f));
        handOfPl2[cardToPlay].GetComponent<UICards>().bckImage.SetActive(false);
        trickZone.Add(handOfPl2[cardToPlay]);
        handOfPl2.RemoveAt(cardToPlay);
        yield return new WaitForSeconds(2);
        EnableClickOnCards(true);
        if (trickZone.Count == 3){
            ComputeTrick();
        }
        
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
                    iTween.Hash("position", dropZone[indexPerson].position, "easeType", "Linear", "time", 0.4f));
		            handOfPerson[index].GetComponent<UICards>().bckImage.SetActive(false);
            trickZone.Add(handOfPerson[index]);
            handOfPerson.RemoveAt(index);
            if (trickZone.Count == 3){
                ComputeTrick();
            }
            else{
                StartCoroutine(PlayTrickC1());
            }
            
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
