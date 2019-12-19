using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance = null;

    public static AudioSource musicPlayer;

    //[SerializeField]
    //Slider volSlider;

    [SerializeField]
    AudioClip[] songList;
    int songIndex = 0;

    void Awake() {
        if( instance != null ) {
            Destroy( gameObject );
        }
        else {
            instance = this;
            GameObject.DontDestroyOnLoad( gameObject );
        }
    }

    private void Start() {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.volume = 0.1f;

        songIndex = Random.Range( 0, songList.Length );
        musicPlayer.clip = songList[ songIndex ];

        if( !musicPlayer.isPlaying ) {
            musicPlayer.Play();
        }
    }

    public void NextSong() {
        if( ++songIndex >= songList.Length ) {
            songIndex = 0;
        }

        musicPlayer.clip = songList[ songIndex ];

        if( !musicPlayer.isPlaying ) {
            musicPlayer.Play();
        }
    }

}
