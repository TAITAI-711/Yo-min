using Unity.Collections;
using UnityEngine;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
    [ReadOnly] public bool isGamePlay = false;
    [ReadOnly] public bool isGamePadOK = false;
    [ReadOnly] public BanmenManager BanmenManagerObj = null;
    [ReadOnly] public UI_GamePadSelect GamePadSelectObj = null;
    [ReadOnly] public PlayerManager PlayerManagerObj = null;
    [ReadOnly] public Floor FloorObj = null;
    [ReadOnly] public static readonly float MasuScaleY = 4.6f;


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // ÉVÅ[ÉìÇ™ïœÇÌÇ¡ÇƒÇ‡éÄÇ»Ç»Ç¢

        isGamePlay = false;
        isGamePadOK = false;
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
