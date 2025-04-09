namespace Nerbotix.Domain.Tasks;

public enum RobotTaskStatus
{
    Pending,
    WaitingForEnqueue,
    InProgress,
    Completed,
    Failed,
    Canceled
}