using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : SingletonMonoBehaviour<EventSystemManager>
{
    public EventSystem EventSystemObj = null;   // �C�x���g�V�X�e��
    public StandaloneInputModuleButton StandaloneInputObj = null;   // �X�^���h�A�����C���v�b�g

    private GameObject OldSelectObject = null;  // �ЂƂO�̑I���I�u�W�F

    public GameObject NoSelectObj = null;   // �����Ȃ�Ȃ��I�u�W�F�N�g�I��p

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // �V�[�����ς���Ă����ȂȂ�


        EventSystemObj = gameObject.GetComponent<EventSystem>();
        StandaloneInputObj = gameObject.GetComponent<StandaloneInputModuleButton>();
    }


    private void Update()
    {
        GameObject SelectObj = EventSystemObj.currentSelectedGameObject;

        if (OldSelectObject != SelectObj &&
            SelectObj != null &&
            OldSelectObject != null)
        {
            if (NoSelectObj == null || (NoSelectObj != null && NoSelectObj != SelectObj))
                SoundManager.Instance.PlaySound("�V�X�e���ړ�", false);
        }

        OldSelectObject = SelectObj;
    }
}
