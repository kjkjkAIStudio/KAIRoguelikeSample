using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KAI;

public class Mono_Core : MonoBehaviour
{
    public GameObject prefab_bullet;

    [System.Serializable]
    public class UI_state
    {
        public GameObject h;
        public Slider slider_hp;
        public TextMeshProUGUI txt_stage;
    }
    public UI_state ui_state;

    [System.Serializable]
    public class UI_dlg
    {
        public GameObject h;
        public TextMeshProUGUI txt;
        public Button btn_ok;
    }
    public UI_dlg ui_dlg;

    private void Awake()
    {
        if (Core.Inst == null)
            Core.Init(this);
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        Core.Inst.Update();
    }

    private void FixedUpdate()
    {
        Core.Inst.FixedUpdate();
    }

    private void LateUpdate()
    {
        Core.Inst.LateUpdate();
    }
}
