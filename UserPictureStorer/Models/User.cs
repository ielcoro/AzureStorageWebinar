using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Drawing;
using System.Linq;
using System.Web;

namespace UserPictureStorer.Models
{
    public class User : TableServiceEntity
    {
        public User()
        {

        }

        public User(string firstName, string lastName)
            : base(firstName, lastName)
        {

        }

        public string FirstName
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }


        public string LastName
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }



        public int Age { get; set; }

    }
}