using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source -----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------- Audio Clips -----------")]
    public AudioClip background;
    public AudioClip secretEndingMusic;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        HandleSceneMusic(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleSceneMusic(scene.name);
    }

    private void HandleSceneMusic(string sceneName)
    {
        if (musicSource == null) return;


        if (sceneName == "secretEnding")
        {
            if (musicSource.clip != secretEndingMusic)
            {
                musicSource.clip = secretEndingMusic;
                musicSource.Play();
            }
        }
        else
        {

            if (musicSource.clip != background)
            {
                musicSource.clip = background;
                musicSource.Play();
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
