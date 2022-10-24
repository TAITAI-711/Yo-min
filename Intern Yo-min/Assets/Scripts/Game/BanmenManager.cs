using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanmenManager : SingletonMonoBehaviour<BanmenManager>
{
    [HideInInspector] public Banmen[] BanmenObj;
    public Gimmick_Wind Gimmick_WindObj = null;

    // Start is called before the first frame update

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        BanmenObj = GetComponentsInChildren<Banmen>();

        //foreach (var obj in BanmenObj)
        //{
        //    Debug.Log(obj);
        //}
    }

    void Start()
    {
        
    }
}
