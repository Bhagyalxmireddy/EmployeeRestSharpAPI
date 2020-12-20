using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace EmplyeeRestSharpAPlTest
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void setUp()
        {
            client = new RestClient(" http://localhost:4000");
        }
        private IRestResponse getEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/employees", Method.GET);
            //act
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void WhileCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(11, dataResponse.Count);
        }
        [TestMethod]
        public void whileAddingEmployee_OnPost_ShouldRetuenAddedEmployee()
        {
             RestRequest request = new RestRequest("/employees", Method.POST);
             JObject jObjectbody = new JObject();
             jObjectbody.Add("name", "Bhagyalaxmi");
             jObjectbody.Add("salary", "20000");

             request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

             IRestResponse response = client.Execute(request);

             Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
             Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
             Assert.AreEqual("Bhagyalaxmi", dataResponse.name);
             Assert.AreEqual("20000", dataResponse.salary);

        }
    }
}
