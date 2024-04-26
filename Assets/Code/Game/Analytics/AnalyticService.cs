using System;
using Code.Services;
using UnityEngine;

namespace Code.Game.Analytics
{
    public class AnalyticService : MonoBehaviour, IService, IInitializable
    {
        public const string IAP = "IAP";
        public const string SHOP = "Shop";
        public const string GAME = "Game";
        public const string ADS = "Ads";
        
        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }
        
        public void DeclareCurrency(string currencyName)
        {
            Debug.LogWarning($"DeclareCurrency: {currencyName}");
            TinySauce.DeclareCurrency(currencyName);
        }

        public void DeclareItemType(string itemTypeName)
        {
            Debug.LogWarning($"DeclareItemType: {itemTypeName}");
            TinySauce.DeclareItemType(itemTypeName);
        }

        public void OnCurrencyGiven(string currency, int currencyAmount, string itemType, string itemId)
        {
            Debug.LogWarning($"OnCurrencyGiven: {currency}, {currencyAmount}, {itemType}, {itemId}");
            TinySauce.OnCurrencyGiven(currency, currencyAmount, itemType, itemId);
        }

        public void OnCurrencyTaken(string currency, int currencyAmount, string itemType, string itemId)
        {
            Debug.LogWarning($"OnCurrencyTaken: {currency}, {currencyAmount}, {itemType}, {itemId}");
            TinySauce.OnCurrencyTaken(currency, currencyAmount, itemType, itemId);
        }

        public void Initialize()
        {
            DeclareItemType(SHOP);
            DeclareItemType(GAME);
            DeclareItemType(ADS);
            DeclareItemType(IAP);
        }
    }
}