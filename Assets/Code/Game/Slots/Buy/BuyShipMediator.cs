using Code.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Code.Game.Slots.DragAndDrop;

namespace Code.Game.Slots.Buy
{
    public class BuyShipMediator : MonoBehaviour, IService, IInitializable, ISlotBeginDragHandler, ISlotEndDragHandler,
        ISlotClickHandler
    {
        [SerializeField] private Category m_Category;
        [SerializeField] private EntityData m_Entity;
        public Category category => m_Category;

        private BuyCurrencyShipService m_BuyCurrencyShipService;
        private BuyAdsShipService m_BuyAdsShipService;
        private BuyShipView m_BuyShipView;
        private ItemFactory m_ItemFactory;
        private DragAndDropService m_DragAndDropService;

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
            m_BuyAdsShipService = ServiceLocator.Get<BuyAdsShipService>();
            m_BuyCurrencyShipService = ServiceLocator.Get<BuyCurrencyShipService>(x => x.category == m_Category);
            m_BuyCurrencyShipService.OnUpdate += UpdateCurrency;
            m_ItemFactory = ServiceLocator.Get<ItemFactory>();
            m_DragAndDropService = ServiceLocator.Get<DragAndDropService>();

            m_BuyShipView = ServiceLocator.Get<BuyShipView>(x => x.category == m_Category);
            m_BuyShipView.slot.SetSlotListener(this);
            m_BuyShipView.slot.Put(m_ItemFactory.Create(m_Entity));
            m_BuyShipView.costLabel.SetValue(m_BuyCurrencyShipService.cost, true);
        }

        private void UpdateCurrency()
        {
            m_BuyShipView.costLabel.SetValue(m_BuyCurrencyShipService.cost, false);
        }
        
        private void Apply(ItemDragHandler itemDragHandler)
        {
            m_BuyCurrencyShipService.Buy();
            m_BuyShipView.slot.Put(m_ItemFactory.Create(m_Entity));
        }

        private bool Validate(ItemDragHandler itemDragHandler)
        {
            return m_BuyCurrencyShipService.IsEnoughCurrency();
        }

        private void Discard(ItemDragHandler itemDragHandler)
        {
            
        }

        private ItemDragHandler m_ItemDragHandler;

        public void OnSlotBeginDrag(Slot slot, PointerEventData eventData)
        {
            if (m_ItemDragHandler != null) return;

            m_ItemDragHandler = m_DragAndDropService.Create(slot.item)
                .SetSource(slot)
                .SetCamera(Camera.main)
                .SetEventData(eventData);

            m_ItemDragHandler.OnApply += Apply;
            m_ItemDragHandler.OnDiscard += Discard;
            m_ItemDragHandler.OnValidate += Validate;
            m_ItemDragHandler.OnBeginDrag();
            
            Debug.Log("OnSlotBeginDrag");
        }

        public void OnSlotEndDrag(Slot slot, PointerEventData eventData)
        {
            if (m_ItemDragHandler == null)return;
            m_ItemDragHandler.OnEndDrag();
            m_DragAndDropService.Dispose(m_ItemDragHandler);
            m_ItemDragHandler = null;
            Debug.Log("OnSlotEndDrag");
        }

        public void OnSlotClick(Slot slot, PointerEventData eventData)
        {
            if (m_ItemDragHandler != null) return;
            /*Debug.Log("OnSlotClick");
            m_DragAndDropService.Create(slot.item)
                .SetSource(slot)
                .OnApply(Apply)
                .OnDiscard(Discard);*/
        }
    }
}