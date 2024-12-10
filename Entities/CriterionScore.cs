using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExpertEvaluator.Entities {

    public class CriterionScore {

        [JsonProperty("criterion")]
        public Criterion Criterion {
            get; set;
        }

        [JsonProperty("expert")]
        public Expert Expert {
            get; set;
        }

        [JsonProperty("subject")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Subject Subject {
            get; set;
        }

        [JsonProperty("rank")]
        public int Rank {
            get; set;
        }

        [JsonProperty("score")]
        public int Score {
            get; set;
        }
    }
}
