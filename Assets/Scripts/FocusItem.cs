using System;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(AudioSource))]
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

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    void OnDrawGizmosSelected()
    {
        
        if (this.is_foccused)
		{
            Gizmos.color = new Color(1f, 0f, 0f, .5f);
		} else {
            Gizmos.color = new Color(0f, 1f, 0f, .5f);
		}

        float scale = (mainCamera == null) ? 1 : Vector3.Distance(mainCamera.transform.position, transform.position) * 0.1f;
        Gizmos.DrawSphere(transform.position, scale);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //objRenderer = GetComponent<Renderer>();
        objCollider = GetComponent<Collider>();
        linecastDetectLayerMask = ~ (1 << 7);// all layers excepted the 7

        mainCamera = Camera.main;
        globalCameraFocus = mainCamera.GetComponent<FocusCamera>();
        mainPlayer = globalCameraFocus.GetPlayer();

        Debug.Log($"start item [{this.name}] that can be foccused");
    }

    // Update is called once per frame
    void Update()
    {
        this.is_foccused = CheckFocus();

        if (this.is_foccused) globalCameraFocus.AddFocusItemFrame(this);
    }

    bool CheckFocus()
    {
        //if (!objRenderer.isVisible)
        //    return;
        
        if (GameManager.ItemToFear != itemId)
            return false;

        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        if (!GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
            return false;

        Debug.DrawLine(
            mainPlayer.transform.position,
            transform.position,
            Color.green
            );
        RaycastHit hitInfo;
        if (Physics.Linecast(mainPlayer.transform.position, transform.position, out hitInfo, linecastDetectLayerMask))
            return false;

        return true;
	}

    public AudioSource GetAudioSource()
    {
        return m_audioSource;
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