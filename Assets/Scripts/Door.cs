using System;
using System.Collections;
using System.Reflection;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorPivot;
    [SerializeField] private AudioSource winningSource, doorSource, openingSource;
    [SerializeField] private GameObject[] endObjs;

    private bool m_opened;

    private void Awake()
    {
        foreach (GameObject obj in endObjs)
        {
            obj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!GameManager.GameEnded)
            return;
        
        if (other.CompareTag("Player"))
        {
            StartCoroutine(WaitForRestart());
            GameManager.GameEnded = true;
        }
    }

    private IEnumerator WaitForRestart()
    {
        endObjs[(int)GameManager.ItemToFear].SetActive(true);

        openingSource.Play();
        while (openingSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        
        doorSource.Play();
        bool doorOpened = false;
        Tweener tweener = doorPivot.DOLocalRotate(new Vector3(-90, 90, 0), 1);
        tweener.OnComplete(() => doorOpened = true);
        tweener.Play();

        while (doorSource.isPlaying || !doorOpened)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.5f);

        winningSource.Play();
        while (winningSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        
        GameManager.RestartGame();
    }
}
