using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MainMusicManager : MonoBehaviour
{
    public static MainMusicManager Instance;

    private AudioSource m_audioSource;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    public void PlayMusic()
    {
        m_audioSource.Play();
    }
}
