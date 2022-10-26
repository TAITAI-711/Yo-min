using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gimmick_Wind;

public class Gimmik_WindEffect : MonoBehaviour
{
    private Gimmick_Wind Gimmick_WindObj;
    private Enum_Wind_Type Wind_Type = Enum_Wind_Type.Right;

    // Start is called before the first frame update
    void Start()
    {
        Gimmick_WindObj = gameObject.GetComponentInParent<Gimmick_Wind>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Wind_Type != Gimmick_WindObj.WindInfo.Wind_Type)
        {
            Wind_Type = Gimmick_WindObj.WindInfo.Wind_Type;

            switch (Wind_Type)
            {
                case Enum_Wind_Type.Up:
                    transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    break;
                case Enum_Wind_Type.Down:
                    transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;
                case Enum_Wind_Type.Left:
                    transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    break;
                case Enum_Wind_Type.Right:
                default:
                    transform.rotation = Quaternion.identity;
                    break;
            }  
        }
    }
}
