using Unity.Collections;
using UnityEngine;

public class GamePlayManager : SingletonMonoBehaviour<GamePlayManager>
{
    [ReadOnly] public bool isGamePlay = false;
    [ReadOnly] public bool isGamePadOK = false;
    public BanmenManager BanmenManagerObj = null;
    public UI_GamePadSelect GamePadSelectObj = null;


    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // �V�[�����ς���Ă����ȂȂ�

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
