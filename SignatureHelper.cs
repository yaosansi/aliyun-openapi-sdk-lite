/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aliyun.DySDKLite
{
    /// <summary>
    /// 签名助手
    /// https://help.aliyun.com/document_detail/30079.html?spm=5176.7739992.2.3.HM7WTG
    /// 
    /// </summary>
    public class SignatureHelper
    {
        private const string ISO8601_DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        private const string ENCODING_UTF8 = "UTF-8";
        public static string PercentEncode(String value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            byte[] bytes = Encoding.GetEncoding(ENCODING_UTF8).GetBytes(value);
            foreach (char c in bytes)
            {
                if (text.IndexOf(c) >= 0)
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append("%").Append(
                        string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c));
                }
            }
            return stringBuilder.ToString();
        }
        public static string FormatIso8601Date(DateTime date)
        {
            return date.ToUniversalTime().ToString(ISO8601_DATE_FORMAT, CultureInfo.CreateSpecificCulture("en-US"));
        }

        private static IDictionary<string, string> SortDictionary(Dictionary<string, string> dic)
        {
            IDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dic, StringComparer.Ordinal);
            return sortedDictionary;
        }

        public static string SignString(string source, string accessSecret)
        {
            using (var algorithm = new HMACSHA1())
            {
                algorithm.Key = Encoding.UTF8.GetBytes(accessSecret.ToCharArray());
                return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(source.ToCharArray())));
            }
        }


        public async Task<string> HttpGet(string url)
        {
            string responseBody = string.Empty;
            using (var http = new HttpClient())
            {
                try
                {
                    http.DefaultRequestHeaders.Add("x-sdk-client", "Net/2.0.0");
                    var response = await http.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException !");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            return responseBody;

        }

        public string Request(string accessKeyId, string accessKeySecret, string domain, Dictionary<string, string> paramsDict, bool security = false)
        {

            string result = string.Empty;
            var apiParams = new Dictionary<string, string>();
            apiParams.Add("SignatureMethod", "HMAC-SHA1");
            apiParams.Add("SignatureNonce", Guid.NewGuid().ToString());
            apiParams.Add("SignatureVersion", "1.0");
            apiParams.Add("AccessKeyId", accessKeyId);
            apiParams.Add("Timestamp", FormatIso8601Date(DateTime.Now));
            apiParams.Add("Format", "JSON");

            foreach (var param in  paramsDict)
            {
                if (!apiParams.ContainsKey(param.Key))
                {
                    apiParams.Add(param.Key, param.Value);
                }
            }
            var sortedDictionary = SortDictionary(apiParams);
            string sortedQueryStringTmp = "";
            foreach (var param in sortedDictionary)
            {
                sortedQueryStringTmp += "&" + PercentEncode(param.Key) + "=" + PercentEncode(param.Value);
            }

            string stringToSign = "GET&%2F&" + PercentEncode(sortedQueryStringTmp.Substring(1));
            string sign = SignString(stringToSign, accessKeySecret + "&");
            string signature = PercentEncode(sign);
            string url = (security ? "https" : "http") + $"://{domain}/?Signature={signature}{sortedQueryStringTmp}";

            try
            {
                result= HttpGet(url).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return result;

        }
    }
}
