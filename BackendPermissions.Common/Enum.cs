using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendPermissions.Common
{
    public class Enum
    {
        public enum LogType
        {
            WebSite,
            ConsoleTask,
        }

        public enum EnumMessage
        {
            Succes,
            Error,
            Alert
        }
        public enum CallType
        {
            Get,
            Request,
            Modify            
        }
    }
}