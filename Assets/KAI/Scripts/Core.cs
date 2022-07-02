using UnityEngine;
using KAIModule.Mgr;
using KAI.Mgr;

namespace KAI
{
    public class Core
    {
        public static Core Inst { get; private set; }

        public Mono_Core Mono { get; private set; }
        public IResMgr RM { get; private set; }
        public DataMgr DM { get; private set; }
        public Cam2DMgr CamM { get; private set; }
        public StageMgr StageM { get; private set; }
        public GameMgr GM { get; private set; }

        public static void Init(Mono_Core mono)
        {
            Inst = new Core(mono);
        }

        public Core(Mono_Core mono)
        {
            Mono = mono;
            Object.DontDestroyOnLoad(mono.gameObject);
            mono.ui_dlg.h.SetActive(false);
            mono.ui_state.h.SetActive(false);

            RM = new ResMgrAssetBundle();
            DM = new DataMgr(RM);
            CamM = new Cam2DMgr();
            CamM.SetCam(Camera.main);
            CamM.T_off = new Vector3(0.0f, 0.0f, -10.0f);
            StageM = new StageMgr();

            ToTitle();
        }

        public void Update()
        {
            if (GM != null)
                GM.Update();
        }

        public void FixedUpdate()
        {
            if (GM != null)
                GM.FixedUpdate();
        }

        public void LateUpdate()
        {
            CamM.LateUpdate();
        }

        public void StartGame()
        {
            GM = new GameMgr();
            GM.StartGame();
        }

        public void EndGame()
        {
            GM.EndGame();
            GM = null;
            ToTitle();
        }

        public void ShowDlg(string str, System.Action okCallback = null)
        {
            Mono.ui_dlg.txt.text = str;
            Mono.ui_dlg.btn_ok.onClick.AddListener(() =>
            {
                Mono.ui_dlg.btn_ok.onClick.RemoveAllListeners();
                Mono.ui_dlg.h.SetActive(false);
                okCallback?.Invoke();
            });
            Mono.ui_dlg.h.SetActive(true);
        }

        private void ToTitle()
        {
            StageM.ToTitle((mono) =>
            {
                mono.btn_start.onClick.AddListener(StartGame);
            });
        }
    }
}