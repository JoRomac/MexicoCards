using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGame : MonoBehaviour
{
    public static SpriteGame spGame;
    public Sprite[] spCards;
    
    private void Awake(){
        if(spGame == null){
            spGame = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            DestroyImmediate(gameObject);
        }
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
