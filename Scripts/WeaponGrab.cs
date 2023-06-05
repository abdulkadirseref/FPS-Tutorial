using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponGrab : MonoBehaviour
{
    public float raycastDistance;
    private bool hasPickedUpWeapon = false;
    public GameObject gun;
    

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (Input.GetKeyDown(KeyCode.E) && hit.collider.CompareTag("Gun") && !hasPickedUpWeapon)
            {
                hit.collider.gameObject.SetActive(false);
                gun.SetActive(true);              
                // do the pickup action here
                hasPickedUpWeapon = true;
            }
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);
        }
    }
}
