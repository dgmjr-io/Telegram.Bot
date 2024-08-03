﻿namespace Telegram.Bot.Requests;

/// <summary>Use this method to move a sticker in a set created by the bot to a specific position.<para>Returns: </para></summary>
public partial class SetStickerPositionInSetRequest : RequestBase<bool>
{
    /// <summary>File identifier of the sticker</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public required InputFileId Sticker { get; set; }

    /// <summary>New sticker position in the set, zero-based</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public required int Position { get; set; }

    /// <summary>Instantiates a new <see cref="SetStickerPositionInSetRequest"/></summary>
    public SetStickerPositionInSetRequest() : base("setStickerPositionInSet") { }
}
