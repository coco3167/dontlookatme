using System;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FocusCamera : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;

    public float dropOutZoomProportion = .5f;
    public float dropOutContrastScore = 1000;
    public float dropOutDarknessProportion = .5f;
    
    public double dropOutCancelSpeed = .5;
    public double dropOutSpeed = 2;
    double dropOutProgress = 0;
    bool focusedFrame = false;
    FocusItem frameFocusedItem;
    FocusItem actualFocusedItem;
    GameObject playerObject;

    UnityEngine.Rendering.Universal.Vignette globalVolumeVignette;
    UnityEngine.Rendering.Universal.ColorAdjustments globalVolumeColor;

    public ItemType selectedItemType;

    private AudioSource m_audioSource;
    private bool m_losing = false;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_losing = false;
        playerObject = this.transform.parent.gameObject;
        UnityEngine.Rendering.VolumeProfile volumeProfile = globalVolume.profile;
        if(!volumeProfile.TryGet(out globalVolumeVignette)) throw new System.NullReferenceException(nameof(globalVolumeVignette));
        if(!volumeProfile.TryGet(out globalVolumeColor)) throw new System.NullReferenceException(nameof(globalVolumeColor));

        UpdateDropEffect();
    }

    // Update is called once per frame
    void Update()
    {
        //!GameManager.GameStarted || 
        if (m_losing)
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
        UnityEngine.Debug.Log($"GAME DYING");
        this.LoseGame();//idk why but not calling this nusisnirgirngunguiozrge
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
            transform.localPosition = UnityEngine.Vector3.Lerp(startPos, endPos, progress * dropOutZoomProportion);
        }
    }

    private IEnumerator LoseGame()
    {
        UnityEngine.Debug.Log($"GAME IS LOOSEN");

        actualFocusedItem.GetAudioSource().Play();
        m_losing = true;
        while (actualFocusedItem.GetAudioSource().isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        
        m_audioSource.Play();
        while (m_audioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        } 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
