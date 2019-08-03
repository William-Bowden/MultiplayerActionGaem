using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour {
    static MusicPlayer instance = null;

    public static AudioSource musicPlayer;

    //[SerializeField]
    //Slider volSlider;

    [SerializeField]
    AudioClip[] songList;

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

        musicPlayer.clip = songList[ Random.Range( 0, songList.Length ) ];

        if( !musicPlayer.isPlaying ) {
            musicPlayer.Play();
        }

        //if( PlayerPrefs.HasKey( "musicVol" ) ) {
        //    musicPlayer.volume = PlayerPrefs.GetFloat( "musicVol" );
        //    volSlider.value = PlayerPrefs.GetFloat( "musicVol" );
        //}
        //else {
        //    musicPlayer.volume = 0.5f;
        //    volSlider.value = 0.5f;
        //    PlayerPrefs.SetFloat( "musicVol", musicPlayer.volume );
        //    PlayerPrefs.Save();
        //}
    }

    //private void Update() {
    //    if( PlayerPrefs.HasKey( "musicVol" ) ) {

    //        if( musicPlayer.volume != PlayerPrefs.GetFloat( "musicVol" ) ) {
    //            PlayerPrefs.SetFloat( "musicVol", musicPlayer.volume );
    //            PlayerPrefs.Save();
    //        }

    //    }
    //}

}
