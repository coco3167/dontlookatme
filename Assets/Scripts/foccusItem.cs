using System.Numerics;
using UnityEngine;

public class foccusItem : MonoBehaviour
{
    Renderer objRenderer;
    Collider objCollider;
    bool is_foccused = false;

    UnityEngine.Plane[] planes;
    Camera mainCamera;

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
        objRenderer = GetComponent<Renderer>();
        objCollider = GetComponent<Collider>();

        mainCamera = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        Debug.Log($"start item [{this.name}] that can be foccused");
    }

    // Update is called once per frame
    void Update()
    {
        is_foccused = false;

        if (!objRenderer.isVisible)
            return;

        if (!GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
            return;

        Debug.DrawLine(mainCamera.transform.position, transform.position, Color.red);
        //if (Physics.Linecast(mainCamera.transform.position, transform.position))
        //    return;

        is_foccused = true;
    }
}
