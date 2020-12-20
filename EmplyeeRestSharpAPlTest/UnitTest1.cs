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
        [TestMethod]
        public void whileAddingMultiEmployees_OnPost_ShouldRetuenAddedEmployee()
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee { name = "Anitha", salary = "30000" });
            employees.Add(new Employee { name = "Jaipal", salary = "40000" });
            foreach (Employee employee in employees)
            {

                RestRequest request = new RestRequest("/employees", Method.POST);
                JObject jObjectbody = new JObject();
                jObjectbody.Add("name", employee.name);
                jObjectbody.Add("salary", employee.salary);

                request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employee.name, dataResponse.name);
            }
        }
        [TestMethod]
        public void whileUpdatingEmployee_OnPut_shouldReturnUpdateEmployee()
        {
            RestRequest request = new RestRequest("/employees/12", Method.PUT);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name","Bhagyalaxmi");
            jObjectbody.Add("salary", "50000");


            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Bhagyalaxmi", dataResponse.name);
            Assert.AreEqual("50000",dataResponse.salary);

        }

    }
}
