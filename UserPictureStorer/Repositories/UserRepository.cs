using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserPictureStorer.Models;

namespace UserPictureStorer.Repositories
{
    public class UserRepository : TableServiceContext
    {

        public UserRepository()
            : base(CloudStorageAccount.Parse(Properties.Settings.Default.DataConnection).CreateCloudTableClient())
        {
            
        }

        public IQueryable<User> Users
        {
            get
            {
                return this.CreateQuery<User>("Users");
            }
        }

    }
}