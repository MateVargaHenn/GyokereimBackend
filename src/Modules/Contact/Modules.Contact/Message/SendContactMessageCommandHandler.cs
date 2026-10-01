
using Modules.File.Contract;

namespace Modules.Contact.Message;

internal sealed class SendContactMessageCommandHandler(
    ContactDbContext dbContext,
    IFileModule fileModule,
    ICaptchaValidator captchaValidator)
    : ICommandHandler<SendContactMessageCommand>
{
    public async Task<Result> Handle(
        SendContactMessageCommand request,
        CancellationToken cancellationToken)
    {


        bool captchaValid = await captchaValidator.ValidateAsync(
            request.CaptchaToken,
            cancellationToken);

        if (!captchaValid)
        {
            return Result.Failure(
                ErrorType.Validation);
        }


        int? attachmentId = null;

        if (request.Attachment is not null)
        {
            attachmentId = await fileModule.SaveFileAsync(
                request.Attachment,
                cancellationToken);
        }

        var contactMessage = ContactMessage.Create(
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.Message,
            attachmentId);

        dbContext.ContactMessages.Add(contactMessage);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
