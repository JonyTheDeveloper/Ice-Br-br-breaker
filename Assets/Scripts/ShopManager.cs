using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ShopManager : MonoBehaviour
{
    //Reference to UIManager
    UIManager UIScript;
    public GameObject manager;

    private bool isPressed = false;
    private float timer;
    private float timeLimit = 0.25f;

    [SerializeField]
    private int cardID;

    private void Start()
    {
        timer = 0;

        //Assign UIManager script reference
        UIScript = manager.GetComponent<UIManager>();
    }

    void Update()
    {
        if (isPressed == true)
        {
            timer += Time.deltaTime;
            if (timer >= timeLimit)
            {
                Reset();
                UIScript.cardPopUp(cardID);
            }
        }
    }

    public void OnPointerDown()
    {
        isPressed = true;
    }

    public void OnPointerUp()
    {
        isPressed = false;
        timer = 0;
    }

    public void Reset()
    {
        isPressed = false;
        timer = 0;
    }
}