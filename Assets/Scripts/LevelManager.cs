using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {

    }

    public void LoadLevel( int index ) {
        SceneManager.LoadScene( index );
    }

    public void NextLevel() {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
    }

    public void PrevLevel() {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex - 1 );
    }
}
