using System;

namespace Icon.Matrix
{
    public static class MemoryProcessFlows
    {
        public static readonly MemoryProcessFlow CharacterMentionedTweet = new MemoryProcessFlow
        {
            Steps = new[]
            {
                new FlowStepDefinition
                {
                    StepName = nameof(MemoryManager.ReplyMentionedTweetDetermineProcessWorkflow),
                    MethodName = nameof(MemoryManager.ReplyMentionedTweetDetermineProcessWorkflow),
                    ParametersJson = null,
                    MaxRetries = 0,
                    Dependencies = Array.Empty<string>() // No dependencies
                },
                new FlowStepDefinition
                {
                    StepName = nameof(MemoryManager.ReplyMentionedTweetGatherContextStoredInDb),
                    MethodName = nameof(MemoryManager.ReplyMentionedTweetGatherContextStoredInDb),
                    MaxRetries = 0,
                    Dependencies = new[] { nameof(MemoryManager.ReplyMentionedTweetDetermineProcessWorkflow) }
                },
                new FlowStepDefinition
                {
                    StepName = nameof(MemoryManager.ReplyMentionedTweetGatherContextFromUserProfile),
                    MethodName = nameof(MemoryManager.ReplyMentionedTweetGatherContextFromUserProfile),
                    MaxRetries = 0,
                    Dependencies = new[] { nameof(MemoryManager.ReplyMentionedTweetDetermineProcessWorkflow) }
                },
                new FlowStepDefinition
                {
                    StepName = nameof(MemoryManager.ReplyMentionedTweetGatherContextFromCurrencyAPI),
                    MethodName = nameof(MemoryManager.ReplyMentionedTweetGatherContextFromCurrencyAPI),
                    MaxRetries = 1,
                    Dependencies = new[] { nameof(MemoryManager.ReplyMentionedTweetDetermineProcessWorkflow) }
                },
                new FlowStepDefinition
                {
                    StepName = nameof(MemoryManager.ReplyMentionedTweetGeneratePromptResponseToPost),
                    MethodName = nameof(MemoryManager.ReplyMentionedTweetGeneratePromptResponseToPost),
                    MaxRetries = 2,
                    Dependencies = new[]
                    {
                        nameof(MemoryManager.ReplyMentionedTweetGatherContextStoredInDb),
                        nameof(MemoryManager.ReplyMentionedTweetGatherContextFromUserProfile),
                        nameof(MemoryManager.ReplyMentionedTweetGatherContextFromCurrencyAPI)
                    }
                },
                new FlowStepDefinition
                {
                    StepName = nameof(MemoryManager.ReplyMentionedPostTwitterReply),
                    MethodName = nameof(MemoryManager.ReplyMentionedPostTwitterReply),
                    MaxRetries = 1,
                    Dependencies = new[] { nameof(MemoryManager.ReplyMentionedTweetGeneratePromptResponseToPost) }
                }
            }
        };

        public static MemoryProcessFlow GetFlow(string memoryTypeName)
        {
            return memoryTypeName switch
            {
                "CharacterMentionedTweet" => CharacterMentionedTweet,
                _ => null
            };
        }

        public class MemoryProcessFlow
        {
            public FlowStepDefinition[] Steps { get; set; }
        }

        public class FlowStepDefinition
        {
            public string StepName { get; set; }
            public string MethodName { get; set; }
            public string ParametersJson { get; set; }
            public string ParameterModelName { get; set; }
            public int MaxRetries { get; set; }
            public string[] Dependencies { get; set; }
        }
    }
}
