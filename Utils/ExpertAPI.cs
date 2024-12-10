using ExpertEvaluator.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ExpertEvaluator.Utils {

    public class ExpertAPI : RestClient {

        public static ExpertAPI API {
            get; private set;
        }

        public ExpertAPI(string url) : base(url) {
            API = this;
            Encoding = Encoding.UTF8;
        }

        public T Request<T>(RestRequest request) {
            object response = null;
            try {
                string responseStr = Execute(request).Content;
                if (typeof(T) == typeof(JObject)) {
                    response = JObject.Parse(responseStr);
                } else if (typeof(T) == typeof(JArray)) {
                    response = JArray.Parse(responseStr);
                } else {
                    response = Convert.ChangeType(responseStr, typeof(T));
                }
            } catch {
                DialogResult result = MessageBox.Show(
                    "Не удалось получить ответ от сервера.", "",
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry) {
                    response = Request<T>(request);
                }
            }
            return (T) response;
        }

        public async void RequestAsync<T>(RestRequest request, Action<T> task) {
            object response = null;
            try {
                IRestResponse resp = await ExecuteAsync(request);
                string responseStr = resp.Content;
                if (typeof(T) == typeof(JObject)) {
                    response = JObject.Parse(responseStr);
                } else if (typeof(T) == typeof(JArray)) {
                    response = JArray.Parse(responseStr);
                } else {
                    response = Convert.ChangeType(responseStr, typeof(T));
                }
                if (task != null) {
                    task.Invoke((T) response);
                }
            } catch {
                DialogResult result = MessageBox.Show(
                    "Не удалось получить ответ от сервера.", "",
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry) {
                    RequestAsync(request, task);
                }
            }
        }

        public int Login(string name) {
            RestRequest request = new RestRequest("experts", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new {
                id = 0, name
            });
            JObject json = Request<JObject>(request);
            if (json.ContainsKey("code") && json.Value<int>("code") == 400) {
                return -1;
            }
            return json.Value<int>("id");
        }

        public List<Criterion> GetCriteria() {
            RestRequest request = new RestRequest("criteria", Method.GET);
            return Request<JArray>(request).ToObject<List<Criterion>>();
        }

        public void SendEvaluation(List<CriterionScore> scores) {
            RestRequest request = new RestRequest("scores", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(JsonConvert.SerializeObject(scores));
            Request<object>(request);
        }

        public double Calculate(Subject subject) {
            RestRequest request = new RestRequest("scores/calculate", Method.GET);
            request.AddParameter("subject", subject.ToString());
            return double.Parse(Request<string>(request).Replace(".", ","));
        }

        public Statistics GetStatistics() {
            RestRequest request = new RestRequest("scores/statistics", Method.GET);
            return Request<JObject>(request).ToObject<Statistics>();
        }
    }
}
