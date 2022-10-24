using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_Ice : MonoBehaviour
{
    [Tooltip("ŠŠ‚è‚â‚·‚³"), Range(0.0f, 1.0f)]
    public float IcePow = 1.0f;    // ŠŠ‚è‚â‚·‚³(1.0f‚Åˆê”ÔŠŠ‚é)

    [Range(1, 20)] public int Tate = 9;
    [Range(1, 20)] public int Yoko = 9;

    private void OnValidate()
    {
        gameObject.transform.localScale = new Vector3(
            Yoko * GamePlayManager.MasuScaleXZ,
            GamePlayManager.MasuScaleY * 0.5f,
            Tate * GamePlayManager.MasuScaleXZ);

        Vector3 Pos = transform.position;
        Pos.y = GamePlayManager.MasuScaleY * 0.25f;
        gameObject.transform.position = Pos;

        //MeshRenderer Mr = this.GetComponent<MeshRenderer>();

        //if (Mr.sharedMaterial != null)
        //{
        //    Color color = Mr.sharedMaterial.color;
        //    color.b = IcePow;
        //    Mr.sharedMaterial.color = color;
        //}
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
