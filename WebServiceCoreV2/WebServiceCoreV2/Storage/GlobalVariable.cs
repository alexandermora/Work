using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceCoreV2.Models;

namespace WebServiceCoreV2.Storage
{
    public class GlobalVariable
    {
        public static string PolicyAssign { get; set; } = "AppBudget";
        public static Credential CredentialUser { get; set; }
        public static JsonTokenCustom TokenCustom { get; set; }
        public static string[] ListMonth { get; set; } = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public static int MAX_BYTES { get; set; } // Read from database
        public static string[] ACCEPTED_FILE_TYPE { get; set; }
        public static string[,,] ListFilters { get; set; } = new string[4, 4, 2];
        public static Credential DataBaseCredentials { get; } = new Credential { Username = "SA", Password = "Karen123" };
    }
}
