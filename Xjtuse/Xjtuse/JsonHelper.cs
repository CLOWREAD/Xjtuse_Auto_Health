using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Xjtuse
{
    public class JsonHelper

    {
        [DataContract]
        public class LoginData
        {
            [DataMember]
            public String pwd = "abfCq2KQVTUNm7ejowenOQ==";
            [DataMember]
            public String username = "clow_read";
            [DataMember]
            public int loginType = 1;
            [DataMember]
            public String jcaptchaCode = "";
        }

  //         "code": 0,
  // "message": "成功",
  // "data": {
  //     "tokenKey": "user_token_86604abc-e28d-4a78-8ec4-9a6fce2ac090",
  //     "orgInfo": {
  //         "logo": null,
  //         "orgId": 1000,
  //         "orgName": "西安交通大学",
  //         "memberId": 699540,
  //         "firstLogin": 1,
  //         "isIdentification": 2,
  //         "memberName": null,
  //         "addNew": 2
  //     },
  //     "state": "xjdCas"
  //


        [DataContract]
        public class LoginResult
        {
            [DataMember]
            public int code = 0;
            [DataMember]
            public String message = "";
            [DataMember]
            public String state = "";

        }
        [DataContract]
        public class GetUserIdentity
        {
            [DataMember]
            public String memberId = "";

        }
        public static string ToJson(Object obj, Type type)

        {



            MemoryStream ms = new MemoryStream();



            DataContractJsonSerializer seralizer = new DataContractJsonSerializer(type);





            seralizer.WriteObject(ms, obj);

            ms.Seek(0, SeekOrigin.Begin);



            StreamReader sr = new StreamReader(ms);

            string jsonstr = sr.ReadToEnd();



            //jsonstr = jsonstr.Replace("\"", "\\\"");



            sr.Close();

            ms.Close();

            return jsonstr;

        }

        public static Object FromJson(String jsonstr, Type type)

        {



            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonstr));



            DataContractJsonSerializer seralizer = new DataContractJsonSerializer(type);



            ms.Seek(0, SeekOrigin.Begin);



            Object res = seralizer.ReadObject(ms);





            ms.Close();

            return res;

        }



    }
}
