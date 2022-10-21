using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : SingletonMonoBehaviour<FloorManager>
{
    public Floor FloorObj = null;
    public PlayerMoveMax PlayerMoveMaxObj = null;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        FloorObj = GetComponentInChildren<Floor>();
        PlayerMoveMaxObj = GetComponentInChildren<PlayerMoveMax>();
    }
}
