using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStudents_Osennikov.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public int UserFrom { get; set; }
        public int UserTo { get; set; }
        public string Message { get; set; }
        public Messages(int userFrom, int userTo, string Message)
        {
            this.UserFrom = userFrom;
            this.UserTo = userTo;
            this.Message = Message;
        }
    }
}
