using System;


namespace HelpDeskData
{
    public class DataRepositoryStore
    {
        public static readonly string KEY_DATACONTEXT = "HelpDeskData";
        public static IDataRepositoryStore CurrentDataStore;
    }
}
