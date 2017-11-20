using System;
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
/// "Paging"
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
        #region " Events "
        public event Action<object> MenuItemsChanged;
        public void RaiseMenuItemsChanged() { if (MenuItemsChanged != null) MenuItemsChanged(this); }

        public event Action<object, int> MenuPageChanged;
        public void RaiseMenuPageChanged(int oldValue) { if (MenuPageChanged != null) MenuPageChanged(this, oldValue); }

        public event Action<object, int> SelectionChanged;

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

        protected int _SelectedIndex;
        protected int SelectedIndex {
            get { return _SelectedIndex; }
            set {
                if (_SelectedIndex == value) return;
                if (_SelectedIndex >= 0 && _SelectedIndex < Segments.Length)
                    //Segments[_SelectedIndex].SetSelecting(false);
                    DeselectSegment(_SelectedIndex);

                var old = _SelectedIndex;
                _SelectedIndex = value;
                
                if (_SelectedIndex >= 0 && _SelectedIndex < Segments.Length)
                    //Segments[_SelectedIndex].SetSelecting(true);
                    SelectSegment(_SelectedIndex);
                // TODO: Raise Selecting Index Changed

            }
        }

        [Header("Items")]
        public List<RadialMenu_MenuItem> RootItems;

        protected Stack<RadialMenu_MenuItem> MenuStack = new Stack<RadialMenu_MenuItem>();

        protected RadialMenu_MenuItem[] _CurrentItems;
        protected RadialMenu_MenuItem[] CurrentItems {
            get {
                return _CurrentItems;
            }
            set {
                if (_CurrentItems == value) return;

            }
        }

        protected int _ItemsPage;
        protected int ItemsPage {
            get {
                return _ItemsPage;
            }
            set {
                if (_ItemsPage == value) return;

            }
        }

        protected LTDescr Tween;

        void Start() {
            ConfirmationRadius = Mathf.Min(SelectionRadius, ConfirmationRadius);
        }

        public void Show(Vector3 position, Vector3 forward) {

            MenuStack.Clear();

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
                    SelectedIndex = -1;
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

            var activeSegments = MenuStack.Count > 0 ? MenuStack.Peek().Children : RootItems;
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

            // For each segment
            //  
        }

        protected void TestSegments(Vector3 localPosition)
        {
            for (var i = 0; i < Segments.Length; i++)
            {
                if (Segments[i].Contains(localPosition))
                {
                    if (localPosition.magnitude > SelectionRadius)
                    {
                        SelectedIndex = i;
                    }
                }
            }
        }


        protected void SelectSegment(int index)
        {

            Segments[index].BackgroundColor = Color.green;
        }

        protected void DeselectSegment(int index)
        {

            Segments[index].BackgroundColor = Color.black;
        }

        protected void ConfirmSegment()
        {

        }
        
    }
}