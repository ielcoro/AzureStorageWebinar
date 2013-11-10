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


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
        
        [Display(Name="User Picture")]
        public string PictureUrl { get; set; }

        public override string PartitionKey
        {
            get
            {
                return "Users";
            }
            set
            {
                base.PartitionKey = value;
            }
        }

        public override string RowKey
        {
            get
            {
                return FirstName;
            }
            set
            {
                base.RowKey = value;
            }
        }



    }
}