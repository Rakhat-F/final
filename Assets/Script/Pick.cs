using UnityEngine;

public class Pick : MonoBehaviour
{
    private Camera mainCamera;
    private Transform targetForTakenObjects;
    private GameObject pointer;
    private GameObject takenObject;
    private RaycastHit hit;
    private float distanceMultiplier;
    private Vector3 scaleMultiplier;
    private LayerMask layerMask = ~(1 << 8);
    private float cameraHeight = 0;
    private float cosine;
    private float positionCalculation;
    private float lastPositionCalculation = 0;
    private Vector3 lastHitPoint = Vector3.zero;
    private Vector3 lastRotation = Vector3.zero;
    private float rayMaxRange = 1000f;
    private bool isRayTouchingSomething = true;
    private float lastRotationY;
    private Vector3 lastHit = Vector3.zero;
    private Vector3 centerCorrection = Vector3.zero;
    private float takenObjSize = 0;
    private int takenObjSizeIndex = 0;
    private float maxScale = 10;
    private float rotationX;
    GameObject Capsule;
    private Ray ray;

    void Start()
    {
        Capsule = GameObject.Find("Capsule");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        targetForTakenObjects = GameObject.Find("Destination").transform;
        
        
    }

    void Update()
    {
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.yellow);
        
        isRayTouchingSomething = Physics.Raycast(ray, out hit, rayMaxRange, layerMask);
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && isRayTouchingSomething)
        {
            if (hit.transform.tag == "Getable")
            {
                takenObject = hit.transform.gameObject;

                distanceMultiplier = Vector3.Distance(mainCamera.transform.position, takenObject.transform.position);
                scaleMultiplier = takenObject.transform.localScale;
                lastRotation = takenObject.transform.rotation.eulerAngles;
                lastRotationY = lastRotation.y - mainCamera.transform.eulerAngles.y;
                takenObject.transform.transform.parent = targetForTakenObjects;


                takenObject.GetComponent<Rigidbody>().isKinematic = true;   

                foreach (Collider col in takenObject.GetComponents<Collider>())
                {
                    col.isTrigger = true;
                }

                if (takenObject.GetComponent<MeshRenderer>() != null)
                {
                    takenObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    takenObject.GetComponent<MeshRenderer>().receiveShadows = true;
                }
                takenObject.gameObject.layer = 8;
                foreach (Transform child in takenObject.GetComponentsInChildren<Transform>())
                {
                    takenObject.GetComponent<Rigidbody>().isKinematic = true;
                    takenObject.GetComponent<Collider>().isTrigger = true;
                    child.gameObject.layer = 8;
                }

                takenObjSize = takenObject.GetComponent<Collider>().bounds.size.y;
                takenObjSizeIndex = 1;
                if (takenObject.GetComponent<Collider>().bounds.size.x > takenObjSize)
                {
                    takenObjSize = takenObject.GetComponent<Collider>().bounds.size.x;
                    takenObjSizeIndex = 0;
                }
                if (takenObject.GetComponent<Collider>().bounds.size.z > takenObjSize)
                {
                    takenObjSize = takenObject.GetComponent<Collider>().bounds.size.z;
                    takenObjSizeIndex = 2;
                }
            }  
            
        }

        if (Input.GetKey(KeyCode.E) || Input.GetMouseButton(0))
        {
            if (takenObject != null)
            {
                if (takenObject.GetComponent<MeshRenderer>() != null)
                {
                    centerCorrection = takenObject.transform.position - takenObject.GetComponent<MeshRenderer>().bounds.center;
                }

                if (Input.GetMouseButton(1))
                {
                    
                    takenObject.transform.rotation = Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse Y"), 0));
                }
                
                takenObject.transform.position = Vector3.Lerp(takenObject.transform.position, targetForTakenObjects.position + centerCorrection, Time.deltaTime * 100);

                takenObject.GetComponent<Collider>().isTrigger = true;
                cosine = Vector3.Dot(ray.direction, hit.normal);
                cameraHeight = Mathf.Abs(hit.distance * cosine);

                
                takenObjSize = takenObject.GetComponent<Collider>().bounds.size[takenObjSizeIndex];

                positionCalculation = (hit.distance * takenObjSize / 6) / (cameraHeight);       
                
                if (positionCalculation < rayMaxRange)
                {
                    lastPositionCalculation = positionCalculation;
                }

                if (isRayTouchingSomething)
                {
                    lastHitPoint = hit.point;
                }
                else
                {
                    lastHitPoint = mainCamera.transform.position + ray.direction * rayMaxRange;
                }

                targetForTakenObjects.position = Vector3.Lerp(targetForTakenObjects.position, lastHitPoint
                        - (ray.direction * lastPositionCalculation), Time.deltaTime * 10000000000);

                takenObject.transform.localScale = scaleMultiplier * (Vector3.Distance(mainCamera.transform.position, takenObject.transform.position) / distanceMultiplier);
            }
        }

        if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(0))
        {
            if (takenObject != null)
            {
                takenObject.GetComponent<Rigidbody>().isKinematic = false;
                foreach (Collider col in takenObject.GetComponents<Collider>())
                {
                    col.isTrigger = false;
                }
                if (takenObject.GetComponent<MeshRenderer>() != null)
                {
                    takenObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    takenObject.GetComponent<MeshRenderer>().receiveShadows = true;
                }
                takenObject.transform.parent = null;
                takenObject.gameObject.layer = 0;
                foreach (Transform child in takenObject.GetComponentsInChildren<Transform>())
                {
                    takenObject.GetComponent<Rigidbody>().isKinematic = false;
                    takenObject.GetComponent<Collider>().isTrigger = false;
                    child.gameObject.layer = 0;
                }
                takenObject = null;
            }
        }
    }
}