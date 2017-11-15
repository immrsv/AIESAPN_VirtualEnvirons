using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeasurementTool : MonoBehaviour {
    public GameObject startSphere;
    public GameObject endSphere;
    public LineRenderer line;
    public float distance;
    private int sphereIndex;

    public TextMeshPro distanceText;
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
            startSphere.transform.position = teleportReticleTransform.position - teleportReticleOffset;
            sphereIndex = 1;
        }

        else
        {
            //placing end
            endSphere.transform.position = teleportReticleTransform.position - teleportReticleOffset;
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
            Debug.Log(trackedController.transform.position + "  " + transform.forward);
            if (Physics.Raycast(trackedController.transform.position, trackedController.transform.forward, out hit, 100, teleportMask))
            {
                Debug.Log("If is triggering");
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


        if (startSphere.activeSelf && endSphere.activeSelf)
        {
            line.SetPosition(0, startSphere.transform.localPosition);
            line.SetPosition(1, endSphere.transform.localPosition);

            distance = (startSphere.transform.position - endSphere.transform.position).magnitude;
            distanceText.text = distance.ToString();

            var textPos = endSphere.transform.position + (startSphere.transform.position - endSphere.transform.position) / 2;
            distanceText.transform.position = textPos + new Vector3(0, 2, 0);
        }

        
    }
}
