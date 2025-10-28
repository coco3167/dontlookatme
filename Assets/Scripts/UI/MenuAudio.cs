using UnityEngine;

namespace UI
{
    public class MenuAudio : MonoBehaviour
    {
        public static MenuAudio Instance;

        [SerializeField] private AudioSource feedbackUI;
        [SerializeField] private AudioSource mainMenuMusic;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
        
            Instance = this;
        }

        public void PlayFeedbackUI()
        {
            feedbackUI.Play();
        }

        public void PlayMainMenuMusic()
        {
            mainMenuMusic.Play();
        }

        public void StopMainMenuMusic()
        {
            mainMenuMusic.Stop();
        }
    }
}
