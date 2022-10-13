using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public Floor FloorObj = null;
    public PlayerMoveMax PlayerMoveMaxObj = null;

    private void Awake()
    {
        GamePlayManager.Instance.FloorManagerObj = this;

        FloorObj = GetComponentInChildren<Floor>();
        PlayerMoveMaxObj = GetComponentInChildren<PlayerMoveMax>();
    }
}
