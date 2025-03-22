namespace RoboTasker.Domain.Tasks;

public enum RobotTaskStatus
{
    Pending,
    WaitingForEnqueue,
    InProgress,
    Completed,
    Failed,
    Canceled
}