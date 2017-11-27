using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeasurementTool : MonoBehaviour {
    public GameObject redSphere;
    public GameObject greenSphere;
    public LineRenderer distanceLine;
    public float distance;
    private int sphereIndex;

    public TextMeshPro distanceText;
    public TextMeshProUGUI distanceToGroundRed;
    public TextMeshProUGUI distanceToGroundGreen;
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;


    protected bool IsVisible = false;

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

    private void TrackedController_Ungripped(object sender, ClickedEventArgs e)
    {
        IsVisible = false;

        //Debug.Log("Trigger realsed");
        //move spheres
        if (sphereIndex == 0)
        {
            //placing start
            redSphere.transform.position = teleportReticleTransform.position - teleportReticleOffset;
            sphereIndex = 1;
        }

        else
        {
            //placing end
            greenSphere.transform.position = teleportReticleTransform.position - teleportReticleOffset;
            sphereIndex = 0;
        }
    }

    private void TrackedController_Gripped(object sender, ClickedEventArgs e)
    {
        IsVisible = true; 
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedController.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);


        // Update Line Renderer Positions:
        var lr = laser.GetComponent<LineRenderer>();

        var posns = new Vector3[lr.positionCount];
        var posnCount = lr.GetPositions(posns);

        posns[0] = trackedController.transform.position;
        posns[1] = hit.point;

        lr.SetPositions(posns);
    }


    void Start () {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    float delay = 0.5f;

	// Update is called once per frame
	void Update () {
        if ( delay > 0 ) { delay -= Time.deltaTime; return; }

        if (IsVisible)
        {

            //Debug.Log("holding trigger");
            RaycastHit hit;
            if (Physics.Raycast(trackedController.transform.position, trackedController.transform.forward, out hit, 100, teleportMask))
            {
                //Debug.Log("If is triggering");
                hitPoint = hit.point;
                ShowLaser(hit);
                reticle.SetActive(true);

                teleportReticleTransform.position = hitPoint + teleportReticleOffset;

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
            //distanceText.transform.up = Vector3.up;
            //RaycastHit hitInfo;
            //var direction = (distanceText.transform.position - Camera.main.transform.position).normalized;
            //if (Physics.Raycast(Camera.main.transform.position, direction, out hitInfo))
            //{
            //    if (hitInfo.collider.gameObject != distanceText)
            //    {
            //        distanceText.transform.position = Camera.main.transform.position + direction * hitInfo.distance *0.95f;
            //    }
            //}
        }

        
    }
}
