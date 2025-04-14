using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.GlobalSearch;

namespace RChat.Web.Controllers.Search;

[ApiController]
[Route("api/[controller]")]
public class GlobalSearchController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GlobalSearchDtoResponse>> Search(
        [FromQuery]string query,
        [FromQuery]int take,
        CancellationToken cancellationToken)
    {
        GlobalSearchDtoResponse searchResponse = 
            await sender.Send(new GlobalSearchQuery(query, take), cancellationToken);
        
        return Ok(searchResponse);
    }
}