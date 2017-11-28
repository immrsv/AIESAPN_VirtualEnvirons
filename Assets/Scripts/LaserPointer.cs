using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    // 1
    public GameObject laserPrefab;
    // 2
    private GameObject laser;
    // 3
    private Transform laserTransform;
    // 4
    private Vector3 hitPoint;

    // 1
    public Transform cameraRigTransform;
    // 2
    public GameObject teleportReticlePrefab;
    // 3
    private GameObject reticle;
    // 4
    private Transform teleportReticleTransform;
    // 5
    public Transform headTransform;
    // 6
    public Vector3 teleportReticleOffset;
    // 7
    public LayerMask teleportMask;
    // 8
    private bool shouldTeleport;

    public Color TargetValid;
    public Color TargetInvalid;
    public bool RaycastIgnoresObstacles = false;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void ShowLaser(RaycastHit hit)
    {
        // 1
        laser.SetActive(true);
        // 2
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hit.point, .5f);
        // 3
        laserTransform.LookAt(hit.point);
        // 4
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);


        // Update Line Renderer Positions:
        var lr = laser.GetComponent<LineRenderer>();

        var posns = new Vector3[lr.positionCount];
        var posnCount = lr.GetPositions( posns);

        posns[0] = trackedObj.transform.position;
        posns[1] = hit.point;

        lr.SetPositions(posns);

        laser.GetComponent<MeshRenderer>().material.color = shouldTeleport ? TargetValid : TargetInvalid;
        
    }

    private void Teleport()
    {
        // 1
        shouldTeleport = false;
        // 2
        reticle.SetActive(false);
        // 3
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        // 4
        difference.y = 0;
        // 5
        cameraRigTransform.position = hitPoint + difference;
    }

    // Use this for initialization
    void Start () {

        // 1
        laser = Instantiate(laserPrefab);
        // 2
        laserTransform = laser.transform;
        // 1
        reticle = Instantiate(teleportReticlePrefab);
        // 2
        teleportReticleTransform = reticle.transform;

    }
	
	// Update is called once per frame
	void Update () {

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hitInfo;


            bool hit;
            bool hitValid = false;

            if ( RaycastIgnoresObstacles)
                hit = Physics.Raycast(trackedObj.transform.position, transform.forward, out hitInfo, 100, teleportMask);
            else
                hit = Physics.Raycast(trackedObj.transform.position, transform.forward, out hitInfo, 100.0f);

            // 2
            if (hit) // Hit something
            {
                
                if (((1 << hitInfo.transform.gameObject.layer) & teleportMask) != 0) {
                    hitValid = true;

                    // Hit 'can teleport' target
                    hitPoint = hitInfo.point;

                    shouldTeleport = true;

                    ShowLaser(hitInfo);

                    reticle.SetActive(true);
                    // 2
                    teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                    // 3
                }
            }

            if (!hitValid) {

                //hitPoint = trackedObj.transform.TransformPoint(Vector3.forward * 300);
                if (!hit) {
                    var distance = 100;
                    hitInfo.point = trackedObj.transform.TransformPoint(Vector3.forward * distance);
                    hitInfo.distance = distance;
                }


                shouldTeleport = false;

                ShowLaser(hitInfo);


                reticle.SetActive(false);
            }
        }
        else // 3
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport)
        {
            Teleport();
        }

    }
}
