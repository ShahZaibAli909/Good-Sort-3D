using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    private Camera mainCamera;

    // Layer mask for ignoring specific layers
    public LayerMask ignoreRaycastLayerMask;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        PerformRaycast();
    }

    private void PerformRaycast()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform raycast, excluding the layers defined in ignoreRaycastLayerMask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreRaycastLayerMask))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            // Add your interaction logic here
        }
    }
}
