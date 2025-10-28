using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    GameObject focusedItem;
    GameObject lastFoccusedItem; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!focusedItem) return;

        lastFoccusedItem = focusedItem;
        focusedItem = null;
    }
    
    public void AddFocusItemFrame(GameObject itemFocused)
    {
        Debug.DrawLine(itemFocused.transform.position, transform.position, Color.red);
        focusedItem = itemFocused;
	}
}
