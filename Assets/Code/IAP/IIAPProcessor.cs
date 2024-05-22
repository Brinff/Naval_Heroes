using UnityEngine.Purchasing;

namespace Code.IAP
{
    public interface IIAPProcessor
    {
        string productId { get; }
        PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent);
        void OnInitialize(Product product, IAPShopService shopService);
    }
}