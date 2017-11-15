using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RadialMenu {
    public class RadialMenu_Master : MonoBehaviour {
        [System.Serializable]
        public class MenuColors {
            public Color BackColor = Color.black;
            public Color HoverColor = Color.gray;
        }

        public float RevealDelay = 0.3f;

        public MenuColors Colors;

        [Header("Segments")]
        public GameObject Cursor;
        public GameObject Centre;
        public GameObject SegmentsMaster;
        public RadialMenu_SegmentAnimator[] Segments;

        public bool IsVisible { get { return Centre.activeSelf; } }

        [Header("Selection")]
        [Range(0,1)]
        public float SelectionRadius;
        [Range(0,1)]
        public float ConfirmationRadius;

        protected int SelectedIndex;
        protected float SelectionStart;

        // Use this for initialization
        void Start() {
            ConfirmationRadius = Mathf.Min(SelectionRadius, ConfirmationRadius);
        }

        // Update is called once per frame
        void Update() {

        }


        public void Show(Vector3 position, Vector3 forward) {

            if (Cursor != null) Cursor.SetActive(true);

            Centre.SetActive(true);
            for (var i = 0; i < Segments.Length; i++) {
                if (Segments[i] == null) continue;
                Segments[i].gameObject.SetActive(true);
            }

            transform.position = position;
            transform.forward = forward;


            LeanTween.value(SegmentsMaster, (value) => { SegmentsMaster.GetComponent<Image>().fillAmount = value; }, 0, 1, RevealDelay)
                .setOnStart(() => {
                    SegmentsMaster.GetComponent<Image>().enabled = true;
                    SegmentsMaster.GetComponent<Image>().fillClockwise = true;
                    SegmentsMaster.GetComponent<Image>().fillAmount = 0;

                    SegmentsMaster.GetComponent<Mask>().enabled = true;
                })
                .setOnComplete(() => {
                    SegmentsMaster.GetComponent<Image>().enabled = false;
                    SegmentsMaster.GetComponent<Mask>().enabled = false;
                });
        }


        public void Hide() {

            LeanTween.value(SegmentsMaster, (value) => { SegmentsMaster.GetComponent<Image>().fillAmount = value; }, 1, 0, RevealDelay)
                .setOnStart(() => {
                    SegmentsMaster.GetComponent<Image>().enabled = true;
                    SegmentsMaster.GetComponent<Image>().fillClockwise = false;
                    SegmentsMaster.GetComponent<Image>().fillAmount = 1;

                    SegmentsMaster.GetComponent<Mask>().enabled = true;
                })
                .setOnComplete(() => {
                    if (Cursor != null) Cursor.SetActive(false);

                    Centre.SetActive(false);
                    for (var i = 0; i < Segments.Length; i++) {
                        if (Segments[i] == null) continue;
                        Segments[i].gameObject.SetActive(false);
                    }
                });

        }


        void ShowSegments(Action onComplete) { }
        void HideSegments(Action onComplete) { }

        public void UpdateCursor(Vector3 localPosition) {
            BroadcastMessage("RM_UpdateCursor", localPosition, SendMessageOptions.DontRequireReceiver);

            if (Cursor != null) Cursor.transform.localPosition = localPosition;

            foreach ( var segment in Segments) {
                if (SegmentContains(segment, localPosition))
                    segment.BackgroundColor = Color.green;
                else
                    segment.BackgroundColor = Color.black;
            }
        }


        protected bool SegmentContains(RadialMenu_SegmentAnimator segment, Vector3 point) {

            var localUp = (segment.transform.localRotation * Vector3.up);
            if (point.magnitude < SelectionRadius)
                return false;

            if (Mathf.Acos(Vector3.Dot(point.normalized, -localUp)) < segment.SegmentFillHalfangle)
                return true;

            return false;
        }
    }
}