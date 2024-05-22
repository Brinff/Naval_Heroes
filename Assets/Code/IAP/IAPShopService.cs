using System;
using System.Collections;
using System.Linq;
using Code.Services;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Code.IAP
{
    public class IAPShopService : MonoBehaviour, IDetailedStoreListener, IService, IInitializable
    {
        private IStoreController m_Controller;
        private IExtensionProvider m_Extensions;
        private IIAPProcessor[] m_Processors;

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
            m_Processors = GetComponentsInChildren<IIAPProcessor>();
            var catalog = ProductCatalog.LoadDefaultCatalog();
            var module = StandardPurchasingModule.Instance();
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            var builder = ConfigurationBuilder.Instance(module);

            IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, catalog);
            UnityPurchasing.Initialize(this, builder);


            //var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            //var catalog = ProductCatalog.LoadDefaultCatalog();
            //IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, catalog);
            //UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            m_Controller = controller;
            m_Extensions = extensions;

            foreach (var product in controller.products.all)
            {
                var processor = m_Processors.FirstOrDefault(x => x.productId == product.definition.id);
                processor?.OnInitialize(product, this);
                Debug.Log("Initialized product: " + product.definition.id);
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"{error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log($"{error}: {message}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            PurchaseProcessingResult result;
            var consumePurchase = false;
            var resultProcessed = false;

            foreach (var processor in m_Processors.Where(button =>
                         button.productId == purchaseEvent.purchasedProduct.definition.id))
            {
                result = processor.ProcessPurchase(purchaseEvent);

                if (result == PurchaseProcessingResult.Complete)
                {
                    consumePurchase = true;
                }

                resultProcessed = true;
            }

            if (resultProcessed)
            {
                Debug.Log("Completed processing purchase: " + purchaseEvent.purchasedProduct.definition.id);
            }

            return consumePurchase ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            
        }

        private void OnTransactionsRestored(bool success, string error)
        {
            
        }

        public void Restore()
        {
            if (Application.platform == RuntimePlatform.WSAPlayerX86 ||
                Application.platform == RuntimePlatform.WSAPlayerX64 ||
                Application.platform == RuntimePlatform.WSAPlayerARM)
            {
                CodelessIAPStoreListener.Instance.GetStoreExtensions<IMicrosoftExtensions>()
                    .RestoreTransactions();
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer ||
                     Application.platform == RuntimePlatform.OSXPlayer ||
                     Application.platform == RuntimePlatform.tvOS
#if UNITY_VISIONOS
                         || Application.platform == RuntimePlatform.VisionOS
#endif
                    )
            {
                CodelessIAPStoreListener.Instance.GetStoreExtensions<IAppleExtensions>()
                    .RestoreTransactions(OnTransactionsRestored);
            }
            else if (Application.platform == RuntimePlatform.Android &&
                     StandardPurchasingModule.Instance().appStore == AppStore.GooglePlay)
            {
                CodelessIAPStoreListener.Instance.GetStoreExtensions<IGooglePlayStoreExtensions>()
                    .RestoreTransactions(OnTransactionsRestored);
            }
            else
            {
                Debug.LogWarning(Application.platform +
                                 " is not a supported platform for the Codeless IAP restore button");
            }
        }

        public void InitiatePurchase(string productID)
        {
            m_Controller.InitiatePurchase(productID);
        }
    }
}