using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Auth.CreateCompany;

public class CreateCompanyCommand : ICommand
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
}

