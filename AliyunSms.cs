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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.DySDKLite
{
    /// <summary>
    /// 阿里云短信
    /// https://www.aliyun.com/product/sms?spm=5176.8142029.388261.321.3836dbccTHaG24
    /// </summary>
    public class AliyunSms
    {
        public static string SendSms()
        {
            // *** 需用户填写部分 ***

            //fixme 必填: 请参阅 https://ak-console.aliyun.com/ 取得您的AK信息
           
            string accessKeyId = "your access key id";//你的accessKeyId，参考本文档步骤2
            string accessKeySecret = "your access key secret";//你的accessKeySecret，参考本文档步骤2

            Dictionary<string, string> smsDict = new Dictionary<string, string>();

            //fixme 必填: 短信接收号码
            smsDict.Add("PhoneNumbers", "17000000000");

            //fixme 必填: 短信签名，应严格按"签名名称"填写，请参考: https://dysms.console.aliyun.com/dysms.htm#/develop/sign
            smsDict.Add("SignName", "短信签名");

            //fixme 必填: 短信模板Code，应严格按"模板CODE"填写, 请参考: https://dysms.console.aliyun.com/dysms.htm#/develop/template

            smsDict.Add("TemplateCode", "SMS_0000001");
            
            // fixme 可选: 设置模板参数, 假如模板中存在变量需要替换则为必填项
            smsDict.Add("TemplateParam", JsonConvert.SerializeObject(new { code = "12345", product = "阿里云短信" }));

            //什么？Newtonsoft.Json也觉得重，那拼字符串好了
            //smsDict.Add("TemplateParam", "{\"appname\":\"微关爱\",\"appstorename\":\"小黑\"}");



            // *** 以下内容无需修改 ***
            smsDict.Add("RegionId", "cn-hangzhou");
            smsDict.Add("Action", "SendSms");
            smsDict.Add("Version", "2017-05-25");

            string domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）

            // 初始化SignatureHelper实例用于设置参数，签名以及发送请求
            var singnature = new SignatureHelper();

            return singnature.Request(accessKeyId, accessKeySecret, domain, smsDict);

        }
    }
}
