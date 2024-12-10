using Newtonsoft.Json;

namespace ExpertEvaluator.Entities {

    public class Criterion {

        [JsonProperty("id")]
        public int ID {
            get; set;
        }

        [JsonProperty("name")]
        public string Name {
            get; set;
        }

        public override string ToString() {
            return Name;
        }
    }
}
