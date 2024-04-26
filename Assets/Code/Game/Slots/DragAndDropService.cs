using System;
using System.Collections.Generic;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Game.Slots
{
    
    public interface IDragHandler
    {
        void OnBeginDrag();
        void OnDrag();
        void OnEndDrag();
    }

    public interface IDropTarget
    {
        bool Overlap(IDragHandler dragHandler, out float weight);
    }

    public interface IEnterDragHandler : IDropTarget
    {
        void OnEnterDragHandler(IDragHandler dragHandler);
    }

    public interface IStayDragHandler : IDropTarget
    {
        void OnStayDragHandler(IDragHandler dragHandler);
    }
    
    public interface IExitDragHandler : IDropTarget
    {
        void OnExitDragHandler(IDragHandler dragHandler);
    }

    public class DragAndDropService : SerializedMonoBehaviour, IService, IInitializable
    {
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
            
        }

        [SerializeField] private List<IDropTarget> m_DropTargets = new List<IDropTarget>();
        [SerializeField] private List<IDragHandler> m_DragHandlers = new List<IDragHandler>();

        public ItemDragHandler Create(Item item)
        {
            ItemDragHandler itemDragHandler = new ItemDragHandler(item, m_DropTargets);
            m_DragHandlers.Add(itemDragHandler);
            return itemDragHandler;
        }

        public void RegisterDropTarget(IDropTarget target)
        {
            if (!m_DropTargets.Contains(target))
            {
                m_DropTargets.Add(target);
            }
        }

        public void UnregisterDropTarget(IDropTarget target)
        {
            m_DropTargets.Remove(target);
        }

        public void Dispose(ItemDragHandler itemDragHandler)
        {
            m_DragHandlers.Remove(itemDragHandler);
        }

        private void Update()
        {
            foreach (var itemHandle in m_DragHandlers)
            {
                itemHandle.OnDrag();
            }
        }
    }
}