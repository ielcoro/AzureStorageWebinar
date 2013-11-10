using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [IgnoreProperty]
        public string FirstName
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }

        [IgnoreProperty]
        public string LastName
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }


        public int Age { get; set; }
        
        [Display(Name="User Picture")]
        public string PictureUrl { get; set; }        

    }
}