using System.Collections;
using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    float spinDuration = 0.15f;
    Vector3 spinAngles = new Vector3(0, -180, 0);

    private bool resetAnimations = false;
    private bool isSpinning = false;


    public void ResetAnimations()
    {
        if (isSpinning) {
            resetAnimations = true;
        }
    }

    public void Spin()
    {
        StartCoroutine(SpinCoroutine(spinDuration, spinAngles));
    }
    
    private IEnumerator SpinCoroutine(float duration, Vector3 angles)
    {
        if (isSpinning) {
            yield break;
        }
        isSpinning = true;

        Quaternion fromAngle = transform.rotation;
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + angles);
        for (float t = 0f; t < 1f; t += Time.deltaTime / duration) {
            if (resetAnimations) {
                resetAnimations = false;
                break;
            }
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }

        isSpinning = false;
        transform.localRotation = Quaternion.identity;
    }
}
