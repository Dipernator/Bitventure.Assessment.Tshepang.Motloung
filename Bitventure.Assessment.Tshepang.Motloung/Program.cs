using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitventure.Assessment.Tshepang.Motloung
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Processing basic_endpoints.json ...");
            BasicEndPoint("basic_endpoints.json");
            Console.WriteLine("Press a key to continue with bonus_endpoints");
            Console.ReadLine();
            Console.WriteLine("Processing bonus_endpoints.json ...");
            BosnusEndPoint("bonus_endpoints.json");
            Console.WriteLine("Press a key to continue exit");
            Console.ReadLine();
        }

        private static void BasicEndPoint(string fileName)
        {
            Root root = GetJsonFile(fileName);

            if (root == null)
            {
                Console.WriteLine("No Endpoint found");
                Console.ReadLine();
                return;
            }

            if (root.services == null)
            {
                Console.WriteLine("No Service found");
                Console.ReadLine();
                return;
            }

            foreach (Service service in root.services)
            {
                if (!service.enabled)
                {
                    continue; // A flag to denoting if we should initiate calls to this service or skip it. 
                }

                service.identifiers = new List<Identifier>
                {
                    new Identifier()
                    {
                        key = "Nothing",
                        value = "Nothing"
                    }
                };

                ProcessService(service);
            }
        }

        private static void BosnusEndPoint(string fileName)
        {
            Root root = GetJsonFile(fileName);

            if (root == null)
            {
                Console.WriteLine("No Endpoint found");
                Console.ReadLine();
                return;
            }

            if (root.services == null)
            {
                Console.WriteLine("No Service found");
                Console.ReadLine();
                return;
            }

            foreach (Service service in root.services)
            {
                if (!service.enabled)
                {
                    continue; // A flag to denoting if we should initiate calls to this service or skip it. 
                }

                ProcessService(service);
            }
        }

        private static void ProcessService(Service service)
        {
            foreach (Endpoint endpoints in service.endpoints)
            {
                if (!endpoints.enabled)
                {
                    continue; // A flag to denoting if we should initiate calls to this service or skip it. 
                }

                IDictionary<string, object> restCallResult = null;

                if (service.datatype != null)
                {
                    service.datatype.ToUpper();
                }
                else
                {
                    service.datatype = "JSON";
                }

                // Check if json or xml, the make rest call, default to json  
                switch (service.datatype)
                {
                    case "JSON":
                        restCallResult = RestCallJson($"{service.baseURL}{endpoints.resource}");
                        break;
                    case "XML":
                        restCallResult = RestCallXml($"{service.baseURL}{endpoints.resource}");
                        break;
                    default:
                        restCallResult = RestCallJson($"{service.baseURL}{endpoints.resource}");
                        break;
                }

                if (restCallResult == null) {
                    Console.WriteLine($"Unable to connect to {service.baseURL}{endpoints.resource}");
                    break;
                }

                foreach (Identifier identifier in service.identifiers)
                {
                    //if (identifier == null)
                    //{
                    //    break;
                    //}

                    foreach (Response response in endpoints.response)
                    {
                        KeyValuePair<string, object> something = restCallResult.Where(m => m.Key == response.element).FirstOrDefault();

                        if (something.Key == default && something.Value == default)
                        {
                            something = CleanUp(restCallResult, response.element).FirstOrDefault();
                        }

                        //if (response.identifier == null)
                        //{
                        if (response.identifier == identifier.key)
                        {
                            if (Regex(something.Value.ToString(), identifier.value))
                            {
                                Console.WriteLine($" √ => {something.Key}, {something.Value}");
                            }
                        }
                        else if (Regex(response.regex, something.Value.ToString()))
                        {
                            Console.WriteLine($" √ => {something.Key}, {something.Value}");
                        }
                        else
                        {
                            Console.WriteLine($" X => {something.Key}, {something.Value}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the child object that on contains our element
        /// </summary>
        /// <param name="data"></param>
        /// <param name="element"></param>
        /// <returns>IDictionary</returns>
        private static IDictionary<string, object> CleanUp(IDictionary<string, object> data, string element)
        {
            try
            {
                foreach (var child in data)
                {
                    KeyValuePair<string, object> b = data.Where(m => m.Value.ToString().Contains(element)).FirstOrDefault();
                    if (b.Value == default && b.Key == default)
                    {
                        continue;
                    }

                    return Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(b.Value.ToString());
                }

                return default;
            }
            catch (Exception ex) {
                return null;
            }
        }


        #region Define as Rest calls
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private static IDictionary<string, object> RestCallJson(string endPoint)
        {
            try
            {
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    webClient.Headers.Clear();
                    webClient.Headers.Add(System.Net.HttpRequestHeader.ContentType, "application/json");
                    string response = webClient.DownloadString(endPoint);
                    Console.WriteLine($"\n-------------------------------------");
                    Console.WriteLine($"EndPoint {endPoint}");
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(response);
                }
            }
            catch (Exception ex)
            {
                //using (System.IO.StreamReader streamReader = new System.IO.StreamReader($"../../Data/Test/groot.json"))
                //{
                //    string json = streamReader.ReadToEnd();
                //    return Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                //}

                //if (ex.Message.ToUpper().Contains("UNDERLYING CONNECTION WAS CLOSED"))
                //{
                //    using (System.IO.StreamReader streamReader = new System.IO.StreamReader($"../../Data/Test/people_1.json"))
                //    {
                //        string json = streamReader.ReadToEnd();
                //        return Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                //    }
                //}
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns>IDictionary</returns>
        private static IDictionary<string, object> RestCallXml(string endPoint)
        {
            try
            {
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    webClient.Headers.Clear();
                    webClient.Headers.Add(System.Net.HttpRequestHeader.ContentType, "application/json");
                    string response = webClient.DownloadString(endPoint);
                    Console.WriteLine($"\n-------------------------------------");
                    Console.WriteLine($"EndPoint {endPoint}");

                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(response);

                    // only return the last chil i.e remove xml and geoPlugin
                    string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc.LastChild, Newtonsoft.Json.Formatting.None, true);

                    return Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Compare Regex with string 
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="str"></param>
        /// <returns>bool</returns>
        private static bool Regex(string regex, string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex($@"{regex}");
            return r.IsMatch(str);
        }

        /// <summary>
        /// Get json file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Root obj</returns>
        private static Root GetJsonFile(string fileName)
        {
            try
            {
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader($"../../Data/{fileName}"))
                {
                    string json = streamReader.ReadToEnd();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(json);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
