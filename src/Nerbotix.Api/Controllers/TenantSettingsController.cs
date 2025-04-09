using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Application.Algorithms.Settings.AntColony;
using Nerbotix.Application.Algorithms.Settings.GeneralSettings;
using Nerbotix.Application.Algorithms.Settings.Genetic;
using Nerbotix.Application.Algorithms.Settings.GetAlgorithmsSettings;
using Nerbotix.Application.Algorithms.Settings.LinearOptimization;
using Nerbotix.Application.Algorithms.Settings.LoadBalancing;
using Nerbotix.Application.Algorithms.Settings.SimulatedAnnealing;

namespace Nerbotix.Api.Controllers;

[Route("tenant-settings")]
public class TenantSettingsController(IMediator mediator) : BaseController
{
    [HttpGet("algorithms")]
    public async Task<IActionResult> GetAlgorithmsSettings()
    {
        var query = new GetAlgorithmsSettingsQuery();
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPut("algorithms/general")]
    public async Task<IActionResult> UpdateGeneralSettings(UpdateGeneralAlgorithmSettingsCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
    
    [HttpPut("algorithms/linear-optimization")]
    public async Task<IActionResult> UpdateLinearOptimizationSettings(UpdateLinearOptimizationSettingsCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpPut("algorithms/ant-colony")]
    public async Task<IActionResult> UpdateAntColonySettings(UpdateAntColonySettingsCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpPut("algorithms/genetic")]
    public async Task<IActionResult> UpdateGeneticSettings(UpdateGeneticSettingsCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpPut("algorithms/load-balancing")]
    public async Task<IActionResult> UpdateLoadBalancingSettings(UpdateLoadBalancingSettingsCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpPut("algorithms/simulated-annealing")]
    public async Task<IActionResult> UpdateSimulatedAnnealingSettings(UpdateSimulatedAnnealingSettingsCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}