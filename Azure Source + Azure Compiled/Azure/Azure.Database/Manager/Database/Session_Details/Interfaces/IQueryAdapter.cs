using System;
using Azure.Database.Manager.Session_Details.Interfaces;

namespace Azure.Database.Manager.Database.Session_Details.Interfaces
{
    public interface IQueryAdapter : IRegularQueryAdapter, IDisposable
    {
        void DoCommit();

        void DoRollBack();

        long InsertQuery();

        void RunQuery();
    }
}