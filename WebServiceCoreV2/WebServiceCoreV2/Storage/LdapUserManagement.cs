using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceCoreV2.Models;

namespace WebServiceCoreV2.Storage
{
    public class LdapUserManagement
    {
        private string ldapHost = "192.168.0.31";
        private int ldapPort = LdapConnection.DEFAULT_PORT;
        private LdapConnection conn = null;
        public string Login(Credential credential)
        {
            try
            {
                string username = GetDcUsername(credential.Username, "hellsingcorp.com", credential.Group, credential.Country);
                string password = credential.Password;
                conn = new LdapConnection();
                conn.Connect(ldapHost, ldapPort);
                conn.Bind(username, password);
                return "OK";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return "Invalid Credential";
            }
        }

        public string RegisterUser(Credential credential)
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RegisterUser error: {ex.Message}");
                return "Invalid Data";
            }
        }
        public List<Credential> GetUsers(string username)
        {
            try
            {
                List<Credential> listUserRegisted = new List<Credential>();
                return listUserRegisted;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUsers error: {ex.Message}");
                return new List<Credential>();
            }
        }
        private string GetDcUsername(string username, string domain, string group = "", string country = "")
        {
            try
            {
                string[] dcDomains = domain.Split('.');
                string g = group == null ? "" : group;
                string c = country == null ? "" : country;
                string usernameFormated = (g.Length == 0 && c.Length == 0) ? $"cn={username}," : $"cn={username},cn={g},ou={c},";
                int domainLength = dcDomains.Length;
                for (int i = 0; i < domainLength; i++)
                {
                    if (i != domainLength - 1)
                    {
                        usernameFormated += $"dc={dcDomains[i]},";
                    }
                    else
                    {
                        usernameFormated += $"dc={dcDomains[i]}";
                    }
                }
                return usernameFormated;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdatePassword(Credential credential, string newPassword)
        {
            try
            {
                string result = "No password updated";

                LdapConnection conn = new LdapConnection();
                conn.Connect(ldapHost, ldapPort);
                string username = GetDcUsername(credential.Username, "hellsingcorp.com", credential.Group, credential.Country);
                string password = credential.Password;
                conn.Bind(username, password);

                LdapModification[] modifications = new LdapModification[2];
                LdapAttribute deletePassword = new LdapAttribute("userPassword", password);
                modifications[0] = new LdapModification(LdapModification.DELETE, deletePassword);
                LdapAttribute addPassword = new LdapAttribute("userPassword", newPassword);
                modifications[1] = new LdapModification(LdapModification.ADD, addPassword);

                conn.Modify(username, modifications);
                conn.Disconnect();
                result = "Password updated";
                return result;
            }
            catch (LdapException e)
            {
                Console.WriteLine("Error:" + e.LdapErrorMessage);
                return e.Message;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return e.Message;
            }
        }
        public string AddNewUsername(Credential masterCredential, Credential newUserCredential, string fullName, string mail)
        {
            try
            {
                string[] temp = fullName.Split(' ');
                LdapAttributeSet attributeSet = new LdapAttributeSet
                {
                    new LdapAttribute("objectclass", "inetOrgPerson"),
                    // new LdapAttribute("cn", new string[] { "James Smith", "Jim Smith", "Jimmy Smith" }),
                    new LdapAttribute("givenname", temp[0]),
                    new LdapAttribute("sn", temp[1]),
                    new LdapAttribute("mail", mail),
                    new LdapAttribute("userpassword", newUserCredential.Password)
                };
                string dn = GetDcUsername(newUserCredential.Username, "hellsingcorp.com", newUserCredential.Group, newUserCredential.Country);
                LdapEntry newEntry = new LdapEntry(dn, attributeSet);
                LdapConnection conn = new LdapConnection();
                conn.Connect(ldapHost, ldapPort);
                string username = GetDcUsername(masterCredential.Username, "hellsingcorp.com", masterCredential.Group, masterCredential.Country);
                string password = masterCredential.Password;
                conn.Bind(username, password);
                conn.Add(newEntry);
                conn.Disconnect();
                return "Entry:" + dn + "  Added Successfully";
            }
            catch (LdapException e)
            {
                Console.WriteLine("Error:" + e.LdapErrorMessage);
                return e.Message;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return e.Message;
            }
        }
    }
}
