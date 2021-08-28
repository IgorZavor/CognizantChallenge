using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace CognizantChallenge.Compilers.Joodle
{
    public class JdoodleCompiler: ICompiler
    {
        private const string _clientId = "f0b80cc341ce8d3e6f6770342cffcf15";
        private const string _clientSecret = "f1f07078262484a327fb4347d5e7a0b085a325003c2ee5ef8bbd0c6c59ebf6b1";
        private const string versionIndex = "0";

        public CompileResponse Run(string script, string language)
        {
            var request =(HttpWebRequest) WebRequest.Create("https://api.jdoodle.com/v1/execute");
            request.Method = "POST";
            request.ContentType = "application/json";
            script = script.Replace("\t", "").Replace("\n", "").Replace("\r", "");
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var input = "{\"clientId\": \"" + _clientId + "\",\"clientSecret\":\"" + _clientSecret + "\",\"script\":\"" + script +
                            "\",\"language\":\"" + language + "\",\"versionIndex\":\"" + versionIndex + "\"} ";

                streamWriter.Write(input);
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            var response = JsonConvert.DeserializeObject<CompileResponse>(result);

            return response;
        }
    }
}