using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Analytics.GetTaskStatuses;

public record GetTaskStatusesAnalyticQuery : ITenantQuery<StatusAnalyticResponse>;