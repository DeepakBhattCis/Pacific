using PacificBBExtranet.Services.Account;
using PacificBBExtranet.Services.PropertyDetails;
using PacificBBExtranet.Services.Services.Deals;
using PacificBBExtranet.Services.Services.Synch;
using PacificBBExtranet.Services.Services.XML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificBBExtranet.Web.Helpers
{
    public static class ApplicationServices
    {
            private static System.Configuration.ConnectionStringSettings _connectionSettings;

            private static string connectionString
            {
                get
                {
                    if (_connectionSettings == null)
                        _connectionSettings = ConfigurationManager.ConnectionStrings["PacificBedBankEntities"];

                    return _connectionSettings.ConnectionString;
                }
            }

        public static AccountService AccountService
        {
            get
            {
                return new AccountService(connString: connectionString, userID:AuthenticationHelper.CurrentUserId());
            }
        }

        public static PropertyDetailsService PropertyDetailsService
        {
            get
            {
                return new PropertyDetailsService(connString: connectionString, 
                    userID: AuthenticationHelper.CurrentUserId(),
                    resortID:AuthenticationHelper.CurrenResortID());
            }
        }

        public static DealsService DealsServices
        {
            get
            {
                return new DealsService(connString: connectionString,
                    userID: AuthenticationHelper.CurrentUserId(),
                    resortID: AuthenticationHelper.CurrenResortID());
            }
        }
        public static XMLMessagesService XMLMessagesService
        {
            get
            {
                return new XMLMessagesService(connectionString: connectionString,
                    userID: AuthenticationHelper.CurrentUserId(),
                    resortID: AuthenticationHelper.CurrenResortID());
            }
        }

        

        public static SynchService SynchService
        {
            get
            {
                return new SynchService(connString: connectionString,
                    userID: AuthenticationHelper.CurrentUserId(),
                    resortID: AuthenticationHelper.CurrenResortID(),
                    propService: PropertyDetailsService);
            }
        }



    }
}
