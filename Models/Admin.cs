using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOOK.Models
{
    public class Admin
    {
        QLBANSACHEntities data = new QLBANSACHEntities();
        public string UserAdmin {  get; set; }
        public string PassAdmin { get; set; }   
        public string Hoten {  get; set; }
    }
}