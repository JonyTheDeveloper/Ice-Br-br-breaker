using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenManager : MonoBehaviour
{

    public int tweenTime;

    public void tweenIn(GameObject gameObject)
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, new Vector3(1, 1, 1), tweenTime).setEase(LeanTweenType.easeOutElastic);
    }

    /*public void tweenOut()
    {
        LeanTween.cancel(this.GameObject);

        LeanTween.scale(this, new Vector3(0, 0, 0), tweenTime).setEase(LeanTweenType.easeOutElastic);
    }*/
}
