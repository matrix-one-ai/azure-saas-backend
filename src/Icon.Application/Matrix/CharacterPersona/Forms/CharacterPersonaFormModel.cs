using System;
using System.Collections.Generic;
using System.Linq;
using Icon.BaseManagement;
using Icon.Matrix.Models;

namespace Icon.Matrix.CharacterPersonas.Forms
{
    public class CharacterPersonaFormModel
    {
        public Guid CharacterPersonaId { get; set; }
        public Character Character { get; set; }
        public Persona Persona { get; set; }

        public string Attitude { get; set; }
        public string Responses { get; set; }
        public bool ShouldRespondNewPosts { get; set; }
        public bool ShouldRespondMentions { get; set; }
        public bool ShouldImportNewPosts { get; set; }
        public bool PersonaIsAi { get; set; }
        public BaseModalType ModalType { get; set; }

        public PersonaPlatform Twitter { get; set; }
        public PersonaPlatform Discord { get; set; }
        public PersonaPlatform Facebook { get; set; }
        public PersonaPlatform Instagram { get; set; }
        public PersonaPlatform Telegram { get; set; }

        public CharacterPersonaFormModel()
        {
        }

        public CharacterPersonaFormModel(CharacterPersona characterPersona)
        {
            CharacterPersonaId = characterPersona.Id;
            Character = new Character
            {
                Id = characterPersona.CharacterId,
                Name = characterPersona.Character?.Name
            };
            Persona = new Persona
            {
                Id = characterPersona.PersonaId,
                Name = characterPersona.Persona?.Name
            };

            Attitude = characterPersona.Attitude;
            Responses = characterPersona.Repsonses;

            ShouldRespondNewPosts = characterPersona.ShouldRespondNewPosts;
            ShouldRespondMentions = characterPersona.ShouldRespondMentions;
            ShouldImportNewPosts = characterPersona.ShouldImportNewPosts;

            PersonaIsAi = characterPersona.PersonaIsAi;

            var personaPlatforms = characterPersona.Persona.Platforms.Select(p => new PersonaPlatform
            {
                PlatformId = p.PlatformId,
                PlatformName = p.Platform.Name,
                PlatformPersonaId = p.PlatformPersonaId
            }).ToList();
            SetPlatforms(personaPlatforms);

        }


        private void SetPlatforms(List<PersonaPlatform> platforms)
        {
            if (platforms == null || platforms.Count == 0)
            {
                return;
            }

            Twitter = platforms.FirstOrDefault(p => p.PlatformName == "Twitter");
            Facebook = platforms.FirstOrDefault(p => p.PlatformName == "Facebook");
            Instagram = platforms.FirstOrDefault(p => p.PlatformName == "Instagram");
            Discord = platforms.FirstOrDefault(p => p.PlatformName == "Discord");
            Telegram = platforms.FirstOrDefault(p => p.PlatformName == "Telegram");
        }

        public void SetMissingPlatforms(List<Platform> platforms)
        {
            if (Twitter == null)
            {
                var twitter = platforms.FirstOrDefault(p => p.Name == "Twitter");
                Twitter = new PersonaPlatform
                {
                    PlatformId = twitter.Id,
                    PlatformName = twitter.Name
                };
            }
            if (Facebook == null)
            {
                var facebook = platforms.FirstOrDefault(p => p.Name == "Facebook");
                Facebook = new PersonaPlatform
                {
                    PlatformId = facebook.Id,
                    PlatformName = facebook.Name
                };
            }
            if (Discord == null)
            {
                var discord = platforms.FirstOrDefault(p => p.Name == "Discord");
                Discord = new PersonaPlatform
                {
                    PlatformId = discord.Id,
                    PlatformName = discord.Name
                };
            }
            if (Instagram == null)
            {
                var instagram = platforms.FirstOrDefault(p => p.Name == "Instagram");
                Instagram = new PersonaPlatform
                {
                    PlatformId = instagram.Id,
                    PlatformName = instagram.Name
                };
            }
            if (Telegram == null)
            {
                var telegram = platforms.FirstOrDefault(p => p.Name == "Telegram");
                Telegram = new PersonaPlatform
                {
                    PlatformId = telegram.Id,
                    PlatformName = telegram.Name
                };
            }
        }

        public CharacterPersonaFormModel(BaseModalType modalType, List<Platform> platforms = null)
        {
            ModalType = modalType;
            if (modalType == BaseModalType.ModalNew)
            {
                SetModelNew(platforms);
            }
        }

        private void SetModelNew(List<Platform> platforms = null)
        {
            Character = new Character();
            Persona = new Persona { Id = Guid.NewGuid() };

            var personaPlatforms = platforms?.Select(p => new PersonaPlatform
            {
                PlatformId = p.Id,
                PlatformName = p.Name,
            }).ToList();

            SetPlatforms(personaPlatforms);
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

}