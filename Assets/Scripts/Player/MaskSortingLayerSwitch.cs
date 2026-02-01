using UnityEngine;

public class MaskSortingLayerSwitch : MonoBehaviour
{
    private Camera camera;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        camera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 viewingVector = camera.transform.position - transform.position;

        if(Vector3.Dot(viewingVector, transform.forward) >= 0)
        {
            spriteRenderer.sortingLayerName = "MaskFrontface";
        }
        else
        {
            spriteRenderer.sortingLayerName = "MaskBackface";
        }
    }
}
