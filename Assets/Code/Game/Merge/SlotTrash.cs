using Game.Grid.Auhoring;
using Game.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Networking.UnityWebRequest;

public class SlotTrash : MonoBehaviour, ISlotPopulate, ISlotRenderer
{
    public List<SlotItem> items { get; private set; } = new List<SlotItem>();
    public SlotItem item => items.Count > 0 ? items[0] : null;
    public SlotCollection collection { get; private set; }

    [SerializeField]
    private GridRendererAuthoring gridRenderer;

    public int id => name.GetDeterministicHashCode();

    private BoxCollider[] colliders;

    public bool AddItem(SlotItem slotItem, Vector3 position)
    {
        Destroy(slotItem.entity);
        Destroy(slotItem.gameObject);
        return true;
    }

    public bool RemoveItem(SlotItem slotItem)
    {
        return false;
    }

    public void Prepare(SlotCollection collection)
    {
        this.collection = collection;

        GridAuhoring grid = gameObject.GetComponent<GridAuhoring>();

        var gridRenderer = GetComponent<GridRendererAuthoring>();
        gridRenderer.BeginFill(grid.scale, grid.center);

        colliders = new BoxCollider[grid.rects.Length];
        for (int i = 0; i < grid.rects.Length; i++)
        {
            var sigleRect = grid.GetSingleRect(grid.rects[i]);
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.center = new Vector3(sigleRect.center.x, 0, sigleRect.center.y);
            boxCollider.size = new Vector3(sigleRect.size.x, 5, sigleRect.size.y);
            colliders[i] = boxCollider;
            gridRenderer.AddRect(grid.rects[i].position, grid.rects[i].size);
        }

        gridRenderer.EndFill();
    }

    public void Show(float duration)
    {
        gameObject.SetActive(true);
    }

    public void Hide(float duration)
    {
        gameObject.SetActive(false);
    }

    public bool Populate(Ray ray, SlotItem item, out Vector3 position)
    {
        Plane plane = new Plane(Vector3.up, 0);
        position = Vector3.zero;
        if (plane.Raycast(ray, out float distance))
        {
            position = ray.GetPoint(distance);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].Raycast(ray, out RaycastHit _, 10000))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
