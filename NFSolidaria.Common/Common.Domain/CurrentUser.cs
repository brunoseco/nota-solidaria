using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
 
    [Serializable]
    public class CurrentUser
    {
        public int UserId { get; set; }

        public object UserInfo { get; set; } 

        public bool OnlyUser { get; set; }
        
        public T GetUserInfo<T>() 
        {
            var result = (T)UserInfo;
            return result;
        }

        public void SetUserInfo<T>(T userInfo)
        {
            this.UserInfo = userInfo;
        }

    }
}
