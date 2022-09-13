using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(byebye());
    }

    IEnumerator byebye()
    {
        yield return new WaitForSeconds(0.05f);
        foreach (Transform child in transform)
        {
            Vector3 force = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            Vector3 rotation = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            child.gameObject.GetComponent<Rigidbody>().AddForce(force * 100);
            child.gameObject.GetComponent<Rigidbody>().AddTorque(rotation * 100);
        }

        Destroy(gameObject, 2f);
    }
}
