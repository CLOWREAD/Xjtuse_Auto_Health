using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace Xjtuse
{
    class Program
    {
        static void Main(string[] args)
        {


            String open_Platform_User="";
            String memberId = "";



            var url = String.Format("https://org.xjtu.edu.cn/openplatform/g/admin/login");
            HttpClient hc = new HttpClient();

            JsonHelper.LoginData login = new JsonHelper.LoginData();
            String json = JsonHelper.ToJson(login, login.GetType());
            HttpContent hcn = new StringContent(json);
            hc.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");



            hc.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            hc.DefaultRequestHeaders.Add("Host", "org.xjtu.edu.cn");
            hc.DefaultRequestHeaders.Add("Origin", "https://org.xjtu.edu.cn");
            hc.DefaultRequestHeaders.Add("Cookie", "JSESSIONID=CB1E52D271C3340D12138EB48264F059; app_info_urla=+L4rB7r6yuXaNSyCPpJbv37GCbbuXhbEWT4VeSdUB0t3gGBeTqRkJBRt8gPP6QhIagz7j2YszjF+xgm27l4WxAwDfhbcm4IHg0D0RbEwhPtQiOxvrAoBPw==; cur_appId_=gvqfQIq76FQ=; state=xjdCas; sid_code=workbench_login_jcaptcha_29FA2016E42FE90528C5F041212771BF; Path=/; route=03126b510bdd0e2627ce6362c7b8ffc4");


            var hcres_get = hc.GetAsync("http://one2020.xjtu.edu.cn/EIP/user/index.htm");
                hcres_get.Wait();
            var reader_get = hcres_get.Result.Content.ReadAsStringAsync();
            //hcn.Headers.Add("Connection","Keep-Alive");
            //hcn.Headers.Add("User-Agent","Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 70.0.3538.102 Safari / 537.36 Edge / 18.18362");
            hcn.Headers.Add("Cookie", "JSESSIONID=CB1E52D271C3340D12138EB48264F059; app_info_urla=+L4rB7r6yuXaNSyCPpJbv37GCbbuXhbEWT4VeSdUB0t3gGBeTqRkJBRt8gPP6QhIagz7j2YszjF+xgm27l4WxAwDfhbcm4IHg0D0RbEwhPtQiOxvrAoBPw==; cur_appId_=gvqfQIq76FQ=; state=xjdCas; sid_code=workbench_login_jcaptcha_29FA2016E42FE90528C5F041212771BF; Path=/; route=03126b510bdd0e2627ce6362c7b8ffc4");
            //HttpContent.Response.Cookies.Append("getCookie", "setCookieValue");

            hcn.Headers.ContentType.MediaType = "application/json";
            hcn.Headers.ContentType.CharSet = "utf-8";
            //.Add("Content-Type", "application/json");
            var hcres = hc.PostAsync(url,hcn);
            
            hcres.Wait();
            var reader = hcres.Result.Content.ReadAsStringAsync();
            reader.Wait();
            String ctxstr = String.Format("{0}", reader.Result);

            
            //dynamic LoginResult = JsonHelper.FromJson( reader.Result, typeof(Object));

            var LoginResult = JsonConvert.DeserializeObject<dynamic>(ctxstr);

            open_Platform_User = LoginResult.data.tokenKey;

            memberId = LoginResult.data.orgInfo.memberId;

            JsonHelper.GetUserIdentity json_GetUserIdentity = new JsonHelper.GetUserIdentity();
            json_GetUserIdentity.memberId = LoginResult.data.orgInfo.memberId;
            String str_GetUserIdentity = JsonHelper.ToJson(json_GetUserIdentity, json_GetUserIdentity.GetType());
            HttpContent hcn_GetUserIdentity = new StringContent(str_GetUserIdentity);
            hcn_GetUserIdentity.Headers.ContentType.MediaType = "application/json";
            hcn_GetUserIdentity.Headers.ContentType.CharSet = "utf-8";
            String getidentityurl = String.Format("https://org.xjtu.edu.cn/openplatform/g/admin/getUserIdentity?memberId={0}", LoginResult.data.orgInfo.memberId);


           

            var hcres_getUserIdentity = hc.GetAsync(getidentityurl);
            
            hcres_getUserIdentity.Wait();
            var reader_GetUserIdentity = hcres_getUserIdentity.Result.Content.ReadAsStringAsync();
            String ctxstr_GetUserIdentity = String.Format("{0}", reader_GetUserIdentity.Result);


            //dynamic LoginResult = JsonHelper.FromJson( reader.Result, typeof(Object));

            var GetUserIdentityResult = JsonConvert.DeserializeObject<dynamic>(ctxstr_GetUserIdentity);


            //redirectUrl
        
            String redirectUrlurl = String.Format("https://org.xjtu.edu.cn/openplatform/oauth/auth/getRedirectUrl?userType={0}&personNo={1}",
                GetUserIdentityResult.data[0].userType, GetUserIdentityResult.data[0].personNo);

            hc.DefaultRequestHeaders.Remove("Cookie");

            String cookie = String.Format("open_Platform_User={0}; memberId={1};app_info_urla=+L4rB7r6yuXaNSyCPpJbv37GCbbuXhbEWT4VeSdUB0t3gGBeTqRkJBRt8gPP6QhIagz7j2YszjF+xgm27l4WxAwDfhbcm4IHg0D0RbEwhPtQiOxvrAoBPw==; cur_appId_=gvqfQIq76FQ=; state=xjdCas; sid_code=workbench_login_jcaptcha_29FA2016E42FE90528C5F041212771BF; Path=/; route=03126b510bdd0e2627ce6362c7b8ffc4"
                , open_Platform_User,memberId);
            hc.DefaultRequestHeaders.Add("Cookie", cookie);
            var hcres_redirectUrl = hc.GetAsync(redirectUrlurl);

            hcres_redirectUrl.Wait();
            var reader_redirectUrl = hcres_redirectUrl.Result.Content.ReadAsStringAsync();
            String ctxstr_redirectUrl = String.Format("{0}", reader_redirectUrl.Result);


            var redirectUrlResult = JsonConvert.DeserializeObject<dynamic>(ctxstr_redirectUrl);


            String loginsucsessurl=redirectUrlResult.data;
            //loginsucsessurl = loginsucsessurl.Replace("\"", "");


           var hch= new HttpClientHandler()
            {
                    AllowAutoRedirect = true,
            };

            hch.CookieContainer.Add(new Cookie() { Name = "open_Platform_User",Domain= "org.xjtu.edu.cn", Value = open_Platform_User });
            hch.CookieContainer.Add(new Cookie() { Name = "memberId", Domain = "org.xjtu.edu.cn", Value = memberId });
            var red_hc= new HttpClient(hch);
            red_hc.DefaultRequestHeaders.Add("Cookie", cookie);

            var hcres_LoginSuccess = red_hc.GetAsync(loginsucsessurl);

            var cookiecollection= hch.CookieContainer.GetCookies(new Uri("https://cas.xjtu.edu.cn"));

            hcres_LoginSuccess.Wait();
            hcres_LoginSuccess.Result.EnsureSuccessStatusCode();


            var reader_LoginSuccess = hcres_LoginSuccess.Result.Content.ReadAsStringAsync();
            String ctxstr_LoginSuccess = String.Format("{0}", reader_LoginSuccess.Result);


            var LoginSuccessResult = JsonConvert.DeserializeObject<dynamic>(ctxstr_LoginSuccess);


            
        }


    }
}
