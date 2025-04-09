namespace Nerbotix.Domain.Robots.Communications;

public class HttpCommunication : RobotCommunication
{
    public string ApiEndpoint { get; set; } = null!;
    public string HttpMethod { get; set; } = System.Net.Http.HttpMethod.Post.ToString(); 
}