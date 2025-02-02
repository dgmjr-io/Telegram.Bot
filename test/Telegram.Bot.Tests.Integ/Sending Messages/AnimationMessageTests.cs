using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Sending_Messages;

[Collection(Constants.TestCollections.SendAnimationMessage)]
[TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
public class AnimationMessageTests
{
    ITelegramBotClient BotClient => _fixture.BotClient;

    readonly TestsFixture _fixture;

    public AnimationMessageTests(TestsFixture fixture)
    {
        _fixture = fixture;
    }

    [OrderedFact("Should send an animation with caption")]
    [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendAnimation)]
    public async Task Should_Send_Animation()
    {
        Message message;
        await using (Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Animation.Earth))
        {
            message = await BotClient.SendAnimationAsync(
                chatId: _fixture.SupergroupChat.Id,
                animation: new InputFile(stream),
                duration: 4,
                width: 400,
                height: 400,
                thumb: null,
                caption: "<b>Rotating</b> <i>Earth</i>",
                parseMode: ParseMode.Html
            );
        }

        // For backwards compatibility, message type is set to Document
        Assert.Equal(MessageType.Animation, message.Type);
        Assert.NotNull(message.Document);
        Assert.NotNull(message.Animation);

        Assert.Equal("Rotating Earth", message.Caption);
        Assert.NotNull(message.CaptionEntities);
        Assert.NotNull(message.CaptionEntityValues);
        Assert.Equal(2, message.CaptionEntities.Length);
        Assert.Equal(2, message.CaptionEntityValues.Count());

        Assert.Equal(4, message.Animation.Duration);

        // Apparently Telegram converts gif to an mp4 with a lower resolution
        Assert.Equal(320, message.Animation.Width);
        Assert.Equal(320, message.Animation.Height);

        Assert.Equal("video/mp4", message.Animation.MimeType);
        Assert.NotEmpty(message.Animation.FileId);
        Assert.NotEmpty(message.Animation.FileUniqueId);
        Assert.NotNull(message.Animation.FileName);
        Assert.NotEmpty(message.Animation.FileName);
        Assert.True(message.Animation.FileSize > 80_000);
    }

    [OrderedFact("Should send an animation with thumbnail")]
    [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendAnimation)]
    public async Task Should_Send_Animation_With_Thumb()
    {
        Message message;
        await using (
            Stream stream1 = System.IO.File.OpenRead(Constants.PathToFile.Animation.Earth),
                stream2 = System.IO.File.OpenRead(Constants.PathToFile.Thumbnail.TheAbilityToBreak)
        )
        {
            message = await BotClient.SendAnimationAsync(
                chatId: _fixture.SupergroupChat,
                animation: new InputFile(stream1, "earth.gif"),
                thumb: new InputFile(stream2, "thumb.jpg")
            );
        }

        Assert.NotNull(message.Animation);
        Assert.NotNull(message.Animation.Thumb);
        Assert.NotEmpty(message.Animation.Thumb.FileId);
        Assert.NotEmpty(message.Animation.Thumb.FileUniqueId);
        Assert.Equal(320, message.Animation.Thumb.Height);
        Assert.Equal(320, message.Animation.Thumb.Width);
        Assert.True(message.Animation.Thumb.FileSize > 10_000);
    }
}
