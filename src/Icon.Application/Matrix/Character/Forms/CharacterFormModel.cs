using System;
using System.Collections.Generic;
using System.Linq;
using Abp.UI;
using Icon.Authorization.Users;
using Icon.BaseManagement;
using Icon.Matrix.Enums;
using Icon.Matrix.Memories.Forms;

namespace Icon.Matrix.Characters.Forms
{
    public class CharacterFormModel
    {
        public Guid CharacterId { get; set; }
        public string Name { get; set; }
        public CharacterBio CurrentBio { get; set; }
        public string TwitterPostAgentId { get; set; }
        public string TwitterScrapeAgentId { get; set; }
        public bool IsTwitterScrapingEnabled { get; set; }
        public bool IsTwitterPostingEnabled { get; set; }
        public bool IsPromptingEnabled { get; set; }
        public string TwitterUserName { get; set; }
        public BaseModalType ModalType { get; set; }
        public int TwitterCommType { get; set; }
        public CharacterPromptInstruction PromptInstructions { get; set; }


        public CharacterFormModel()
        {
        }

        public CharacterFormModel(Models.Character character)
        {
            Name = character.Name;
            CharacterId = character.Id;

            TwitterPostAgentId = character.TwitterPostAgentId;
            TwitterScrapeAgentId = character.TwitterScrapeAgentId;
            IsTwitterScrapingEnabled = character.IsTwitterScrapingEnabled;
            IsTwitterPostingEnabled = character.IsTwitterPostingEnabled;
            IsPromptingEnabled = character.IsPromptingEnabled;
            TwitterUserName = character.TwitterUserName;
            TwitterCommType = (int)character.TwitterCommType;

            var latestBio = character.Bios.Where(b => b.IsActive).FirstOrDefault();

            if (latestBio == null)
            {
                throw new UserFriendlyException("No active bio found for character");
            }

            if (latestBio != null)
            {
                CurrentBio = new CharacterBio
                {
                    Bio = latestBio.Bio,
                    Personality = latestBio.Personality,
                    Appearance = latestBio.Appearance,
                    Occupation = latestBio.Occupation,
                    //BirthDate = latestBio.BirthDate,
                    // ActiveFrom = latestBio.ActiveFrom,
                    // ActiveTo = latestBio.ActiveTo,
                    IsActive = latestBio.IsActive,
                    Motivations = latestBio.Motivations,
                    Fears = latestBio.Fears,
                    Values = latestBio.Values,
                    SpeechPatterns = latestBio.SpeechPatterns,
                    Skills = latestBio.Skills,
                    Backstory = latestBio.Backstory,
                    PublicPersona = latestBio.PublicPersona,
                    PrivateSelf = latestBio.PrivateSelf,
                    MediaPresence = latestBio.MediaPresence,
                    CrisisBehavior = latestBio.CrisisBehavior,
                    Relationships = latestBio.Relationships,
                    TechDetails = latestBio.TechDetails
                };
            }

            PromptInstructions = new CharacterPromptInstruction
            {
                TwitterAutoPostInstruction = character.TwitterAutoPostInstruction,
                TwitterAutoPostExamples = character.TwitterAutoPostExamples,

                TwitterMentionReplyInstruction = character.TwitterMentionReplyInstruction,
                TwitterMentionReplyExamples = character.TwitterMentionReplyExamples
            };

        }
    }

    public class CharacterBio
    {
        public string Bio { get; set; }
        public string Personality { get; set; }
        public string Appearance { get; set; }
        public string Occupation { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public bool IsActive { get; set; }
        public string Motivations { get; set; }
        public string Fears { get; set; }
        public string Values { get; set; }
        public string SpeechPatterns { get; set; }
        public string Skills { get; set; }
        public string Backstory { get; set; }
        public string PublicPersona { get; set; }
        public string PrivateSelf { get; set; }
        public string MediaPresence { get; set; }
        public string CrisisBehavior { get; set; }
        public string Relationships { get; set; }
        public string TechDetails { get; set; }
    }

    public class CharacterPromptInstruction
    {
        public string TwitterAutoPostInstruction { get; set; }
        public string TwitterAutoPostExamples { get; set; }
        public int TwitterAutoPostDelayMinutes { get; set; }

        public string TwitterMentionReplyInstruction { get; set; }
        public string TwitterMentionReplyExamples { get; set; }
    }

}