using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Game.Wallet
{
    internal interface IWalletMigration
    {
        public bool Migrate(out int value);
    }
}
