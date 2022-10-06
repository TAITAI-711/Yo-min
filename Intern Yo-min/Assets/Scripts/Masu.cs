using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masu : MonoBehaviour
{
    private Osero OseroObj = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOsero(Osero osero)
    {
        if (OseroObj == null)
        {
            OseroObj = osero;
            OseroObj.OseroSet(transform.position);
        }
        else
        {
            Destroy(OseroObj);
            OseroObj = osero;
            OseroObj.OseroSet(transform.position);
        }
    }
}
