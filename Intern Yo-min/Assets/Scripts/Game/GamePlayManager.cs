using Unity.Collections;
using UnityEngine;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
    [ReadOnly] public bool isGamePlay = true;
    public BanmenManager BanmenManagerObj = null;


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
