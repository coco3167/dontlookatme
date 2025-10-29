using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class FocusItem : MonoBehaviour
{
    public ItemType itemId;

    //Renderer objRenderer;
    Collider objCollider;
    int linecastDetectLayerMask;

    bool is_foccused = false;

    UnityEngine.Plane[] planes;
    Camera mainCamera;
    GameObject mainPlayer;
    FocusCamera globalCameraFocus;

    void OnDrawGizmosSelected()
    {
        if (is_foccused)
            Gizmos.color = new Color(1f, 0f, 0f, .5f);
        else
            Gizmos.color = new Color(0f, 0f, 0f, .5f);
            
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(UnityEngine.Vector3.zero, UnityEngine.Vector3.one);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //objRenderer = GetComponent<Renderer>();
        objCollider = GetComponent<Collider>();
        linecastDetectLayerMask = ~ 1 << 7;// all layers excepted the 7

        mainCamera = Camera.main;
        globalCameraFocus = mainCamera.GetComponent<FocusCamera>();
        mainPlayer = globalCameraFocus.GetPlayer();

        Debug.Log($"start item [{this.name}] that can be foccused");
    }

    // Update is called once per frame
    void Update()
    {
        is_foccused = false;

        //if (!objRenderer.isVisible)
        //    return;

        if (globalCameraFocus.selectedItemType != itemId)
            return;

        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        if (!GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
            return;

        Debug.DrawLine(
            mainPlayer.transform.position,
            transform.position,
            Color.yellow
            );
        RaycastHit hitInfo;
        if (Physics.Linecast(mainPlayer.transform.position, transform.position, out hitInfo, linecastDetectLayerMask))
            return;

        is_foccused = true;
        globalCameraFocus.AddFocusItemFrame(gameObject);
    }
}

public enum ItemType
{
    FLOWERPOT,
    GIRAFFE,
    TRUMPET,
    CAR,
    SAW,
    DINOSAUR,
}