using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
public class Ak : MonoBehaviour
{
    public float damage = 10f;          // Damage per shot
    public float range = 100f;          // Maximum range of the rifle
    public float fireRate = 12f;        // Shots per second
    public float recoil = 0.1f;         // Recoil amount per shot
    public float reloadTime = 2f;       // Time to reload the rifle
    public int magazineSize = 30;       // Number of bullets in a magazine
    public float timeToAim = 0.25f;     // Time to aim down the sight
    public Transform bulletSpawn;       // Position where bullets are spawned

    public AudioClip fireSound;         // Sound effect when firing the rifle
    public AudioClip reloadSound;       // Sound effect when reloading the rifle
    public Camera mainCamera;           // Camera component of the player

    private int currentAmmo;            // Current bullets in the magazine
    private bool isReloading = false;   // Whether the rifle is currently reloading
    public bool isAiming;      // Whether the player is aiming down the sight
    private float nextTimeToFire = 0f;  // Time until the next shot can be fired  

    public GameObject bulletPrefab;
    public float bulletSpeed;

    public VisualEffect muzzleFlashEffect;
    // Start is called before the first frame update

    [SerializeField] private TrailRenderer bulletTrail;


    void Start()
    {
        gameObject.SetActive(false);
        currentAmmo = magazineSize;
        //  originalPosition = transform.localPosition;
        // aimPosition = new Vector3(originalPosition.x - 0.1f, originalPosition.y - 0.025f, originalPosition.z + 0.3f);
    }

    // Update is called once per frame
    void Update()
    {


        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime / timeToAim);
            isAiming = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isAiming = false;
        }
        //else
        //{
        //    transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime / timeToAim);
        //    isAiming = false;
        //}

        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            muzzleFlashEffect.Play();
        }
    }

    void Shoot()
    {
        // Play sound effect
        //  AudioSource.PlayClipAtPoint(fireSound, mainCamera.transform.position);

        // Instantiate muzzle flash effect
        // Instantiate(muzzleFlash, bulletSpawn.position, bulletSpawn.rotation);

        // Spawn bullet casing prefab



        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawn.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));

            Debug.Log(hit.collider.gameObject.name);



            // If the raycast hits an object with a Health script, deal damage
            //// Health targetHealth = hit.transform.GetComponent<Health>();
            // if (targetHealth != null)
            // {
            //     targetHealth.TakeDamage(damage);
            // }
        }
        // Decrease ammo count
        currentAmmo--;
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        // Play reload sound effect
        //  AudioSource.PlayClipAtPoint(reloadSound, mainCamera.transform.position);

        // Wait for reload time
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;
    }
}