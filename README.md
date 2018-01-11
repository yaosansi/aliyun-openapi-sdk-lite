
# 阿里云短信服务接口`.Net Core`轻量版

## 前言

项目环境使用 `.NET Core 2.0`，由于阿里云官方SDK目前（2018-01-12）版本尚未支持 `.NET Core` 环境，`github` 找到 [aliyun-openapi-sdk-net-core](https://github.com/aliyun/aliyun-openapi-net-sdk)，经测试使用的SDK已是旧版本，无法正常工作了。

阿里云官方SDK[回复](https://github.com/aliyun/aliyun-openapi-net-sdk/issues/11)下个版本支持 `.NET Core` 环境。

## 说明

由于并未阅读官方接口文档，本项目主要参考[阿里云官方PHPSDK轻量版](http://ytx-sdk.oss-cn-shanghai.aliyuncs.com/dysmsapi-lite-php.zip?spm=5176.doc55359.2.5.cdi7jO&file=dysmsapi-lite-php.zip)，改写为 `.NET Core 2.0` 版本，经测试运行正常，其中加密等方法均参考官方SDK。（详见参考和引用部分）

## 环境要求

- 支持 .Net Core 2.0 及以上版本；
- 直接复制代码文件即可，或自行改为单文件版本。

## 示例

```
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
```


## 参考和引用

- [阿里云API调用签名机制](https://help.aliyun.com/document_detail/30079.html?spm=5176.7739992.2.3.HM7WTG)

- [阿里云官方.NET SDK](https://github.com/aliyun/aliyun-openapi-net-sdk)

- [Open API SDK for .Net Core developers](https://github.com/VAllens/aliyun-openapi-sdk-net-core)

- [阿里云官方PHPSDK轻量版](http://ytx-sdk.oss-cn-shanghai.aliyuncs.com/dysmsapi-lite-php.zip?spm=5176.doc55359.2.5.cdi7jO&file=dysmsapi-lite-php.zip)


## Authors

[yaosansi](https://github.com/yaosansi)

## License

licensed under the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0.html)
