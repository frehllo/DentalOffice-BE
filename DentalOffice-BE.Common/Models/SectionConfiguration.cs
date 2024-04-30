using System.Text.Json.Serialization;

namespace DentalOffice_BE.Common;

public class SectionConfiguration
{
    [JsonPropertyName("iconName")]
    public string? IconName { get; set; }
    [JsonPropertyName("tableHeaderFields")]
    public ICollection<TableHeaderField>? TableHeaderFields { get; set; } = null;
    [JsonPropertyName("formConfiguration")]
    public ICollection<FormGroupConfiguration>? FormConfiguration { get; set; } = null;
}

public class TableHeaderField
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = null!;
    [JsonPropertyName("headerName")]
    public string? HeaderName { get; set; } = null!;
    [JsonPropertyName("cellRenderer")]
    public string? CellRenderer { get; set; } = null!;
}

public class FormGroupConfiguration
{
    [JsonPropertyName("fieldGroupClassName")]
    public string? FieldGroupClassName { get; set; }
    [JsonPropertyName("fieldGroup")]
    public ICollection<FormFieldConfiguration>? FieldGroup { get; set; } = null!;
}

public class FormFieldConfiguration 
{

    [JsonPropertyName("key")]
    public string Key { get; set; } = null!;
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("className")]
    public string? ClassName { get; set; }
    [JsonPropertyName("props")]
    public FormFieldProps? Props { get; set; } = null!;
    [JsonPropertyName("fieldGroup")]
    public ICollection<FormFieldConfiguration>? FieldGroup { get; set; } = null!;
}

public class FormFieldProps
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }
    [JsonPropertyName("required")]
    public bool? Required { get; set; }
    [JsonPropertyName("options")]
    public ICollection<FormFieldPropsOption>? Options { get; set; }
}

public class FormFieldPropsOption
{
    [JsonPropertyName("value")]
    public long? Value { get; set; }
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}