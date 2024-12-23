using System;
using System.Linq;
using Icon.Matrix.AIManager.CharacterMentioned;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Icon.Matrix.Memories.Forms
{
    public class MemoryFormModel
    {
        public Guid MemoryId { get; set; }
        public Character Character { get; set; }
        public Persona Persona { get; set; }
        public MemoryType MemoryType { get; set; }
        public Platform Platform { get; set; }
        public Prompt Prompt { get; set; }

        public string MemoryContent { get; set; }
        public string MemoryUrl { get; set; }



        public MemoryFormModel()
        {
        }

        public MemoryFormModel(Models.Memory memory)
        {
            var persona = memory.CharacterPersona?.Persona;

            MemoryId = memory.Id;
            Character = new Character
            {
                Id = memory.CharacterId,
                Name = memory.Character?.Name
            };
            Persona = new Persona
            {
                Id = persona?.Id ?? Guid.Empty,
                Name = persona?.Name
            };
            MemoryType = new MemoryType
            {
                Id = memory.MemoryTypeId,
                Name = memory.MemoryType?.Name
            };
            Platform = new Platform
            {
                Id = memory.PlatformId ?? Guid.Empty,
                Name = memory.Platform?.Name
            };
            MemoryContent = memory.MemoryContent;
            MemoryUrl = memory.MemoryUrl;

            var promptJson = memory.Prompts?.OrderByDescending(p => p.GeneratedAt).FirstOrDefault()?.ResponseJson;
            if (promptJson != null)
            {
                try
                {
                    var promptResponse = JsonConvert.DeserializeObject<AICharacterMentionedResponse>(promptJson);
                    Prompt = new Prompt
                    {
                        PromptOutput = promptResponse?.ResultToPost
                    };
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                    // log error
                }
            }
        }
    }

    public class Character
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }

    public class Persona
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class PersonaPlatform
    {
        public Guid PlatformId { get; set; }
        public string PlatformName { get; set; }
        public string PlatformPersonaId { get; set; }
    }

    public class MemoryType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class Platform
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class Prompt
    {
        public string PromptInput { get; set; }
        public string PromptOutput { get; set; }
    }


}