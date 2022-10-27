using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banmen_Move : Banmen
{
    [Header("[ à⁄ìÆî’ñ ê›íË ]")]

    [Tooltip("à⁄ìÆéûä‘")]
    public float MoveTime = 3.0f;

    [Tooltip("í‚é~éûä‘")]
    public float StopTime = 5.0f;

    private float NowMoveTime = 0.0f;
    private float NowStopTime = 0.0f;
    private bool isMoveFront = true;

    private Vector3 StartPos;
    private Vector3 EndPos;

    public GameObject MovePointObj = null;

    private Rigidbody Rb = null;
    private Vector3 OldPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartPos = OldPos = this.transform.position;
        EndPos = MovePointObj.transform.position;

        MovePointObj.GetComponent<MeshRenderer>().enabled = false;

        if ((Rb = gameObject.GetComponent<Rigidbody>()) == null)
        {
            Rb = gameObject.AddComponent<Rigidbody>();
        }

        Rb.mass = 10000000.0f;
        Rb.drag = 0.0f;
        Rb.useGravity = false;
        Rb.freezeRotation = true;
        Rb.constraints = RigidbodyConstraints.None;
        Rb.constraints = RigidbodyConstraints.FreezeRotation | 
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezePositionZ;
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        gameObject.transform.localScale = new Vector3(
            Yoko * GamePlayManager.MasuScaleXZ,
            GamePlayManager.MasuScaleY,
            Tate * GamePlayManager.MasuScaleXZ);

        Vector3 Pos = transform.position;
        Pos.y = GamePlayManager.MasuScaleY * 0.5f;
        gameObject.transform.position = Pos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GamePlayManager.Instance.isGamePlay)
            return;

        // ìÆÇ¢ÇƒÇÈ
        if (NowStopTime <= 0.0f)
        {
            // ëOà⁄ìÆ
            if (isMoveFront)
            {
                //transform.position = StartPos + (EndPos - StartPos) * (NowMoveTime / MoveTime);
                Rb.velocity = ((StartPos + (EndPos - StartPos) * (NowMoveTime / MoveTime)) - OldPos) * (1.0f / Time.fixedDeltaTime);

                OldPos = StartPos + (EndPos - StartPos) * (NowMoveTime / MoveTime);
            }
            else
            {
                //transform.position = EndPos - (EndPos - StartPos) * (NowMoveTime / MoveTime);
                Rb.velocity = ((EndPos - (EndPos - StartPos) * (NowMoveTime / MoveTime)) - OldPos) * (1.0f / Time.fixedDeltaTime);

                OldPos = EndPos - (EndPos - StartPos) * (NowMoveTime / MoveTime);
            }

            NowMoveTime += Time.fixedDeltaTime;

            // é~Ç‹ÇÈ
            if (NowMoveTime >= MoveTime)
            {
                NowStopTime = StopTime;

                Rb.velocity = Vector3.zero;

                if (isMoveFront)
                    transform.position = EndPos;
                else
                    transform.position = StartPos;

                OldPos = transform.position;
            }
        }
        // é~Ç‹Ç¡ÇƒÇÈ
        else
        {
            NowStopTime -= Time.fixedDeltaTime;

            if (NowStopTime <= 0.0f)
            {
                isMoveFront = !isMoveFront;
                NowMoveTime = 0.0f;
            }
        }
    }
}
