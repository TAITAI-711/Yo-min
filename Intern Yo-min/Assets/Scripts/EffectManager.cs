using UnityEngine;


public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    [System.Serializable]
    public struct Effect_Info
    {
        public string EffectName;
        public GameObject EffectObj;
    }

    [SerializeField] private Effect_Info[] EffectObj;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // ÉVÅ[ÉìÇ™ïœÇÌÇ¡ÇƒÇ‡éÄÇ»Ç»Ç¢
    }


    public void SetEffect(string EffectName, Vector3 Pos, Quaternion Rot, float Size)
    {
        for (int i = 0; i < EffectObj.Length; ++i)
        {
            if (EffectObj[i].EffectName == EffectName)
            {
                GameObject Effect = Instantiate(EffectObj[i].EffectObj, Pos, Rot);
                Effect.transform.localScale *= Size;
                ParticleSystem[] particles;
                particles = Effect.gameObject.GetComponentsInChildren<ParticleSystem>();

                for (int j = 0; j < particles.Length; ++j)
                {
                    particles[i].Play();
                }
                return;
            }
        }
    }
}
