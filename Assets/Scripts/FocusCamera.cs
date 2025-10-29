using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FocusCamera : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;

    public double dropOutSpeed = 1;
    double dropOutProgress = 0;
    GameObject focusedItem;
    GameObject lastFocusedItem;

    UnityEngine.Rendering.Universal.Vignette globalVolumeVignette;
    UnityEngine.Rendering.Universal.ColorAdjustments globalVolumeColor;

    public ItemType selectedItemType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.Debug.Log(globalVolume.profile);
        UnityEngine.Rendering.VolumeProfile volumeProfile = globalVolume.profile;
        if(!volumeProfile.TryGet(out globalVolumeVignette)) throw new System.NullReferenceException(nameof(globalVolumeVignette));
        if(!volumeProfile.TryGet(out globalVolumeColor)) throw new System.NullReferenceException(nameof(globalVolumeColor));

        UpdateDropEffect();
    }

    // Update is called once per frame
    void Update()
    {

        if (focusedItem != lastFocusedItem)
        {
            // dropout cancel
            dropOutProgress = 0;
            UpdateDropEffect();
        }

        if (!focusedItem) return;

        dropOutProgress += Time.deltaTime * dropOutSpeed;

        if (dropOutProgress < 1)
        {
            // dropout frame
            lastFocusedItem = focusedItem;
            focusedItem = null;
            UpdateDropEffect();
            return;
        }

        // dropout finish
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void UpdateDropEffect()
    {
        float progress = (float)dropOutProgress;
        globalVolumeColor.contrast.Override(100f + progress * 100);
        globalVolumeColor.colorFilter.Override(new Color(1 - progress, 1 - progress, 1 - progress));
	}
    
    public void AddFocusItemFrame(GameObject itemFocused)
    {
        UnityEngine.Debug.DrawLine(itemFocused.transform.position, transform.position, Color.red);
        focusedItem = itemFocused;
	}
}
