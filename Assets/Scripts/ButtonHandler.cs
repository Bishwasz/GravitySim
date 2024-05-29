using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GameObject cBodyPrefab; // we'll instantiate this prefab as our body mass
    public GravityManager gravityManager; // reference to the GravityManager
    private GameObject currentCBody; // to track the current CBody being placed

    public void OnButtonClick()
    {
        if (cBodyPrefab != null && gravityManager != null)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            spawnPosition.z = 0; // Assuming you want to place it on a 2D plane (z = 0)
            currentCBody = Instantiate(cBodyPrefab, spawnPosition, Quaternion.identity);

            // disable the collider while on select mode
            Collider2D collider = currentCBody.GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false;
            // disable trails
            TrailHandler trailHandler = currentCBody.GetComponent<TrailHandler>();
            if (trailHandler != null) trailHandler.DisableTrail();

        }
    }

    void Update()
    {
        if (currentCBody != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition); // turn mouse pos to screen pos
            worldPosition.z = 0; // ensure proper depth

            currentCBody.transform.position = worldPosition; // set position of instantiated object

            if (Input.GetMouseButtonDown(0))
            {
                CBody newCBody = currentCBody.GetComponent<CBody>();
                gravityManager.AddCBody(newCBody);

                // enable the collider after placement
                Collider2D collider = currentCBody.GetComponent<Collider2D>();
                if (collider != null) collider.enabled = true;

                // enable trails
                TrailHandler trailHandler = currentCBody.GetComponent<TrailHandler>();
                if (trailHandler != null) trailHandler.EnableTrail();


                currentCBody = null;
            }
        }
    }
}


