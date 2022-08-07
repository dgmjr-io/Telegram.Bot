using System.IO;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Unit.Serialization;

public class InputFileSerializationTests
{
    [Fact(DisplayName = "Should serialize & deserialize input file from stream")]
    public void Should_Serialize_InputFile()
    {
        const string fileName = "myFile";
        InputFile inputFile = new InputFile(new MemoryStream(), fileName);

        string json = JsonConvert.SerializeObject(inputFile);
        InputFile obj = JsonConvert.DeserializeObject<InputFile>(json)!;

        Assert.Equal(@$"""attach://{fileName}""", json);
        Assert.Equal(Stream.Null, obj.Content);
        Assert.Equal(fileName, obj.FileName);
        Assert.Equal(FileType.Stream, obj.FileType);
    }

    [Fact(DisplayName = "Should serialize & deserialize input file with file_id")]
    public void Should_Serialize_FileId()
    {
        const string fileId = "This-is-a-file_id";
        InputFileId inputFileId = new(fileId);

        string json = JsonConvert.SerializeObject(inputFileId);
        InputFileId obj = JsonConvert.DeserializeObject<InputFileId>(json);

        Assert.Equal(@$"""{fileId}""", json);
        Assert.Equal(fileId, obj.value);
        Assert.Equal(FileType.Id, obj.FileType);
    }

    [Fact(DisplayName = "Should serialize & deserialize input file with URL")]
    public void Should_Serialize_InputUrlFile()
    {
        const string url = "http://github.org/TelgramBots";
        InputFileUrl inputFileUrl = new(url);

        string json = JsonConvert.SerializeObject(inputFileUrl);
        InputFileUrl obj = JsonConvert.DeserializeObject<InputFileUrl>(json);

        Assert.Equal(@$"""{url}""", json);
        Assert.Equal(url, obj.value);
        Assert.Equal(FileType.Url, obj.FileType);
    }
}
