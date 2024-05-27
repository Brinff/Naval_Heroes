using Code.UI.Components;
using DG.Tweening;
using Game.Grid.Auhoring;
using Game.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

public class SlotBattleGrid : MonoBehaviour, ISlotPopulate, IItemBeginDrag, IItemEndDrag, ISlotRenderer
{
    public GridAuhoring grid;
    public SlotCollection collection { get; private set; }
    public List<SlotItem> items { get; private set; } = new List<SlotItem>();

    public int id => name.GetDeterministicHashCode();

    public GridRendererAuthoring gridHighlight;
    public GridRendererAuthoring gridField;
    public GridRendererAuthoring gridReject;
    public GridRendererAuthoring gridCurrent;
    public GridRendererAuthoring gridNew;

    public void Prepare(SlotCollection collection)
    {
        this.collection = collection;

        grid = GetComponent<GridAuhoring>();
        gridField.BeginFill(grid.scale, grid.center);
        gridHighlight.BeginFill(grid.scale, grid.center);
        foreach (var item in grid.rects)
        {
            gridHighlight.AddRect(item.position, item.size);
            gridField.AddRect(item.position, item.size);
        }
        gridField.EndFill();
        gridHighlight.EndFill();

        gridHighlight.SetAlpha(0);
    }

    public bool AddItem(SlotItem item, Vector3 position)
    {
        if (items.Contains(item)) return false;

        item.parentSlot = this;
        item.targetPosition = position;
        item.transform.position = position;

        items.Add(item);
        collection.SetDirty();
        UpdateCurrentGrid();

        return true;
    }

    public bool RemoveItem(SlotItem slotItem)
    {
        if (items.Remove(slotItem))
        {
            collection.SetDirty();
            UpdateCurrentGrid();
            return true;
        }
        return false;
    }

    private void UpdateCurrentGrid()
    {
        gridCurrent.BeginFill(grid.scale, grid.center);
        Matrix4x4 worldToLocal = grid.GetWorldToLocalMatrix();
        foreach (var currentItem in items)
        {
            foreach (var itemRect in currentItem.grid.rects)
            {
                RectInt projectRect = currentItem.grid.GetProjectRect(itemRect, currentItem.grid.GetLocalToWorldMatrix(), worldToLocal).GetRoundedRect();
                gridCurrent.AddRect(projectRect.position, projectRect.size);
            }
        }

        gridCurrent.EndFill();
    }

    public bool Populate(Ray ray, SlotItem slotItem, out Vector3 position)
    {
        Plane plane = new Plane(Vector3.up, 0);
        position = Vector3.zero;
        if (plane.Raycast(ray, out float distance))
        {
            position = ray.GetPoint(distance);
            gridNew.BeginFill(grid.scale, grid.center);
            gridReject.BeginFill(grid.scale, grid.center);
            bool isOverlap = false;
            bool isIntersect = false;
            Matrix4x4 worldToLocal = grid.GetWorldToLocalMatrix();
            Matrix4x4 localToWorld = grid.GetLocalToWorldMatrix();

            for (int i = 0; i < grid.rects.Length; i++)
            {
                Rect rect = grid.rects[i].GetSignleRect();

                foreach (var itemRect in slotItem.grid.rects)
                {
                    Rect projectRect = slotItem.grid.GetProjectRect(itemRect, Matrix4x4.Translate(position) * slotItem.grid.GetMatrix(), worldToLocal);
                    if (rect.Overlaps(projectRect))
                    {
                        var clampedRect = projectRect.GetClampRect(rect).GetRoundedRect();
                        gridNew.AddRect(clampedRect.position, clampedRect.size);

                        position = localToWorld.MultiplyPoint((Vector2)clampedRect.position) + slotItem.grid.GetOffset();

                        foreach (var otherItem in items)
                        {
                            if (otherItem == slotItem) continue;
                            foreach (var otherRect in otherItem.grid.rects)
                            {
                                RectInt projectOtherRect = otherItem.grid.GetProjectRect(otherRect, otherItem.grid.GetLocalToWorldMatrix(), worldToLocal).GetRoundedRect();
                                if (clampedRect.Overlaps(projectOtherRect))
                                {
                                    var intersectRect = clampedRect.GetIntersectRect(projectOtherRect);
                                    gridReject.AddRect(intersectRect.position, intersectRect.size);

                                    isIntersect = true;
                                }                               
                            }
                        }

                        isOverlap = true;
                    }
                }
            }

            gridNew.EndFill();
            gridReject.EndFill();

            return isOverlap && !isIntersect;
        }
        return false;
    }

    public void ItemBeginDrag(SlotItem slotItem)
    {
        gridHighlight.SetAlpha(1);
        //gridField.SetColor(m_ColorField * m_HighlightActive);
    }

    public void ItemEndDrag(SlotItem slotItem)
    {
        gridHighlight.SetAlpha(0);
        //gridField.SetColor(m_ColorField * m_HighlightDefault);
        gridNew.Clear();
        gridReject.Clear();
    }

    public void Show(bool immediately)
    {
        var sequence = DOTween.Sequence(this);
        sequence.Join(gridField.DoAlpha(1, 0.15f));
        sequence.Join(gridCurrent.DoAlpha(1, 0.15f));
        sequence.Join(gridNew.DoAlpha(1, 0.15f));
        sequence.Join(gridReject.DoAlpha(1, 0.15f));

        if (immediately)
        {
            sequence.Complete(true);
        }
    }

    public void Hide(bool immediately)
    {
        var sequence = DOTween.Sequence(this);

        sequence.Join(gridField.DoAlpha(0, 0.15f));
        sequence.Join(gridCurrent.DoAlpha(0, 0.15f));
        sequence.Join(gridNew.DoAlpha(0, 0.15f));
        sequence.Join(gridReject.DoAlpha(0, 0.15f));

        if (immediately)
        {
            sequence.Complete(true);
        }
    }

    public bool AddItemPossible(SlotItem item, Vector3 position)
    {
        if (items.Contains(item)) return false;
        return true;
    }

    public bool RemoveItemPossible(SlotItem slotItem, Vector3 position)
    {
        return true;
    }
}
