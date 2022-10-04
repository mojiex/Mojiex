using UnityEngine;

namespace Mojiex
{


    public class BubbleCom : MonoBehaviour
    {
        public int BubbleId;

        public Transform TargetTrans;
        public Vector3 Offset;
        public Vector3 realPos;
        public Vector3 cameraPos;
        public bool ChangeScale = true;
        public bool IsFinish = false;

        public float maxCameraZoom = 30;
        public float minCameraZoom = 8;

        public float Scale { get; protected set; } = 1.0f;
        // Start is called before the first frame update
        public Camera MainCam
        {
            get
            {
                if (mainCam == null)
                    mainCam = Camera.main;
                return mainCam;
            }
        }
        private Camera mainCam;
        public Camera UiCam
        {
            get
            {
                if (uiCam == null)
                {
                    uiCam = Mojiex.Mgr.uiMgr.GetUICamera();
                }
                return uiCam;
            }
        }
        private Camera uiCam;

        public virtual void Init(BubbleInfo info)
        {
            SetTarget(info.offset, info.target, info.ChangeScale, info.Id);
        }
        private void SetTarget(Vector3 offset, Transform target, bool _changeScale, int i)
        {
            BubbleId = i;
            ChangeScale = _changeScale;
            UpdatePosition(target, offset);
        }

        public void UpdatePosition(Transform trans, Vector3 ost)
        {
            TargetTrans = trans;
            Offset = ost;
            gameObject.SetActive(true);

            ResetPos();
            ResetScale();
        }

        protected virtual void Update()
        {

            if (!IsFinish && ChangeScale)
            {
                ResetScale();
            }

            if (realPos != Vector3.zero)
            {
                ResetPos();
            }
        }

        protected void ResetScale()
        {
            if (gameObject && MainCam)
            {
                transform.localScale = (maxCameraZoom / MainCam.orthographicSize) * Scale * Vector3.one;
            }
        }

        private void ResetPos()
        {
            if (TargetTrans != null)
            {
                realPos = TargetTrans.position + Offset;
            }
            cameraPos = MainCam.WorldToScreenPoint(realPos);
            cameraPos = UiCam.ScreenToWorldPoint(cameraPos);
            transform.position = cameraPos;
        }
    }

    public class BubbleInfo
    {
        public int Id;
        public Vector3 offset;
        public Transform target;
        public bool ChangeScale = true;
    }
}