using UnityEngine;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour
{
    public Text currentText;
    public Text maximumText;


    public void SetScore(Score score)
    {
        currentText.text = score.current.ToString();
        maximumText.text = string.Format("HIGHSCORE: {0}", score.maximum);
    }
}
