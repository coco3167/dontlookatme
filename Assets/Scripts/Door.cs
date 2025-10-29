using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorPivot;

    private AudioSource m_audioSource;
    private bool m_opened;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_opened)
            return;
        
        if (other.CompareTag("Player"))
        {
            Tweener tweener = doorPivot.DOLocalRotate(new Vector3(-90, 90, 0), 1);
            tweener.OnComplete(GameWon);
            tweener.Play();
            m_opened = true;
        }
    }

    private void GameWon()
    {
        m_audioSource.Play();
        StartCoroutine(WaitForRestart());
    }

    private IEnumerator WaitForRestart()
    {
        while (m_audioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        GameManager.RestartGame();
    }
}
