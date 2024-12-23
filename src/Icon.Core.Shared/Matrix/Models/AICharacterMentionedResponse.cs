using System.Collections.Generic;
using Newtonsoft.Json;

namespace Icon.Matrix.AIManager.CharacterMentioned
{
    public class AICharacterMentionedResponse
    {
        [JsonProperty("tagsFound")]
        public List<string> TagsFound { get; set; } = new List<string>();

        [JsonProperty("cryptocoinsFound")]
        public List<string> CryptocoinsFound { get; set; } = new List<string>();

        [JsonProperty("actionsToTake")]
        public List<AIAction> ActionsToTake { get; set; } = new List<AIAction>();

        [JsonProperty("resultToPost")]
        public string ResultToPost { get; set; }

        [JsonProperty("rawResponse")]
        public string RawResponse { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("exception")]
        public string Exception { get; set; }
        [JsonProperty("exceptionMessage")]
        public string ExceptionMessage { get; set; }

        [JsonProperty("scores")]
        public AIScores Scores { get; set; } = new AIScores();
    }

    public class AIScores
    {

        [JsonProperty("relevance")]
        public int Relevance { get; set; }

        [JsonProperty("depth")]
        public int Depth { get; set; }

        [JsonProperty("sentiment")]
        public int Sentiment { get; set; }

        // [JsonProperty("novelty")]
        // public int Novelty { get; set; }

        // [JsonProperty("aiTrainingValue")]
        // public int AITrainingValue { get; set; }
    }

    public class AIAction
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
    }
}
