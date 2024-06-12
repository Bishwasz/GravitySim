using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public GameObject cBodyPrefab; // we'll instantiate this prefab as our body mass
    public GravityManager gravityManager; // reference to the GravityManager
    private GameObject currentCBody; // to track the current CBody being placed

    public TrajectoryHandler trajectoryHandler; // Reference to the TrajectoryHandler component
    private Vector3 initialWorldPosition; // Initial world position when click is pressed
    private bool isDragging = false; // Flag to track if the mouse is being dragged


    public Button addButton;
    public Button deleteButton;
    private ButtonHoverHandler addButtonHoverTracker;
    private ButtonHoverHandler deleteButtonHoverTracker;

    void Start()
    {
        // Assign the hover tracker from the buttons
        addButtonHoverTracker = addButton.GetComponent<ButtonHoverHandler>();
        deleteButtonHoverTracker = deleteButton.GetComponent<ButtonHoverHandler>();
        if (addButtonHoverTracker == null)
        {
            Debug.LogError("ButtonHoverHandler component not found on Add Button.");
        }

        if (deleteButtonHoverTracker == null)
        {
            Debug.LogError("ButtonHoverHandler component not found on Delete Button.");
        }
    }

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

            IAttractee cbody = currentCBody.GetComponent<CBody>();

            ColorHandler colorHandler = currentCBody.GetComponent<ColorHandler>();
            if (colorHandler != null) colorHandler.Initialize(cbody != null ? cbody.mass : 0.5f);

            trajectoryHandler.EnableTrajectory();

            // Disable camera dragging
            CameraMove.IsCameraDragDisabled = true;
        }
    }

    void Update()
    {
        if (currentCBody != null)
        {
            // Check if any button is hovered
            bool isAnyButtonHovered = (addButtonHoverTracker.IsButtonHovered()) ||(deleteButtonHoverTracker.IsButtonHovered());

            if (isAnyButtonHovered)
            {
                // Hide the current CBody if any button is hovered
                currentCBody.SetActive(false);
            }
            else
            {
                // Show the current CBody if no button is hovered
                currentCBody.SetActive(true);

                Vector3 mousePosition = Input.mousePosition;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition); // turn mouse pos to screen pos
                worldPosition.z = 0; // ensure proper depth

                if (!isDragging) currentCBody.transform.position = worldPosition; // set position of instantiated object

                if (Input.GetMouseButtonDown(0))
                {
                    initialWorldPosition = worldPosition;
                    isDragging = true;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (isDragging)
                    {
                        Vector3 drag = initialWorldPosition - worldPosition;
                        Vector2 initialVelocity = GetInitialVelocityFromDrag(drag);

                        CBody newCBody = currentCBody.GetComponent<CBody>();
                        newCBody.velocity = initialVelocity;
                        gravityManager.AddAttractee(newCBody);
                        gravityManager.AddAttractor(newCBody);

                        // enable the collider after placement
                        Collider2D collider = currentCBody.GetComponent<Collider2D>();
                        if (collider != null) collider.enabled = true;

                        // enable trails
                        TrailHandler trailHandler = currentCBody.GetComponent<TrailHandler>();
                        if (trailHandler != null) trailHandler.EnableTrail();

                        isDragging = false;
                        currentCBody = null;
                        trajectoryHandler.ClearTrajectory();
                        trajectoryHandler.DisableTrajectory();

                        // Enable camera dragging
                        CameraMove.IsCameraDragDisabled = false;
                    }
                }

                // Update trajectory if dragging
                if (isDragging)
                {
                    // Calculate the drag
                    Vector3 drag = initialWorldPosition - worldPosition;
                    Vector2 initialVelocity = GetInitialVelocityFromDrag(drag);

                    List<IAttractor> bodies = gravityManager.attractors;

                    trajectoryHandler.DrawTrajectory(
                        initialWorldPosition,
                        1f,
                        initialVelocity,
                        bodies,
                        0.01f, // Time step
                        150 // Number of steps
                    );
                }
            }
        }
    }

    private Vector2 GetInitialVelocityFromDrag(Vector3 drag) => drag * 0.5f; // Adjust as needed
}