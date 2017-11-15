using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeasurementTool : MonoBehaviour {
    public GameObject startSphere;
    public GameObject endSphere;
    public LineRenderer line;
    public float distance;

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

    public SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);


        // Update Line Renderer Positions:
        var lr = laser.GetComponent<LineRenderer>();

        var posns = new Vector3[lr.positionCount];
        var posnCount = lr.GetPositions(posns);

        posns[0] = trackedObj.transform.position;
        posns[1] = hit.point;

        lr.SetPositions(posns);
    }


    void Start () {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }
	
	// Update is called once per frame
	void Update () {
		if(startSphere.activeSelf && endSphere.activeSelf)
        {
            line.SetPosition(0, startSphere.transform.localPosition);
            line.SetPosition(1, endSphere.transform.localPosition);

            distance = (startSphere.transform.position - endSphere.transform.position).magnitude;
            distanceText.text = distance.ToString();

            var textPos = endSphere.transform.position + (startSphere.transform.position - endSphere.transform.position) / 2;
            distanceText.transform.position = textPos + new Vector3(0, 2, 0);
        }

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            RaycastHit hit;

            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
            {
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

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            //move spheres
        }
    }
}
