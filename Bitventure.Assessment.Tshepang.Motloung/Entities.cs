using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitventure.Assessment.Tshepang.Motloung
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Response
    {
        public string element { get; set; }
        public string identifier { get; set; }
        public string regex { get; set; }
    }

    public class Endpoint
    {
        public bool enabled { get; set; }
        public string resource { get; set; }
        public List<Response> response { get; set; }
        public string requestBody { get; set; }
    }

    public class Identifier
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class Service
    {
        public string baseURL { get; set; }
        public string datatype { get; set; }
        public bool enabled { get; set; }
        public List<Endpoint> endpoints { get; set; }
        public List<Identifier> identifiers { get; set; }
    }

    public class Root
    {
        public List<Service> services { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class Response
    //{
    //    public string element { get; set; }
    //    public string regex { get; set; }
    //    public string identifier { get; set; }
    //}

    //public class Endpoint
    //{
    //    public bool enabled { get; set; }
    //    public string resource { get; set; }
    //    public List<Response> response { get; set; }
    //}

    //public class Service
    //{
    //    public string baseURL { get; set; }
    //    public bool enabled { get; set; }
    //    public List<Endpoint> endpoints { get; set; }
    //}

    //public class Root
    //{
    //    public List<Service> services { get; set; }
    //}


}
