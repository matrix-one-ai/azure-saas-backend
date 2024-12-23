using MimeKit;

namespace Icon.BaseManagement
{
    public static class BaseListColumnFactory
    {

        // GetCharacterName
        public static BaseListColumnDto GetCharacterNameColumn(
            string valuePath = "name",
            string sortPath = "name",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "CharacterNameColumn",
                Header = "Character",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }

        // GetPersonaName
        public static BaseListColumnDto GetPersonaNameColumn(
            string valuePath = "persona.name",
            string sortPath = "Persona",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "PersonaNameColumn",
                Header = "Persona",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }

        //GetPersonaAttitude
        public static BaseListColumnDto GetPersonaAttitudeColumn(
            string valuePath = "attitude",
            string sortPath = "Attitude",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "PersonaAttitudeColumn",
                Header = "Attitude",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null,
                ShouldTruncate = true
            };
        }

        // GetPersonaShouldRespondNewPosts
        public static BaseListColumnDto GetPersonaShouldRespondNewPostsColumn(
            string valuePath = "shouldRespondNewPosts",
            string sortPath = "ShouldRespondNewPosts",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "PersonaRespondNewPostsColumn",
                Header = "RespondToPosts",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }

        // GetPersonaShouldRespondMentions
        public static BaseListColumnDto GetPersonaShouldRespondMentionsColumn(
            string valuePath = "shouldRespondMentions",
            string sortPath = "ShouldRespondMentions",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "PersonaRespondMentionsColumn",
                Header = "RespondMentions",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }

        //GetPersonaShouldImportNewPosts
        public static BaseListColumnDto GetPersonaShouldImportNewPostsColumn(
            string valuePath = "shouldImportNewPosts",
            string sortPath = "ShouldImportNewPosts",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "PersonaImportNewPostsColumn",
                Header = "ImportNewPosts",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }


        public static BaseListColumnDto GetMemoryUrlColumn(
            string valuePath = "memoryUrl",
            string sortPath = "MemoryUrl")
        {
            return new BaseListColumnDto
            {
                Name = "MemoryUrlColumn",
                Header = "Url",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = true,
                Template = BaseListTemplateType.Url,
                Navigation = null
            };
        }


        public static BaseListColumnDto GetPlatformColumn(
            string valuePath = "platform.name",
            string sortPath = "Platform",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "PlatformColumn",
                Header = "Platform",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }

        public static BaseListColumnDto GetPlatformsColumn(
            string valuePath = "platformNames",
            string sortPath = "Platform",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "PlatformColumn",
                Header = "Platforms",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }


        public static BaseListColumnDto GetMemoryTypeColumn(
            string valuePath = "memoryType.name",
            string sortPath = "MemoryType",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "MemoryTypeColumn",
                Header = "MemoryType",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }

        public static BaseListColumnDto GetMemoryContentColumn(
            string valuePath = "memoryContent",
            string sortPath = "MemoryContent",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "MemoryContentColumn",
                Header = "MemoryContent",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 250,
                CanSort = false,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null,
                ShouldTruncate = true
            };
        }

        // GetCharacterColumn
        public static BaseListColumnDto GetCharacterColumn(
            string valuePath = "character.name",
            string sortPath = "Character",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "CharacterColumn",
                Header = "Character",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }

        public static BaseListColumnDto GetPersonaColumn(
            string valuePath = "characterPersona.persona.name",
            string sortPath = "CharacterPersona.Persona.Name",
            bool canNavigate = false)
        {
            return new BaseListColumnDto
            {
                Name = "PersonaColumn",
                Header = "Persona",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = false,
                UseTemplate = canNavigate,
                Template = canNavigate ? BaseListTemplateType.Navigation : BaseListTemplateType.None,
                Navigation = null
            };
        }



        public static BaseListColumnDto GetDateColumn(
            string header = "Date",
            string valuePath = "",
            string valueField = "",
            string sortPath = "",
            string sortField = "")
        {
            return new BaseListColumnDto
            {
                Name = header + "Column",
                Header = header,
                SortPath = sortPath + sortField,
                ValuePath = valuePath + valueField,
                Width = 120,
                CanSort = true,
                ShouldPipe = true,
                PipeType = BaseListPipeType.Date,
                PipeFormat = "dd-MM-yyyy"
            };
        }

        public static BaseListColumnDto GetTimeColumn(
            string header,
            string valuePath)
        {
            return new BaseListColumnDto
            {
                Name = "TimeColumn",
                Header = header,
                ValuePath = valuePath,
                Width = 100,
                CanSort = false,
                ShouldTranslate = false,
                UseTemplate = false,
                ShouldPipe = true,
                PipeType = BaseListPipeType.Date,
                PipeFormat = "HH:mm"
            };
        }

        public static BaseListColumnDto GetScheduledTimeColumn(
            string valuePath = "tripSegment.scheduledTime",
            string sortPath = "ScheduledTime")
        {
            return new BaseListColumnDto
            {
                Name = "ScheduledTimeColumn",
                Header = "ScheduledTime",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                ShouldTranslate = false,
                UseTemplate = false,
                ShouldPipe = true,
                PipeType = BaseListPipeType.Date,
                PipeFormat = "HH:mm"
            };
        }

        public static BaseListColumnDto GetEtaColumn(
            string valuePath = "tripSegment.combinedTime",
            string sortPath = "CombinedTime")
        {
            return new BaseListColumnDto
            {
                Name = "EtaColumn",
                Header = "ETA",
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                ShouldPipe = true,
                PipeType = BaseListPipeType.Date,
                PipeFormat = "HH:mm"
            };
        }

        public static BaseListColumnDto GetTwitterRankColumn(
            string header = "TwitterRank",
            string valuePath = "twitterRank",
            string sortPath = "TwitterRank.Rank")
        {
            return new BaseListColumnDto
            {
                Name = header + "Column",
                Header = header,
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
                ShouldTranslate = false,
                UseTemplate = true,
                Template = BaseListTemplateType.Ranking
            };
        }

        // isPromptGenerated
        public static BaseListColumnDto GetPromptStatusColumn(
            string header = "Prompted",
            string valuePath = "isPromptGenerated",
            string sortPath = "IsPromptGenerated")
        {
            return new BaseListColumnDto
            {
                Name = header + "Column",
                Header = header,
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 150,
                CanSort = true,
                ShouldTranslate = true,
                UseTemplate = true,
                Template = BaseListTemplateType.Status
            };
        }

        // isActionTaken
        public static BaseListColumnDto GetActionStatusColumn(
            string header = "ActionTaken",
            string valuePath = "isActionTaken",
            string sortPath = "IsActionTaken")
        {
            return new BaseListColumnDto
            {
                Name = header + "Column",
                Header = header,
                SortPath = sortPath,
                ValuePath = valuePath,
                Width = 150,
                CanSort = true,
                ShouldTranslate = true,
                UseTemplate = true,
                Template = BaseListTemplateType.Status
            };
        }

        public static BaseListColumnDto GetRowActionsColumn(string entityName)
        {
            return new BaseListColumnDto
            {
                Name = "RowActionsColumn",
                Header = entityName + "Actions",
                Width = 100,
                CanSort = false,
                UseTemplate = true,
                Template = BaseListTemplateType.RowActions
            };
        }

        public static BaseListColumnDto GetExternalTripIdColumn(string valuePath = "trip.externalId")
        {
            return new BaseListColumnDto
            {
                Name = "ExternalTripIdColumn",
                Header = "ExternalTripId",
                SortPath = "ExternalTripId",
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
            };
        }

        public static BaseListColumnDto GetExternalRouteIdColumn(string valuePath = "route.externalId")
        {
            return new BaseListColumnDto
            {
                Name = "ExternalRouteIdColumn",
                Header = "ExternalRouteId",
                SortPath = "ExternalRouteId",
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
            };
        }

        public static BaseListColumnDto GetExternalClientIdColumn(string valuePath = "client.externalId")
        {
            return new BaseListColumnDto
            {
                Name = "ExternalClientIdColumn",
                Header = "ExternalClientId",
                SortPath = "ExternalClientId",
                ValuePath = valuePath,
                Width = 100,
                CanSort = true,
            };
        }

        public static BaseListColumnDto GetExternalBlockadeIdColumn(string valuePath = "externalBlockadeIds")
        {
            return new BaseListColumnDto
            {
                Name = "ExternalBlockadeIdColumn",
                Header = "ExternalBlockadeIds",
                ValuePath = valuePath,
                Width = 100,
                CanSort = false,
            };
        }


    }
}