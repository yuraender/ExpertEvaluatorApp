using Newtonsoft.Json;

namespace ExpertEvaluator.Entities {

    public class Expert {


        [JsonProperty("id")]
        public int ID {
            get; set;
        }
    }
}
