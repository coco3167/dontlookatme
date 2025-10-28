using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FocusCamera : MonoBehaviour
{
    public double dropOutSpeed = 1;
    double dropOutProgress = 0;
    GameObject focusedItem;
    GameObject lastFocusedItem;

    Image effectAlphaImage; 

    public ItemType selectedItemType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        effectAlphaImage = this.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
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
        effectAlphaImage.color = new(0f, 0f, 0f, (float)dropOutProgress);
	}
    
    public void AddFocusItemFrame(GameObject itemFocused)
    {
        Debug.DrawLine(itemFocused.transform.position, transform.position, Color.red);
        focusedItem = itemFocused;
	}
}
