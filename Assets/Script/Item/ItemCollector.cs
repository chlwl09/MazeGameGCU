using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public GameObject clearObject; 
    public Text hintText; 
    private bool isHintActive = false;

    void Start()
    {
        hintText.enabled = false; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) 
        {
            Destroy(other.gameObject);
            if (!isHintActive)
            {
                StartCoroutine(ShowClearObjectLocation());
            }
        }
    }

    IEnumerator ShowClearObjectLocation()
    {
        isHintActive = true;
        Vector3 clearObjectPosition = clearObject.transform.position;
        hintText.text = "Game Clear Object is at: " + clearObjectPosition;
        hintText.enabled = true;

        yield return new WaitForSeconds(3);

        hintText.enabled = false;
        isHintActive = false;
    }
}
