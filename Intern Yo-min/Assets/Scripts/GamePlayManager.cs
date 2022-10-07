using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
    public bool isGamePlay = true;


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // ƒV[ƒ“‚ª•Ï‚í‚Á‚Ä‚à€‚È‚È‚¢
    }

    // Start is called before the first frame update
    void Start()
    {
        isGamePlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
