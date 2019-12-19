using UnityEngine;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    LevelManager lm;

    // Start is called before the first frame update
    void Start() {
        lm = GameObject.Find( "LevelManager" ).GetComponent<LevelManager>();

        Button homeButton = GetComponent<Button>();

        homeButton.onClick.AddListener( LoadHome );
    }

    void LoadHome() {
        lm.LoadLevel( 0 );
    }
}
