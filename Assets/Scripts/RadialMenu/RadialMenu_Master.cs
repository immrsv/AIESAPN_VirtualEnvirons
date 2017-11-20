﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// TODO:
/// Preselect segment
/// Confirm Select Segment
/// Activate Segment
/// </summary>
namespace RadialMenu {
    public class RadialMenu_Master : MonoBehaviour {

        #region " Segment Layout Presets "

        private class SegmentPreset
        {
            public float Rotation;
            public float Size;
        }

        private Dictionary<int, List<SegmentPreset>> SegmentPresets = new Dictionary<int, List<SegmentPreset>>();

        public RadialMenu_Master()
        {
            // 8 Elements:
            var segmentCount = 8;
            SegmentPresets.Add(segmentCount, new List<SegmentPreset>());
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 0, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 045, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 090, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 135, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 180, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 225, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 270, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 315, Size = 40 });
        }

        #endregion

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

        [Header("Items")]
        public List<RadialMenu_MenuItem> RootItems;

        protected Stack<RadialMenu_MenuItem> Submenu = new Stack<RadialMenu_MenuItem>();

        // Use this for initialization
        void Start() {
            ConfirmationRadius = Mathf.Min(SelectionRadius, ConfirmationRadius);
        }

        // Update is called once per frame
        void Update() {

        }


        public void Show(Vector3 position, Vector3 forward) {

            Submenu.Clear();

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

        void BuildSegments() {

            var activeSegments = Submenu.Count > 0 ? Submenu.Peek().Items : RootItems;
            var ShowPaging = false;

            var SystemButtons = (ShowPaging ? 3 : 1);

            // Count = 1 (Back) + Item Count + (optional: paging buttons)
            var count = activeSegments.Count + SystemButtons;

            var layout = SegmentPresets.ContainsKey(count) ? SegmentPresets[count] : SegmentPresets[8];

            for ( var i = 0; i < layout.Count; i++)
            {
                Segments[i].transform.localRotation = Quaternion.Euler(0, 0, layout[i].Rotation);
                Segments[i].SegmentFillAmount = layout[i].Size / 360.0f;
            }
        }

        void WipeSegments(bool setVisible)
        {
            if (setVisible)
            {

            }
        }

        void ShowSegments(Action onComplete) { }
        void HideSegments(Action onComplete) { }

        public void UpdateCursor(Vector3 localPosition) {
            BroadcastMessage("RM_UpdateCursor", localPosition, SendMessageOptions.DontRequireReceiver);

            if (Cursor != null) Cursor.transform.localPosition = localPosition;

            TestSegments(localPosition);

        }


        protected bool SegmentContains(RadialMenu_SegmentAnimator segment, Vector3 point) {

            var localUp = (segment.transform.localRotation * Vector3.up);
            if (point.magnitude < SelectionRadius)
                return false;

            if (Mathf.Acos(Vector3.Dot(point.normalized, -localUp)) < segment.SegmentFillHalfangle)
                return true;

            return false;
        }

        protected void TestSegments(Vector3 localPosition)
        {
            for (var i = 0; i < Segments.Length; i++)
            {
                if (SegmentContains(Segments[i], localPosition))
                    Segments[i].BackgroundColor = Color.green;
                else
                    Segments[i].BackgroundColor = Color.black;
            }
        }

        protected void SelectSegment()
        {

        }

        protected void ConfirmSegment()
        {

        }
    }
}