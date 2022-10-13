using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [HideInInspector] private List<Osero> OseroObj = new List<Osero>();


    public void SetFieldOsero(Osero osero)
    {
        if (OseroObj.Count >= GamePlayManager.Instance.GamePadSelectObj.PlayerNum * 5)
        {
            Osero obj = OseroObj[0];
            OseroObj.Remove(OseroObj[0]);
            if (obj != null)
                obj.SetDestroy();
        }

        OseroObj.Add(osero);
    }

    public void RemoveFieldOsero(Osero osero)
    {
        OseroObj.Remove(osero);
    }
}
