using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Members.Create;
using RChat.Application.Members.Delete;

namespace RChat.Web.Controllers.Members;

[ApiController]
[Route("api/[controller]/")]
public class MembersController(ISender sender): ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CreateMemberResponse>> Create(
        [FromBody] CreateMemberRequest request,
        CancellationToken cancellationToken)
    {
        int memberId = await sender.Send(
            new CreateMemberCommand
            {
                ChatId = request.ChatId,
            }, cancellationToken);

        return Ok(new CreateMemberResponse
        {
            MemberId = memberId
        });
    }

    [Authorize]
    [HttpDelete("{memberId:int}")]
    public async Task<ActionResult> Delete(
        [FromRoute] int memberId,
        CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteMemberCommand
        {
            MemberId = memberId
        }, cancellationToken);

        return Ok();
    }
}