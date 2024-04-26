using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Slots.DragAndDrop;
using Code.Services;
using Game.Grid.Auhoring;
using Game.UI.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace Code.Game.Slots.Battle
{
    public class BattleFieldMediator : MonoBehaviour,
        IService,
        IInitializable,
        IDropTarget,
        IEnterDragHandler,
        IExitDragHandler,
        IStayDragHandler,
        ISlotBeginDragHandler,
        ISlotEndDragHandler
    {
        private BattleFieldView m_BattleFieldView;
        private DragAndDropService m_DragAndDropService;
        private ItemFactory m_ItemFactory;


        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public void Initialize()
        {
            m_BattleFieldView = ServiceLocator.Get<BattleFieldView>();
            m_DragAndDropService = ServiceLocator.Get<DragAndDropService>();
            m_DragAndDropService.RegisterDropTarget(this);
            
            m_BattleFieldView.newGrid.Clear();
            m_BattleFieldView.currentGrid.Clear();
            m_BattleFieldView.rejectGrid.Clear();
        }
        
        public class GridItemHandler
        {
            public ItemDragHandler ItemDragHandler;
            public GridRect[] GridRects;
            public Vector2 Center;
        }

        private Dictionary<ItemDragHandler, GridItemHandler> m_GridItemHandlers =
            new Dictionary<ItemDragHandler, GridItemHandler>();
        
        public bool Overlap(IDragAndDropHandler dragHandler, out float weight)
        {
            var itemDragHandler = dragHandler as ItemDragHandler;
            if (itemDragHandler == null)
            {
                weight = 0;
                return false;
            }

            return m_BattleFieldView.area.Overlap(itemDragHandler.position, new Rect(0, 0, 10, 10), out weight);
        }

        public void OnStayDragHandler(IDragAndDropHandler dragHandler)
        {
            var itemDragHandler = dragHandler as ItemDragHandler;
            if (itemDragHandler == null) return;
            
            if (m_GridItemHandlers.TryGetValue(itemDragHandler, out GridItemHandler gridItemHandler))
            {
                var localPosition = m_BattleFieldView.newGrid.transform.InverseTransformPoint(itemDragHandler.position);
                foreach (var gridRect in gridItemHandler.GridRects)
                {
                    gridRect.position = Vector2Int.RoundToInt((((Vector2)localPosition + gridItemHandler.Center) - m_BattleFieldView.newGrid.center) / m_BattleFieldView.newGrid.scale);
                    /*m_BattleFieldView.newGrid.RemoveGridRect(gridRect);*/
                }
                itemDragHandler.OnApply -= ApplyDestination;
            }
            
            
            
            /*m_BattleFieldView.newGrid.AddRect();
            
            itemDragHandler.item.entity.GetComponent<Grid>()
            
            itemDragHandler.position */
        }
        
        public void OnExitDragHandler(IDragAndDropHandler dragHandler)
        {
            var itemDragHandler = dragHandler as ItemDragHandler;
            if (itemDragHandler == null) return;
            if (m_GridItemHandlers.TryGetValue(itemDragHandler, out GridItemHandler gridItemHandler))
            {
                foreach (var gridRect in gridItemHandler.GridRects)
                {
                    m_BattleFieldView.newGrid.RemoveGridRect(gridRect);
                }
                itemDragHandler.OnApply -= ApplyDestination;
            }

            m_GridItemHandlers.Remove(itemDragHandler);
            Debug.Log("OnExitDragHandler");
        }

        private void ApplyDestination(ItemDragHandler itemDragHandler)
        {
            
        }

        public void OnEnterDragHandler(IDragAndDropHandler dragHandler)
        {
            var itemDragHandler = dragHandler as ItemDragHandler;
            if (itemDragHandler == null) return;

            var grid = itemDragHandler.item.entity.GetComponent<GridAuhoring>();
            var gridRects = grid.rects
                .Select(x => new GridRect(){ size = x.size, position = x.position })
                .ToArray();
            
            if (m_GridItemHandlers.TryAdd(itemDragHandler, new GridItemHandler()
                {
                    ItemDragHandler = itemDragHandler,
                    Center = grid.center,
                    GridRects = gridRects,
                }))
            {
                itemDragHandler.OnApply += ApplyDestination;
            }

            foreach (var gridRect in gridRects)
            {
                m_BattleFieldView.newGrid.AddGridRect(gridRect);
            }
            
            
            Debug.Log("OnEnterDragHandler");
        }

        private ItemDragHandler m_ItemDragHandler;
        public void OnSlotBeginDrag(Slot slot, PointerEventData eventData)
        {
            if(m_ItemDragHandler!=null)return;
            m_ItemDragHandler = m_DragAndDropService.Create(slot.item)
                .SetCamera(Camera.main)
                .SetEventData(eventData)
                .SetSource(slot);

            m_ItemDragHandler.OnApply += ApplySource;
            m_ItemDragHandler.OnBeginDrag();
        }

        private void ApplySource(ItemDragHandler itemDragHandler)
        {
            
        }

        public void OnSlotEndDrag(Slot slot, PointerEventData eventData)
        {
            if(m_ItemDragHandler == null) return;
            m_ItemDragHandler.OnEndDrag();
            m_DragAndDropService.Dispose(m_ItemDragHandler);
            m_ItemDragHandler = null;
        }
    }
}