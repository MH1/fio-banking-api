﻿using System;
using System.Net;
using System.Text;

namespace Mach.Banking.Fio
{
    public class FioBank
    {
        private const string _base = "https://www.fio.cz/ib_api/rest/";
        private string _token;

        public FioBank(string token)
        {
            _token = token;
        }

        public void GetTransactions(DateTime from, DateTime to)
        {
            WebClient wc = new WebClient();
            string address = BuildAddress("periods", "transactions", from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"));
            string result = wc.DownloadString(address);

            JsonSerializer serializer = new JsonSerializer();
            AccountStatementResponse statement = serializer.Deserialize<AccountStatementResponse>(result);
        }

        private string BuildAddress(string action, string document, params string[] parameters)
        {
            StringBuilder address = new StringBuilder();
            address.Append(_base);
            address.Append(action);
            address.Append('/');
            address.Append(_token);
            address.Append('/');

            if (parameters != null)
            {
                foreach (string parameter in parameters)
                {
                    address.Append(parameter);
                    address.Append('/');
                }
            }

            if (document != null)
            {
                address.Append(document);
                address.Append(".json");
            }

            return address.ToString();
        }
    }
}
