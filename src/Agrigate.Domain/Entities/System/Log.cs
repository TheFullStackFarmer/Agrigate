using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace Agrigate.Domain.Entities.System;

/// <summary>
/// A table for logging events and information
/// </summary>
[Table(nameof(Log))]
public class Log : EntityBase
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The time at which the log was recorded
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// The severity of the log
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// The message of the log
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Where the log originated
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Serialized JSON of any data associated with the log
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// The stacktrace if logging an exception
    /// </summary>
    public string? StackTrace { get; set; }
}