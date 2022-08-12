using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickOnCard : MonoBehaviour,IPointerClickHandler
{
    public GameCtrl gc;
    public void OnPointerClick(PointerEventData eventData)
    {
        gc.InitCard();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
