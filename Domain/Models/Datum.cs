using System;
using System.Text.Json.Serialization;

namespace BeebopNoteApp.Domain.Models;

public partial class Datum
{
    [JsonPropertyName("cta")]
    public Cta Cta { get; set; } = null!;

    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("_templateId")]
    public string TemplateId { get; set; } = string.Empty;

    [JsonPropertyName("_environmentId")]
    public string EnvironmentId { get; set; } = string.Empty;

    [JsonPropertyName("_messageTemplateId")]
    public string MessageTemplateId { get; set; } = string.Empty;

    [JsonPropertyName("_notificationId")]
    public string NotificationId { get; set; } = string.Empty;

    [JsonPropertyName("_organizationId")]
    public string OrganizationId { get; set; } = string.Empty;

    [JsonPropertyName("_subscriberId")]
    public string SubscriberId { get; set; } = string.Empty;

    [JsonPropertyName("_jobId")]
    public string JobId { get; set; } = string.Empty;

    [JsonPropertyName("templateIdentifier")]
    public string TemplateIdentifier { get; set; } = string.Empty;

    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    [JsonPropertyName("_feedId")]
    public object FeedId { get; set; } = string.Empty;

    [JsonPropertyName("channel")]
    public string Channel { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("providerId")]
    public string ProviderId { get; set; } = string.Empty;

    [JsonPropertyName("deviceTokens")]
    public object[] DeviceTokens { get; set; } = [];

    [JsonPropertyName("seen")]
    public bool Seen { get; set; }

    [JsonPropertyName("read")]
    public bool Read { get; set; }

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("transactionId")]
    public string TransactionId { get; set; } = string.Empty.ToString();

    [JsonPropertyName("data")]
    public Data Data { get; set; } = null!;

    [JsonPropertyName("payload")]
    public Payload Payload { get; set; } = null!;

    [JsonPropertyName("tags")]
    public object[] Tags { get; set; } = [];

    [JsonPropertyName("avatar")]
    public Uri Avatar { get; set; } = null!;

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("__v")]
    public long V { get; set; }

    [JsonPropertyName("lastReadDate")]
    public DateTimeOffset LastReadDate { get; set; }

    [JsonPropertyName("lastSeenDate")]
    public DateTimeOffset LastSeenDate { get; set; }

    [JsonPropertyName("subscriber")]
    public Subscriber Subscriber { get; set; } = null!;

    [JsonPropertyName("template")]
    public Template Template { get; set; } = null!;

    [JsonPropertyName("actorSubscriber")]
    public object ActorSubscriber { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string DatumId { get; set; } = string.Empty;
}