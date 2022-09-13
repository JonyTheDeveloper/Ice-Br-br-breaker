using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardRotation : MonoBehaviour
{
    private void OnEnable()
    {
        
    }

    private void Start()
    {
        LeanTween.moveY(gameObject, -0.5f, 1f).setLoopPingPong();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 100f, 0f) * Time.deltaTime);
    }
}
