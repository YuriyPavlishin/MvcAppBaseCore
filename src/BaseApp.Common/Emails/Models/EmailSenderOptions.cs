using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseApp.Common.Utils.Email
{
    public class EmailSenderOptions
    {
        public string AllowedEmailAddresses { get; set; }
        public string FromEmail { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        private HashSet<string> AllowedEmailAddressesHasSet
        {
            get
            {
                HashSet<string> l_set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (!string.IsNullOrEmpty(AllowedEmailAddresses))
                {
                    string[] l_arr = AllowedEmailAddresses.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(m => m.Trim())
                        .Where(m => !String.IsNullOrWhiteSpace(m)).ToArray();
                    l_set.UnionWith(l_arr);
                }
                return l_set;
            }
        }

        public bool IsEmailAddressAllowed(string p_emailAddressList)
        {
            bool l_res = true;
            if (!string.IsNullOrWhiteSpace(p_emailAddressList))
            {
                HashSet<string> l_addressesAllowed = AllowedEmailAddressesHasSet;

                if (l_addressesAllowed.Count > 0)
                {
                    string[] l_addresses = p_emailAddressList.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string l_address in l_addresses)
                    {
                        if (!l_addressesAllowed.Contains(l_address))
                        {
                            l_res = false;
                            break;
                        }
                    }
                }
            }
            return l_res;
        }
    }
}
