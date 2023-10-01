using Scriban;

namespace EnumSerializer.Generator;

internal static class SourceGenerationHelper
{
    internal const string ConverterTemplate = """
        //------------------------------------------------------------------------------
        // <auto-generated>
        //     This code was generated by the EnumSerializer.Generator source generator
        //
        //     Changes to this file may cause incorrect behavior and will be lost if
        //     the code is regenerated.
        // </auto-generated>
        //------------------------------------------------------------------------------

        #nullable enable

        using System;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Linq;
        using System.Runtime.CompilerServices;

        {{~ if enum_namespace ~}}
        namespace {{ enum_namespace }};
        {{~ end ~}}

        internal partial class {{ enum_name }}Converter : JsonConverter<{{ enum_name }}>
        {
            public override void WriteJson(JsonWriter writer, {{ enum_name }} value, JsonSerializer serializer) =>
                writer.WriteValue(value switch
                {
                {{~ for enum_member in enum_members ~}}
                    {{ enum_name }}.{{enum_member.key}} => "{{ enum_member.value }}",
                {{~ end ~}}
                {{~ if has_unknown_member ~}}
                    _ => throw new NotSupportedException(),
                {{~ else ~}}
                    ({{ enum_name }})0 => "unknown",
                    _ => throw new NotSupportedException(),
                {{~ end ~}}
                });

            public override {{ enum_name }} ReadJson(
                JsonReader reader,
                Type objectType,
            {{ enum_name }} existingValue,
                bool hasExistingValue,
                JsonSerializer serializer
            ) =>
                JToken.ReadFrom(reader).Value<string>() switch
                {
                {{~ for enum_member in enum_members ~}}
                    "{{ enum_member.value }}" => {{ enum_name }}.{{ enum_member.key }},
                {{~ end ~}}
                {{~ if has_unknown_member ~}}
                    _ => {{ enum_name }}.Unknown,
                {{~ else ~}}
                    _ => 0,
                {{~ end ~}}
                };
        }
        """;

    internal static string GenerateConverterClass(Template template, EnumInfo enumToGenerate)
    {
        var hasUnknownMember = enumToGenerate.Members.Any(
            e => string.Equals(e.Value, "Unknown", StringComparison.OrdinalIgnoreCase)
        );

        var result = template.Render(
            new
            {
                EnumNamespace = enumToGenerate.Namespace,
                EnumName = enumToGenerate.Name,
                EnumMembers = enumToGenerate.Members,
                HasUnknownMember = hasUnknownMember,
            }
        );

        return result;
    }
}
