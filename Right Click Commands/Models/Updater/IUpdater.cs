using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Updater
{
    public interface IUpdater
    {
        //  Methods
        //  =======

        Task<Asset> CheckForUpdateAsync();
        void UpdateTo(Asset asset);
    }
}
