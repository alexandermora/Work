using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebServiceCoreV2.Models;
using WebServiceCoreV2.Models.Payments;
using WebServiceCoreV2.Storage.Sql;

namespace WebServiceCoreV2.Storage
{
    public class ManageSqlServer
    {
        private SqlConnection connection;
        private string ip = "192.168.0.31";
        private string schema = "personalBudgetAS";

        public ManageSqlServer()
        {

        }
        public ManageSqlServer(Credential credential)
        {
            connection = new SqlConnection($"Data Source={ip};Initial Catalog={schema};Persist Security Info=True;User ID={credential.Username};Password={credential.Password}");
        }
        private async Task<bool> OpenConnectionAsync()
        {
            try
            {
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Open connection error: {ex.Message}");
                return false;
            }
        }
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Close connection error: {ex.Message}");
                return false;
            }
        }
        public async Task<string> GetImageUrl(string user)
        {
            try
            {
                string value = "";
                if (await OpenConnectionAsync())
                {

                }
                return value;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task<List<MethodPay>> GetListPayment(string typePayment, DateTime datePay)
        {
            try
            {
                List<MethodPay> listResult = new List<MethodPay>();
                int idMonth = await GetMonthAsync(datePay);
                int idWeek = await GetWeekAsync(datePay, idMonth);
                switch (typePayment)
                {
                    case "Day":
                        {
                            int idDay = await GetDayAsync(datePay, idWeek);
                            listResult = await GetListRegisterAsync(CommandGlobal.GetRegByDay, "@idDay", typePayment, idDay);
                            return listResult;
                        }
                    case "Week":
                        {
                            listResult = await GetListRegisterAsync(CommandGlobal.GetRegByWeek, "@idWeek", typePayment, idWeek);
                            return listResult;
                        }
                    case "Month":
                        {
                            listResult = await GetListRegisterAsync(CommandGlobal.GetRegByMonth, "@idMonth", typePayment, idMonth);
                            return listResult;
                        }
                    default:
                        {
                            return listResult;
                        }
                }
            }
            catch (Exception ex)
            {
                return new List<MethodPay> { new MethodPay { TypePayment = "Error", PaymentValue = new Payment { Name = $"Error getting Data is: {ex.Message}" } } };
            }
        }
        private async Task<List<MethodPay>> GetListRegisterAsync(string sql, string param, string typePayment, int id)
        {
            try
            {
                List<MethodPay> listResult = new List<MethodPay>();
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = sql;
                    myCommand.Parameters.AddWithValue(param, id);
                    SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int idReg = reader.GetInt32(reader.GetOrdinal("id"));
                        string name = reader["name"].ToString();
                        string description = reader["description"].ToString();
                        string typeCatalogue = reader["typeCatalogue"].ToString();
                        double valuePay = double.Parse(reader["valuePay"].ToString());
                        string currency = reader["currency"].ToString();
                        string datePay = reader["dateBuy"].ToString();
                        string timezone = reader["timezone"].ToString();
                        string support = reader["support"].ToString();
                        Payment payment = new Payment
                        {
                            Id = idReg,
                            Name = name,
                            Description = description,
                            TypeCatalogue = typeCatalogue,
                            ValuePay = valuePay,
                            Currency = currency,
                            DatePay = datePay,
                            Timezone = timezone,
                            Support = support
                        };
                        MethodPay mPay = new MethodPay
                        {
                            TypePayment = typePayment,
                            PaymentValue = payment
                        };
                        listResult.Add(mPay);
                    }
                }
                return listResult;
            }
            catch (Exception ex)
            {
                return new List<MethodPay> { new MethodPay { TypePayment = "Error", PaymentValue = new Payment { Name = $"Error getting Data is: {ex.Message}" } } };
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task<List<ListShow>> GetFilterAsync()
        {
            List<ListShow> resultList = new List<ListShow>();
            try
            {
                string param = "Filter";
                await GetListAsync(resultList, param);
                return resultList;
            }
            catch (Exception ex)
            {
                resultList.Add(new ListShow { Id = -1, Name = $"Error getting the list of filters is: {ex.Message}" });
                return resultList;
            }
        }
        public async Task<List<ListShow>> GetTypeConsultAsync()
        {
            List<ListShow> resultList = new List<ListShow>();
            try
            {
                string param = "TypeConsult";
                await GetListAsync(resultList, param);
                return resultList;
            }
            catch (Exception ex)
            {
                resultList.Add(new ListShow { Id = -1, Name = $"Error getting the list of consult is: {ex.Message}" });
                return resultList;
            }
        }
        public async Task<List<PaymentFiltered>> GetListPaymentFiltedAsync(string filter, string typePayment, string typeConsult, string dateBuy)
        {
            try
            {
                List<PaymentFiltered> listResult = new List<PaymentFiltered>();
                List<ListShow> listParamFilters = new List<ListShow>();
                List<ListShow> listParamTime = new List<ListShow>();
                List<ListShow> listParamTypeConsult = new List<ListShow>();
            
                string sql = "";

                string param = "Filter";
                await GetListAsync(listParamFilters, param);

                param = "Time";
                await GetListAsync(listParamTime, param);

                param = "TypeConsult";
                await GetListAsync(listParamTypeConsult, param);
                
                int indexFilter = GetIdList(listParamFilters, filter);
                int indexTime = GetIdList(listParamTime, typePayment) + 1;
                int indexTypeConsult = GetIdList(listParamTypeConsult, typeConsult);
                int idDate = (dateBuy != null) ? await GetIdDate(dateBuy, typePayment) : -1;

                sql = GlobalVariable.ListFilters[indexFilter, indexTime, indexTypeConsult];
                await GetListRegisterFiltedAsync(listResult, sql, idDate, GlobalVariable.CredentialUser.Username);
                return listResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting the payment filtered: {ex.Message}");
                return new List<PaymentFiltered> { new PaymentFiltered { Name = $"Error getting the payment filtered: {ex.Message}", Count = -1.0D } };
            }
        }
        private async Task<int> GetIdDate(string dateBuy, string typePayment)
        {
            try
            {
                int idMonth = await GetMonthAsync(DateTime.Parse(dateBuy));
                int idWeek = await GetWeekAsync(DateTime.Parse(dateBuy), idMonth);
                switch (typePayment)
                {
                    case "Day":
                        {
                            int idDay = await GetDayAsync(DateTime.Parse(dateBuy), idWeek);
                            return idDay;
                        }
                    case "Week":
                        {
                            return idWeek;
                        }
                    case "Month":
                        {
                            return idMonth;
                        }
                    default:
                        {
                            return -1;
                        }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int GetIdList(List<ListShow> list, string param)
        {
            try
            {
                int id = -1;
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (list[i].Name.Equals(param))
                    {
                        id = i;
                        i = count + 1;
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting index of {param}: {ex.Message}");
                return -1;
            }
        }
        private async Task GetListRegisterFiltedAsync(List<PaymentFiltered> listResult, string sql, int id, string username)
        {
            try
            {
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = sql;
                    
                    if (id != -1)
                    {
                        myCommand.Parameters.Add("@id", SqlDbType.Int);
                        myCommand.Parameters["@id"].Value = id;
                    }
                    myCommand.Parameters.Add("@username", SqlDbType.VarChar);
                    myCommand.Parameters["@username"].Value = username;
                    SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        string fieldRow = reader[0].ToString();
                        double count = double.Parse(reader["TOTAL"].ToString().Trim());
                        listResult.Add(new PaymentFiltered { Name = fieldRow, Count = count });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Data is: {ex.Message}");
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task<List<ListShow>> GetContriesAsync()
        {
            List<ListShow> resultList = new List<ListShow>();
            try
            {
                string param = "Country";
                await GetListAsync(resultList, param);
                return resultList;
            }
            catch (Exception ex)
            {
                resultList.Add(new ListShow { Id = -1, Name = $"Error getting the list of countries is: {ex.Message}" });
                return resultList;
            }
        }
        public async Task<List<ListShow>> GetGroupAsync()
        {
            List<ListShow> resultList = new List<ListShow>();
            try
            {
                string param = "Group";
                await GetListAsync(resultList, param);
                return resultList;
            }
            catch (Exception ex)
            {
                resultList.Add(new ListShow { Id = -1, Name = $"Error getting the list of countries is: {ex.Message}" });
                return resultList;
            }
        }
        public async Task<string> GetPrivilegesAsync(string username)
        {
            string result = "";
            try
            {
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.GetPrivileges;
                    myCommand.Parameters.AddWithValue("@Param1", username);
                    SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        result = reader.GetString(reader.GetOrdinal("ROLE"));
                    }
                    
                }
                return result;
            }
            catch (Exception ex)
            {
                return $"Error getting the list of countries is: {ex.Message}";
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task<string> SaveTokenUserAsync(string json)
        {
            try
            {
                string result = "";                
                if (await OpenConnectionAsync())
                {
                    JsonTokenCustom tokenDeserialize = JsonConvert.DeserializeObject<JsonTokenCustom>(json);
                    // Instant represents time from epoch
                    Instant now = SystemClock.Instance.GetCurrentInstant();

                    DateTime dateSystem = DateTime.Parse(now.ToString());

                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.InsertRegToken;
                    myCommand.Parameters.Add("@tokenName", SqlDbType.NVarChar);
                    myCommand.Parameters.Add("@duration", SqlDbType.Int);
                    myCommand.Parameters.Add("@tokenDateUser", SqlDbType.DateTime2);
                    myCommand.Parameters.Add("@username", SqlDbType.NVarChar);
                    myCommand.Parameters.Add("@nameApp", SqlDbType.NVarChar);
                    myCommand.Parameters["@tokenName"].Value = tokenDeserialize.Token;
                    myCommand.Parameters["@duration"].Value = tokenDeserialize.Expires;
                    myCommand.Parameters["@tokenDateUser"].Value = dateSystem;
                    myCommand.Parameters["@username"].Value = GlobalVariable.CredentialUser.Username;
                    myCommand.Parameters["@nameApp"].Value = "Budget-App";
                    int temp = await myCommand.ExecuteNonQueryAsync();
                    result = (temp == 1) ? "OK" : "Fail";
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        private async Task GetListAsync(List<ListShow> resultList, string param)
        {
            try
            {
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.GetList;
                    myCommand.Parameters.Add("@Param1", SqlDbType.VarChar);
                    myCommand.Parameters["@Param1"].Value = param;
                    SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("ID"));
                        string value = reader.GetString(reader.GetOrdinal("VALUE"));
                        resultList.Add(new ListShow { Id = id, Name = value });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }

        }
        public async Task<List<ListShow>> GetTimeAsync()
        {
            List<ListShow> resultList = new List<ListShow>();
            try
            {
                string param = "Time";
                await GetListAsync(resultList, param);
                return resultList;
            }
            catch (Exception ex)
            {
                resultList.Add(new ListShow { Id = -1, Name = $"Error getting the list of countries is: {ex.Message}" });
                return resultList;
            }
        }
        public async Task<List<ListShow>> GetCataloguesAsync()
        {
            List<ListShow> resultList = new List<ListShow>();
            try
            {
                string param = "Catalogue";
                await GetListAsync(resultList, param);
                return resultList;
            }
            catch (Exception ex)
            {
                resultList.Add(new ListShow { Id = -1, Name = $"Error getting the list of countries is: {ex.Message}" });
                return resultList;
            }
        }
        public async Task<List<ListShow>> GetCurrencyAsync()
        {
            List<ListShow> resultList = new List<ListShow>();
            try
            {
                string param = "Currency";
                await GetListAsync(resultList, param);
                return resultList;
            }
            catch (Exception ex)
            {
                resultList.Add(new ListShow { Id = -1, Name = $"Error getting the list of countries is: {ex.Message}" });
                return resultList;
            }
        }
        private async Task<int> GetDayAsync(DateTime date, int idWeek)
        {
            int id = -1;
            try
            {
                if (await OpenConnectionAsync())
                {
                    string dayName = date.Day.ToString();
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.GetDay;
                    myCommand.Parameters.AddWithValue("@name", dayName);
                    myCommand.Parameters.AddWithValue("@idWeek", idWeek);
                    SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("ID"));
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        private async Task<int> GetWeekAsync(DateTime date, int idMonth)
        {
            int id = -1;
            try
            {
                if (await OpenConnectionAsync())
                {
                    string weekName = $"Week {(date.Day - 1) / 7 + 1}";
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.GetWeek;
                    myCommand.Parameters.AddWithValue("@week", weekName);
                    myCommand.Parameters.AddWithValue("@idMonth", idMonth);
                    SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("ID"));
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        private async Task<int> GetMonthAsync(DateTime date)
        {
            int id = -1;
            try
            {
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.GetMonth;
                    myCommand.Parameters.AddWithValue("@month", date.Month);
                    myCommand.Parameters.AddWithValue("@year", date.Year);
                    SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("ID"));
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task<string> GetPropertyAsync(string property)
        {
            try
            {
                string value = "";
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.GetProperty;
                    myCommand.Parameters.Add("@Param1", SqlDbType.VarChar);
                    myCommand.Parameters["@Param1"].Value = property;
                    SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        value = reader.GetString(reader.GetOrdinal("value"));
                    }
                }
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task GetProfileAsync(string username)
        {
            try
            {
                if (await OpenConnectionAsync())
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<int> InsertDayAsync(DateTime date, int idWeek)
        {
            try
            {
               if (await OpenConnectionAsync())
               {
                    string dayName = date.Day.ToString();
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.InsertDay;
                    myCommand.Parameters.Add("@name", SqlDbType.NVarChar);
                    myCommand.Parameters.Add("@idWeek", SqlDbType.Int);
                    myCommand.Parameters.Add("@dayOfWeek", SqlDbType.NVarChar);
                    myCommand.Parameters["@name"].Value = dayName;
                    myCommand.Parameters["@idWeek"].Value = idWeek;
                    myCommand.Parameters["@dayOfWeek"].Value = date.DayOfWeek.ToString();
                    int result = await myCommand.ExecuteNonQueryAsync();
                    return result;
               }
               return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        private async Task<int> InsertWeekAsync(DateTime date, int idMonth)
        {
            try
            {
                if (await OpenConnectionAsync())
                {
                    int weekValue = (date.Day - 1) / 7 + 1;
                    string weekName = $"Week {weekValue}";
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.InsertWeek;
                    myCommand.Parameters.Add("@nameWeek", SqlDbType.NVarChar);
                    myCommand.Parameters.Add("@valueInMonth", SqlDbType.Int);
                    myCommand.Parameters.Add("@idMonth", SqlDbType.Int);
                    myCommand.Parameters["@nameWeek"].Value = weekName;
                    myCommand.Parameters["@valueInMonth"].Value = weekValue;
                    myCommand.Parameters["@idMonth"].Value = idMonth;
                    int result = await myCommand.ExecuteNonQueryAsync();
                    return result;
                }
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        private async Task<int> InsertMonthAsync(DateTime date)
        {
            try
            {
                if (await OpenConnectionAsync())
                {
                    int monthValue = date.Month;
                    string monthName = GlobalVariable.ListMonth[monthValue];
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.InsertMonth;
                    myCommand.Parameters.Add("@name", SqlDbType.NVarChar);
                    myCommand.Parameters.Add("@valueInYear", SqlDbType.Int);
                    myCommand.Parameters.Add("@year", SqlDbType.Int);
                    myCommand.Parameters["@name"].Value = monthName;
                    myCommand.Parameters["@valueInYear"].Value = monthValue;
                    myCommand.Parameters["@year"].Value = date.Year;
                    int result = await myCommand.ExecuteNonQueryAsync();
                    return result;
                }
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task<String> InsertPaymentAsync(MethodPay mPay)
        {
            try
            {
                string result = "";
                int resultD = 0;
                int idMonth = await GetMonthAsync(DateTime.Parse(mPay.PaymentValue.DatePay));
                if (idMonth == -1)
                {
                    int resultM = await InsertMonthAsync(DateTime.Parse(mPay.PaymentValue.DatePay));
                    idMonth = await GetMonthAsync(DateTime.Parse(mPay.PaymentValue.DatePay));
                }
                int idWeek = await GetWeekAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idMonth);
                if (idWeek == -1)
                {
                    int resultW = await InsertWeekAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idMonth);
                    idWeek = await GetWeekAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idMonth);
                }
                switch (mPay.TypePayment)
                {
                    case "Day":
                        {
                            int idDay = await GetDayAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idWeek);
                            if (idDay == -1)
                            {
                                resultD = await InsertDayAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idWeek);
                                idDay = await GetDayAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idWeek);
                            }
                            string idReg = await InsertRegisterAsync(mPay.PaymentValue);
                            string temp = await InsertDayPaymentAsync(idDay, int.Parse(idReg));
                            result = $"The result of insert day payment is: {temp}";
                            return result;
                        }
                    case "Week":
                        {
                            string idReg = await InsertRegisterAsync(mPay.PaymentValue);
                            string temp = await InsertWeekPaymentAsync(idWeek, int.Parse(idReg));
                            result = $"The result of insert week payment is: {temp}";
                            return result;
                        }
                    case "Month":
                        {
                            string idReg = await InsertRegisterAsync(mPay.PaymentValue);
                            string temp = await InsertMonthPaymentAsync(idMonth, int.Parse(idReg));
                            result = $"The result of insert month payment is: {temp}";
                            return result;
                        }
                    default:
                        {
                            result = "Isn't Implementated";
                            return result;
                        }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private async Task<String> InsertRegisterAsync(Payment payment)
        {
            int id = -1;
            try
            {
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.InsertPayment;
                    SetParamsPayment(payment, ref myCommand);
                    int result = await myCommand.ExecuteNonQueryAsync();
                    if (result == 1)
                    {
                        myCommand = connection.CreateCommand();
                        myCommand.CommandText = CommandGlobal.GetRegisterId;
                        SetParamsPayment(payment, ref myCommand);
                        SqlDataReader reader = await myCommand.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            id = reader.GetInt32(reader.GetOrdinal("ID"));
                        }
                    }
                }
                return $"{id}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Insert: {ex.Message}");
                return $"Error Insert: {ex.Message}";
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task<string> InsertOptionsAsync(ShowDto<ListShow> value)
        {
            string message = "";
            try
            {
                if (await OpenConnectionAsync())
                {

                }

                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        private async Task<String> InsertDayPaymentAsync(int idDay, int idRegister)
        {
            int result = -1;
            try
            {
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.InsertPaymentDay;
                    myCommand.Parameters.Add("@day", SqlDbType.Int);
                    myCommand.Parameters.Add("@reg", SqlDbType.Int);
                    myCommand.Parameters["@day"].Value = idDay;
                    myCommand.Parameters["@reg"].Value = idRegister;
                    result = await myCommand.ExecuteNonQueryAsync();
                }

                return (result == 1) ? "OK" : "Fail";
            }
            catch (Exception ex)
            {
                return $"Error Insert Day and Payment {ex.Message}";
            }
        }
        private async Task<String> InsertWeekPaymentAsync(int idWeek, int idRegister)
        {
            int result = -1;
            try
            {
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.InsertPaymentWeek;
                    myCommand.Parameters.Add("@week", SqlDbType.Int);
                    myCommand.Parameters.Add("@reg", SqlDbType.Int);
                    myCommand.Parameters["@week"].Value = idWeek;
                    myCommand.Parameters["@reg"].Value = idRegister;
                    result = await myCommand.ExecuteNonQueryAsync();
                }
                return (result == 1) ? "OK" : "Fail";
            }
            catch (Exception ex)
            {
                return $"Error Insert Week and Payment {ex.Message}";
            }
        }
        private async Task<String> InsertMonthPaymentAsync(int idMonth, int idRegister)
        {
            int result = -1;
            try
            {
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.InsertPaymentMonth;
                    myCommand.Parameters.Add("@month", SqlDbType.Int);
                    myCommand.Parameters.Add("@reg", SqlDbType.Int);
                    myCommand.Parameters["@month"].Value = idMonth;
                    myCommand.Parameters["@reg"].Value = idRegister;
                    result = await myCommand.ExecuteNonQueryAsync();
                }
                return (result == 1) ? "OK" : "Fail";
            }
            catch (Exception ex)
            {
                return $"Error Insert Month and Payment {ex.Message}";
            }
        }
        public async Task<string> UpdatePaymentAsync(MethodPay mPay, string oldTypePayment)
        {
            try
            {
                var listTemp = await GetListRegisterAsync(CommandGlobal.GetRegById, "id", oldTypePayment, mPay.PaymentValue.Id);
                MethodPay oldMethodPay = listTemp[0];
                MethodPay newMethodPay = mPay;
                string result = "No update the payment";
                string resultUpdateRegister = await UpdateRegisterAsync(oldMethodPay.PaymentValue, newMethodPay.PaymentValue);
                result = $"The result of update day payment is: {resultUpdateRegister}";
                if (!oldMethodPay.TypePayment.Equals(newMethodPay.TypePayment) || !oldMethodPay.PaymentValue.DatePay.Equals(newMethodPay.PaymentValue.DatePay))
                {
                    int idMonth = await GetMonthAsync(DateTime.Parse(mPay.PaymentValue.DatePay));
                    if (idMonth == -1)
                    {
                        int resultM = await InsertMonthAsync(DateTime.Parse(mPay.PaymentValue.DatePay));
                        idMonth = await GetMonthAsync(DateTime.Parse(mPay.PaymentValue.DatePay));
                    }
                    int idWeek = await GetWeekAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idMonth);
                    if (idWeek == -1)
                    {
                        int resultW = await InsertWeekAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idMonth);
                        idWeek = await GetWeekAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idMonth);
                    }
                    string resultDelete = await DeleteConsolidateRegisterAsync(oldMethodPay.PaymentValue.Id, oldMethodPay.TypePayment);
                    switch (newMethodPay.TypePayment)
                    {
                        case "Day":
                            {
                                int idDay = await GetDayAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idWeek);
                                if (idDay == -1)
                                {
                                    int resultD = await InsertDayAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idWeek);
                                    idDay = await GetDayAsync(DateTime.Parse(mPay.PaymentValue.DatePay), idWeek);
                                }
                                string temp = await InsertDayPaymentAsync(idDay, newMethodPay.PaymentValue.Id);
                                result = $"The result of update day payment is: {temp}";
                                return result;
                            }
                        case "Week":
                            {
                                string temp = await InsertWeekPaymentAsync(idWeek, newMethodPay.PaymentValue.Id);
                                result = $"The result of update week payment is: {temp}";
                                return result;
                            }
                        case "Month":
                            {
                                string temp = await InsertMonthPaymentAsync(idMonth, newMethodPay.PaymentValue.Id);
                                result = $"The result of update Month payment is: {temp}";
                                return result;
                            }
                        default:
                            {
                                return "Isn't implementated";
                            }
                    }

                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                return $"Error update payment: {ex.Message}";
            }
        }
        private async Task<string> UpdateRegisterAsync(Payment oldPayment, Payment newPayment)
        {
            try
            {
                string result = "";
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    string updatedQuery = CommandGlobal.UpdatePayment;
                    CreateUpdateQuery("name", oldPayment.Name != newPayment.Name, false, ref updatedQuery);
                    CreateUpdateQuery("description", oldPayment.Description != newPayment.Description, false, ref updatedQuery);
                    CreateUpdateQuery("typeCatalogue", oldPayment.TypeCatalogue != newPayment.TypeCatalogue, false, ref updatedQuery);
                    CreateUpdateQuery("valuePay", oldPayment.ValuePay != newPayment.ValuePay, false, ref updatedQuery);
                    CreateUpdateQuery("currency", oldPayment.Currency != newPayment.Currency, false, ref updatedQuery);
                    CreateUpdateQuery("dateBuy", oldPayment.DatePay != newPayment.DatePay, false, ref updatedQuery);
                    CreateUpdateQuery("timezone", oldPayment.Timezone != newPayment.Timezone, false, ref updatedQuery);
                    CreateUpdateQuery("support", oldPayment.Support != newPayment.Support, true, ref updatedQuery);
                    int sizeUpdateQuery = updatedQuery.Trim().Length;
                    char[] query = updatedQuery.Trim().ToCharArray();
                    updatedQuery = (query[query.Length - 1].Equals(',')) ? updatedQuery.Trim().Substring(0, sizeUpdateQuery - 1) : updatedQuery.Trim();
                    if (updatedQuery.Length > CommandGlobal.UpdatePayment.Length)
                    {
                        updatedQuery += " WHERE [id] = @id";
                        myCommand.CommandText = updatedQuery;
                        SetUpdateParams("name", oldPayment.Name != newPayment.Name, newPayment.Name, ref myCommand, SqlDbType.VarChar);
                        SetUpdateParams("description", oldPayment.Description != newPayment.Description, newPayment.Description, ref myCommand, SqlDbType.VarChar);
                        SetUpdateParams("typeCatalogue", oldPayment.TypeCatalogue != newPayment.TypeCatalogue, newPayment.TypeCatalogue, ref myCommand, SqlDbType.VarChar);
                        SetUpdateParams("valuePay", oldPayment.ValuePay != newPayment.ValuePay, newPayment.ValuePay, ref myCommand, SqlDbType.Float);
                        SetUpdateParams("currency", oldPayment.Currency != newPayment.Currency, newPayment.Currency, ref myCommand, SqlDbType.VarChar);
                        SetUpdateParams("dateBuy", oldPayment.DatePay != newPayment.DatePay, newPayment.DatePay, ref myCommand, SqlDbType.DateTime2);
                        SetUpdateParams("timezone", oldPayment.Timezone != newPayment.Timezone, newPayment.Timezone, ref myCommand, SqlDbType.VarChar);
                        SetUpdateParams("support", oldPayment.Support != newPayment.Support, newPayment.Support, ref myCommand, SqlDbType.VarChar);
                        myCommand.Parameters.Add("@id", sqlDbType: SqlDbType.Int);
                        myCommand.Parameters["@id"].Value = oldPayment.Id;
                        int r = await myCommand.ExecuteNonQueryAsync();
                        result = (r == 1) ? "OK" : "Server Error";

                    }
                }
                return $"The update is: {result}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error update register is: {ex.Message}");
                return ex.Message;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        public async Task<string> DeletePaymentAsync(int idReg, string typePayment)
        {
            try
            {
                string resultDeleteConsolidate = await DeleteConsolidateRegisterAsync(idReg, typePayment);
                string temp = await DeleteRegisterAsync(idReg);
                string result = (resultDeleteConsolidate == "OK") ? $"Delete {typePayment} register was: {temp}" : "Error delete Payment";
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private async Task<string> DeleteRegisterAsync(int idReg)
        {
            try
            {
                string result = "";
                if (await OpenConnectionAsync())
                {
                    SqlCommand myCommand = connection.CreateCommand();
                    myCommand.CommandText = CommandGlobal.DeleteRegister;
                    myCommand.Parameters.Add("@id", sqlDbType: SqlDbType.Int);
                    myCommand.Parameters["@id"].Value = idReg;

                    int r = await myCommand.ExecuteNonQueryAsync();
                    result = (r == 1) ? "OK" : "Fail";
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        private async Task<string> DeleteConsolidateRegisterAsync(int idReg, string typePayment)
        {
            try
            {
                string result = "";
                if (await OpenConnectionAsync())
                {
                    switch (typePayment)
                    {
                        case "Day":
                        {
                            SqlCommand myCommand = connection.CreateCommand();
                            myCommand.CommandText = CommandGlobal.DeletePaymentDay;
                            myCommand.Parameters.Add("@idReg", sqlDbType: SqlDbType.Int);
                            myCommand.Parameters["@idReg"].Value = idReg;

                            int r = await myCommand.ExecuteNonQueryAsync();
                            result = (r == 1) ? "OK" : "Fail";
                            return result;
                        }
                        case "Week":
                        {
                            SqlCommand myCommand = connection.CreateCommand();
                            myCommand.CommandText = CommandGlobal.DeletePaymentWeek;
                            myCommand.Parameters.Add("@idReg", sqlDbType: SqlDbType.Int);
                            myCommand.Parameters["@idReg"].Value = idReg;

                            int r = await myCommand.ExecuteNonQueryAsync();
                            result = (r == 1) ? "OK" : "Fail";
                            return result;
                        }
                        case "Month":
                        {
                            SqlCommand myCommand = connection.CreateCommand();
                            myCommand.CommandText = CommandGlobal.DeletePaymentMonth;
                            myCommand.Parameters.Add("@idReg", sqlDbType: SqlDbType.Int);
                            myCommand.Parameters["@idReg"].Value = idReg;

                            int r = await myCommand.ExecuteNonQueryAsync();
                            result = (r == 1) ? "OK" : "Fail";
                            return result;
                        }
                        default:
                        {
                            return "Doesn't allow operation";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (connection != null) { CloseConnection(); }
            }
        }
        private static void SetParamsPayment(Payment payment, ref SqlCommand myCommand)
        {
            try
            {
                myCommand.Parameters.Add("@name", SqlDbType.VarChar);
                myCommand.Parameters.Add("@description", SqlDbType.VarChar);
                myCommand.Parameters.Add("@typeCatalogue", SqlDbType.VarChar);
                myCommand.Parameters.Add("@valuePay", SqlDbType.Float);
                myCommand.Parameters.Add("@currency", SqlDbType.VarChar);
                myCommand.Parameters.Add("@dateBuy", SqlDbType.DateTime2);
                myCommand.Parameters.Add("@timezone", SqlDbType.VarChar);
                myCommand.Parameters.Add("@support", SqlDbType.VarChar);
                myCommand.Parameters.Add("@username", SqlDbType.VarChar);

                myCommand.Parameters["@name"].Value = payment.Name;
                myCommand.Parameters["@description"].Value = payment.Description;
                myCommand.Parameters["@typeCatalogue"].Value = payment.TypeCatalogue;
                myCommand.Parameters["@valuePay"].Value = payment.ValuePay;
                myCommand.Parameters["@currency"].Value = payment.Currency;
                myCommand.Parameters["@dateBuy"].Value = payment.DatePay;
                myCommand.Parameters["@timezone"].Value = payment.Timezone;
                myCommand.Parameters["@support"].Value = payment.Support;
                myCommand.Parameters["@username"].Value = GlobalVariable.CredentialUser.Username;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting params is: {ex.Message}");
            }
        }
        private static void CreateUpdateQuery(string name, bool condition, bool end, ref string updateQuery)
        {
            try
            {
                if (condition)
                {
                    updateQuery += $"[{name}] = @{name}";

                    if (end == false)
                    {
                        updateQuery += ", ";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private static void SetUpdateParams(string name, bool condition, object value, ref SqlCommand myCommand, SqlDbType typeData)
        {
            if (condition)
            {
                myCommand.Parameters.Add($"@{name}", sqlDbType: typeData);
                myCommand.Parameters[$"@{name}"].Value = value;
            }
        }

    }
}