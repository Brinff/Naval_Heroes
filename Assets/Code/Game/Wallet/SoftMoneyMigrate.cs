using System.Collections;
using UnityEngine;

namespace Assets.Code.Game.Wallet
{
    public class SoftMoneyMigrate : MonoBehaviour, IWalletMigration
    {
        private PlayerPrefsData<int> m_MoneySoft;
        public bool Migrate(out int value)
        {
            m_MoneySoft = new PlayerPrefsData<int>(nameof(m_MoneySoft));
            value = m_MoneySoft.Value;
            return m_MoneySoft.HasValue();
        }
    }
}