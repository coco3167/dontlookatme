using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class FocusCamera : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;

    public float dropOutZoomProportion = .5f;
    public float dropOutContrastScore = 1000;
    public float dropOutDarknessProportion = .5f;
    public float dropOutShakePower = 1;
    
    public double dropOutCancelSpeed = .5;
    public double dropOutSpeed = 2;
    double dropOutProgress = 0;
    bool focusedFrame = false;
    FocusItem frameFocusedItem;
    FocusItem actualFocusedItem;
    GameObject playerObject;

    UnityEngine.Rendering.Universal.Vignette globalVolumeVignette;
    UnityEngine.Rendering.Universal.ColorAdjustments globalVolumeColor;

    //public ItemType selectedItemType;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObject = this.transform.parent.gameObject;
        UnityEngine.Rendering.VolumeProfile volumeProfile = globalVolume.profile;
        if(!volumeProfile.TryGet(out globalVolumeVignette)) throw new System.NullReferenceException(nameof(globalVolumeVignette));
        if(!volumeProfile.TryGet(out globalVolumeColor)) throw new System.NullReferenceException(nameof(globalVolumeColor));

        UpdateDropEffect();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.GameStarted)
            return;
        
        if (!focusedFrame)
        {
            if (dropOutProgress > 0)
			{
                // dropout cancel
                dropOutProgress -= Time.deltaTime * dropOutCancelSpeed;
                if (dropOutProgress <= 0)
                {
                    dropOutProgress = 0;
                    actualFocusedItem = null;
                    frameFocusedItem = null;
                }
                UpdateDropEffect();
			}
            return;
        }

        focusedFrame = false;

        if (dropOutProgress < 1)
        {
            actualFocusedItem = frameFocusedItem;
            dropOutProgress += Time.deltaTime * dropOutSpeed;

            // dropout frame
            UpdateDropEffect();
            return;
        }
        
        // dropout finish
        StartCoroutine(EndSequence());
    }

    private IEnumerator EndSequence()
    {
        GameManager.GameStarted = false;

        actualFocusedItem.GetAudioSource().Play();
        while (actualFocusedItem.GetAudioSource().isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        
        m_audioSource.Play();
        while (m_audioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        
        GameManager.RestartGame();
    }
        
    void UpdateDropEffect()
    {
        // global volume
        float progress = (float)dropOutProgress;
        globalVolumeColor.contrast.Override(100f + progress * dropOutContrastScore);
        float grayProgress = 1f - progress * dropOutDarknessProportion;
        globalVolumeColor.colorFilter.Override(new Color(grayProgress, grayProgress, grayProgress));
        // zoom
        if (progress == 0)
        {
            transform.localPosition = UnityEngine.Vector3.zero;
        }
        else
        {
            UnityEngine.Vector3 startPos = transform.parent.InverseTransformPoint(playerObject.transform.position);
            UnityEngine.Vector3 endPos = transform.parent.InverseTransformPoint(actualFocusedItem.transform.position);
            float rsize = dropOutShakePower * progress;
            UnityEngine.Vector3 randomShake = new(UnityEngine.Random.Range(rsize * -1, rsize), UnityEngine.Random.Range(rsize * -1, rsize), UnityEngine.Random.Range(rsize * -1, rsize));
            transform.localPosition = UnityEngine.Vector3.Lerp(startPos, endPos, progress * dropOutZoomProportion) + randomShake;
        }
    }

    public void AddFocusItemFrame(FocusItem itemFocused)
    {
        if (frameFocusedItem != itemFocused) UnityEngine.Debug.Log($"new focused item [{frameFocusedItem}] -> [{itemFocused}]");
        focusedFrame = true;
        UnityEngine.Debug.DrawLine(itemFocused.transform.position, playerObject.transform.position, Color.red);
        frameFocusedItem = itemFocused;
        if (!actualFocusedItem) actualFocusedItem = itemFocused;
    }
        
    public GameObject GetPlayer()
    {
        return this.transform.parent.gameObject;
        //return playerObject;
	}
}
