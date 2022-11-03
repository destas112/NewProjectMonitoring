using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects;
using System.Linq;
using HelpDeskData;



namespace HelpDeskData
{
    public static class CurrentDataContext
    {
        public static HelpDeskDBDB CurrentContext
        {
            get
            {
                var repository = DataRepositoryStore.CurrentDataStore[DataRepositoryStore.KEY_DATACONTEXT] as HelpDeskDBDB;//IRepository;
                if (repository == null)
                {

                    repository = new HelpDeskDBDB();
                    //repository.CommandTimeout = 1800;
                    DataRepositoryStore.CurrentDataStore[DataRepositoryStore.KEY_DATACONTEXT] = repository;
                }

                return repository;

            }
        }

        public static void CloseCurrentRepository()
        {
            var repository = DataRepositoryStore.CurrentDataStore[DataRepositoryStore.KEY_DATACONTEXT] as HelpDeskDBDB;//IRepository;
            if (repository != null)
            {
                repository.Close();
                DataRepositoryStore.CurrentDataStore[DataRepositoryStore.KEY_DATACONTEXT] = null;
            }
        }
    }

}
