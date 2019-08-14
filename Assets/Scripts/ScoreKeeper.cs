using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    int leftScore;
    int rightScore;

    [SerializeField]
    Text leftScorePanel;
    [SerializeField]
    Text rightScorePanel;

    // Start is called before the first frame update
    void Start() {
        leftScore = 0;
        rightScore = 0;
    }

    public void UpdateScore( int team ) {
        if( team == 0 ) {
            leftScorePanel.text = ( ++leftScore ).ToString();
        }
        else if( team == 1 ) {
            rightScorePanel.text = ( ++rightScore ).ToString();
        }
    }

}
