using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Icon.Matrix.Enums;

namespace Icon.Matrix.Portal.Dto
{

    public class MemoryThoughtDto
    {
        public string ModuleType = "twitter";
        public string ActionGroup = "handleMention";
        public string Action { get; set; }
        public string State { get; set; }
        public List<string> Thoughts { get; set; }
        public DateTimeOffset LoggedAt { get; set; }

        public MemoryThoughtDto(
            string action,
            MemoryProcessStepState state,
            List<MemoryProcessLogDto> logs)
        {
            Action = action;
            State = state.ToString();
            Thoughts = logs.Select(x => x.Message).ToList();
        }

    }

    public class MemoryProcessDto : EntityDto<Guid>
    {
        public int TenantId { get; set; }
        public Guid MemoryId { get; set; }
        public MemoryProcessState State { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }

        public ICollection<MemoryProcessStepDto> Steps { get; set; } = new List<MemoryProcessStepDto>();
        public ICollection<MemoryProcessLogDto> Logs { get; set; } = new List<MemoryProcessLogDto>();
    }


    public class MemoryProcessStepDto
    {
        public int OrderIndex { get; set; }
        public string StepName { get; set; }
        public string MethodName { get; set; }
        public string ParametersJson { get; set; }
        public MemoryProcessStepState State { get; set; }
        public string DependenciesJson { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; } = 3;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }

    public class MemoryProcessLogDto
    {
        public string Message { get; set; }
        public DateTimeOffset LoggedAt { get; set; }
        public string LogLevel { get; set; }
        public string Exception { get; set; }
        public string ExceptionMessage { get; set; }
    }

}