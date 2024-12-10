using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpertEvaluator.Entities {

    public class Statistics {

        [JsonProperty("experts")]
        public int Experts {
            get; set;
        }

        [JsonProperty("countries")]
        public Dictionary<string, int> Countries {
            get; set;
        }

        [JsonProperty("concordance")]
        public double Concordance {
            get; set;
        }
    }
}
