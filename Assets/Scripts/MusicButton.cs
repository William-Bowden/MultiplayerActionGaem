using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    [SerializeField]
    MusicPlayer mp;

    // Start is called before the first frame update
    void Start() {
        mp = GameObject.Find( "Music Player" ).GetComponent<MusicPlayer>();

        Button musicButton = GetComponent<Button>();

        musicButton.onClick.AddListener( mp.NextSong );
    }
}
