using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetVFX : VFXInstantiator
{
    private PlayerInteraction playerInteraction;
    private Carrello carrello;
    private GameObject instantiatedVFX;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInteraction = player.GetComponent<PlayerInteraction>();
        }
        carrello = GetComponent<Carrello>();
    }

    void Update()
    {
        if (playerInteraction == null || carrello == null)
        {
            return;
        }

        if (playerInteraction.magnetismON)
        {
            if (carrello.isCarrelloInHand == true)
            {
                if (instantiatedVFX == null)
                {
                    instantiatedVFX = Instantiate(Prefab, placeToPSawn.position, placeToPSawn.rotation);
                    instantiatedVFX.transform.SetParent(placeToPSawn, true); 
                }

                if (instantiatedVFX != null)
                {
                    instantiatedVFX.transform.position = placeToPSawn.position;
                    instantiatedVFX.transform.rotation = placeToPSawn.rotation;
                }
            }
            else
            {
                if (instantiatedVFX != null)
                {
                    Destroy(instantiatedVFX);
                    instantiatedVFX = null;
                }
            }
        }
        else
        {
            if (instantiatedVFX != null)
            {
                Destroy(instantiatedVFX);
                instantiatedVFX = null;
            }
        }
    }
}
