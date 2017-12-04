using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeasurementTool : MonoBehaviour {
    public RadialMenu.ScriptedMenus.RadialMenu_MenuItem MenuItem;

    
    public GameObject redSphere;
    public LineRenderer redVertical;

    public GameObject greenSphere;
    public LineRenderer greenVertical;

    
    public float distance;
    private int sphereIndex;

    [Space]
    public GameObject HudPanel;
    public GameObject DistancePanel;
    public TextMeshPro distanceText;
    public LineRenderer distanceLine;

    public TextMeshProUGUI distanceToGroundRed;
    public TextMeshProUGUI distanceToGroundGreen;

    [Space]
    public GameObject laserPrefab;
    public Color TargetValid = Color.green;
    public Color TargetInvalid = Color.red;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;

    public bool SnapToY = false;

    protected bool IsVisible = false;

    public int SphereCount {  get { return (redSphere.activeInHierarchy ? 1 : 0) + (greenSphere.activeInHierarchy ? 1 : 0); } }

    public GameObject NextMarker {
        get {
            var result = sphereIndex == 0 ? redSphere : greenSphere;

            sphereIndex = (int)Mathf.Repeat(++sphereIndex, 2);

            return result;
        }
    }

    public SteamVR_TrackedController trackedController;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedController.controllerIndex); }
    }

    void Awake()
    {
        trackedController.Gripped += TrackedController_Gripped;
        trackedController.Ungripped += TrackedController_Ungripped;
    }

    void Start() {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;

        ResetMarkers();
    }

    void ToggleSnap() { SnapToY = !SnapToY; }

    private void OnEnable() {

        //TODO: turn off distance display and lines to ground
        sphereIndex = 0;

        MenuItem.name = "Turn Off";
        ResetMarkers();

        if (HudPanel) HudPanel.SetActive(true);
    }

    private void OnDisable() {
        MenuItem.name = "Turn On";
        ResetMarkers();

        if (HudPanel) HudPanel.SetActive(false);
    }

    private void ResetMarkers() {
        if (redSphere) redSphere.SetActive(false);
        if (redVertical) redVertical.enabled = false;

        if (greenSphere) greenSphere.SetActive(false);
        if (greenVertical) greenVertical.enabled = false;

        if (DistancePanel) DistancePanel.SetActive(false);
        if (distanceToGroundRed) distanceToGroundRed.enabled = false;
        if (distanceToGroundGreen) distanceToGroundGreen.enabled = false;
        if (distanceLine) distanceLine.enabled = false;
    }

    void ActivateMarker(GameObject marker) {

        if (marker) marker.SetActive(true);
        if (marker) marker.transform.position = teleportReticleTransform.position - teleportReticleOffset;

        if ( marker == redSphere) {
            if (redVertical) redVertical.enabled = true;
            if (distanceToGroundRed) distanceToGroundRed.enabled = true;
        }

        if ( marker == greenSphere) {
            if (greenVertical) greenVertical.enabled = true;
            if (distanceToGroundGreen) distanceToGroundGreen.enabled = true; 
        }

        if ( SphereCount == 2) {
            if (distanceLine) distanceLine.enabled = true;
            if (DistancePanel) DistancePanel.SetActive(true);
        }
    }

    private void TrackedController_Ungripped(object sender, ClickedEventArgs e)
    {
        IsVisible = false;
        ActivateMarker(NextMarker);        
    }

    private void TrackedController_Gripped(object sender, ClickedEventArgs e)
    {
        IsVisible = true; 
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedController.transform.position, hit.point, .5f);
        laserTransform.LookAt(hit.point);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);


        // Update Line Renderer Positions:
        var lr = laser.GetComponent<LineRenderer>();

        var posns = new Vector3[lr.positionCount];
        var posnCount = lr.GetPositions(posns);

        posns[0] = trackedController.transform.position;
        posns[1] = hit.point;

        lr.SetPositions(posns);
        laser.GetComponent<MeshRenderer>().material.color = IsVisible ? TargetValid : TargetInvalid;
    }




    float delay = 0.5f;

	// Update is called once per frame
	void Update () {
        if ( delay > 0 ) { delay -= Time.deltaTime; return; }

        if (IsVisible)
        {

            bool hitValid = false;
            RaycastHit hitInfo;
            var hit = Physics.Raycast(trackedController.transform.position, transform.forward, out hitInfo, 100.0f);
            if (hit)
            {
                hitValid = true;

                hitPoint = hitInfo.point;
                ShowLaser(hitInfo);
                reticle.SetActive(true);

                teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                
            }

            if (!hitValid) {
                if (!hit) {
                    var distance = 100;
                    hitInfo.point = trackedController.transform.TransformPoint(Vector3.forward * distance);
                    hitInfo.distance = distance;
                }
                ShowLaser(hitInfo);
                reticle.SetActive(false);
            }

        }
        else
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }


        if (redSphere.activeSelf && greenSphere.activeSelf)
        {
            distanceLine.SetPosition(0, redSphere.transform.localPosition);
            distanceLine.SetPosition(1, greenSphere.transform.localPosition);

            distance = (redSphere.transform.position - greenSphere.transform.position).magnitude;
            distanceText.text = distance.ToString("N3") + "m";
            if (distanceToGroundRed)
                distanceToGroundRed.text = "Red Y: " + redSphere.transform.position.y.ToString("N3");

            if (distanceToGroundGreen)
                distanceToGroundGreen.text = "Green Y: " + greenSphere.transform.position.y.ToString("N3");

            var textPos = greenSphere.transform.position + (redSphere.transform.position - greenSphere.transform.position) / 2;
            distanceText.transform.position = textPos + new Vector3(0, 1, 0);
            distanceText.transform.forward = Camera.main.transform.forward;
        }

        
    }
}
