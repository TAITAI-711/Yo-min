using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanmenManager : MonoBehaviour
{
    [HideInInspector] public Banmen[] BanmenObj;

    // Start is called before the first frame update

    private void Awake()
    {
        BanmenObj = GetComponentsInChildren<Banmen>();

        GamePlayManager.Instance.BanmenManagerObj = this;

        //foreach (var obj in BanmenObj)
        //{
        //    Debug.Log(obj);
        //}
    }

    void Start()
    {
        
    }
}
