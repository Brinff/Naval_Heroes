using System;
using Code.Services;
using Game.UI.Grid;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Game.Slots.Battle
{
    public class BattleFieldView : MonoBehaviour, IService
    {
        [SerializeField] private SlotGroup m_SlotGroup;
        public SlotGroup slotGroup => m_SlotGroup;

        [SerializeField] private GridRenderer m_CurrentGrid;
        public GridRenderer currentGrid => m_CurrentGrid;
        [SerializeField] private GridRenderer m_NewGrid;
        public GridRenderer newGrid => m_NewGrid;
        [SerializeField] private GridRenderer m_RejectGrid;
        public GridRenderer rejectGrid => m_RejectGrid;
        
        [SerializeField] private Area m_Area;
        public Area area => m_Area;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }
    }
}