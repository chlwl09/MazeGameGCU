using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeItem : MonoBehaviour
{
    private GameObject player;
    private bool isFrozen = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !isFrozen)
        {
            StartCoroutine(FreezePlayer());
        }
    }

    IEnumerator FreezePlayer()
    {
        isFrozen = true;
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            yield return new WaitForSeconds(2f);
            playerMovement.enabled = true;
        }
        isFrozen = false;
        Destroy(gameObject);
    }
}
