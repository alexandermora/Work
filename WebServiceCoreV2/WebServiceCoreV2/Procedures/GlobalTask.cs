using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceCoreV2.Storage;
using WebServiceCoreV2.Storage.Sql;

namespace WebServiceCoreV2.Procedures
{
    public class GlobalTask
    {
        public string EncodeString (string value)
        {
            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string DecodeString (string value)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(value));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void ChartSqlArray()
        {
            try
            {

                GlobalVariable.ListFilters[0, 0, 0] = CommandGlobal.GetCountCurrency;
                GlobalVariable.ListFilters[0, 0, 1] = CommandGlobal.GetTotalCurrency;
                GlobalVariable.ListFilters[0, 1, 0] = CommandGlobal.GetCountCurrencyByDay;
                GlobalVariable.ListFilters[0, 1, 1] = CommandGlobal.GetTotalCurrencyByDay;
                GlobalVariable.ListFilters[0, 2, 0] = CommandGlobal.GetCountCurrencyByWeek;
                GlobalVariable.ListFilters[0, 2, 1] = CommandGlobal.GetTotalCurrencyByWeek;
                GlobalVariable.ListFilters[0, 3, 0] = CommandGlobal.GetCountCurrencyByMonth;
                GlobalVariable.ListFilters[0, 3, 1] = CommandGlobal.GetTotalCurrencyByMonth;

                GlobalVariable.ListFilters[1, 0, 0] = CommandGlobal.GetCountDateBuy;
                GlobalVariable.ListFilters[1, 0, 1] = CommandGlobal.GetTotalDateBuy;
                GlobalVariable.ListFilters[1, 1, 0] = CommandGlobal.GetCountDateBuyByDay;
                GlobalVariable.ListFilters[1, 1, 1] = CommandGlobal.GetTotalDateBuyByDay;
                GlobalVariable.ListFilters[1, 2, 0] = CommandGlobal.GetCountDateBuyByWeek;
                GlobalVariable.ListFilters[1, 2, 1] = CommandGlobal.GetTotalDateBuyByWeek;
                GlobalVariable.ListFilters[1, 3, 0] = CommandGlobal.GetCountDateBuyByMonth;
                GlobalVariable.ListFilters[1, 3, 1] = CommandGlobal.GetTotalDateBuyByMonth;

                GlobalVariable.ListFilters[2, 0, 0] = CommandGlobal.GetCountTimezone;
                GlobalVariable.ListFilters[2, 0, 1] = CommandGlobal.GetTotalTimezone;
                GlobalVariable.ListFilters[2, 1, 0] = CommandGlobal.GetCountTimezoneByDay;
                GlobalVariable.ListFilters[2, 1, 1] = CommandGlobal.GetTotalTimezoneByDay;
                GlobalVariable.ListFilters[2, 2, 0] = CommandGlobal.GetCountTimezoneByWeek;
                GlobalVariable.ListFilters[2, 2, 1] = CommandGlobal.GetTotalTimezoneByWeek;
                GlobalVariable.ListFilters[2, 3, 0] = CommandGlobal.GetCountTimezoneByMonth;
                GlobalVariable.ListFilters[2, 3, 1] = CommandGlobal.GetTotalTimezoneByMonth;

                GlobalVariable.ListFilters[3, 0, 0] = CommandGlobal.GetCountTypeCatalogue;
                GlobalVariable.ListFilters[3, 0, 1] = CommandGlobal.GetTotalTypeCatalogue;
                GlobalVariable.ListFilters[3, 1, 0] = CommandGlobal.GetCountTypeCatalogueByDay;
                GlobalVariable.ListFilters[3, 1, 1] = CommandGlobal.GetTotalTypeCatalogueByDay;
                GlobalVariable.ListFilters[3, 2, 0] = CommandGlobal.GetCountTypeCatalogueByWeek;
                GlobalVariable.ListFilters[3, 2, 1] = CommandGlobal.GetTotalTypeCatalogueByWeek;
                GlobalVariable.ListFilters[3, 3, 0] = CommandGlobal.GetCountTypeCatalogueByMonth;
                GlobalVariable.ListFilters[3, 3, 1] = CommandGlobal.GetTotalTypeCatalogueByMonth;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
