using System.Collections.Generic;

namespace Quantum.ServiceBus;

/// <summary>
///     Specification of a new topic to be created via the CreateTopics
///     method. This class is used for the same purpose as NewTopic in
///     the Java API.
/// </summary>
public class TopicSpecification
{
    /// <summary>
    ///     The configuration to use to create the new topic.
    /// </summary>
    public Dictionary<string, string> Configs { get; set; }

    /// <summary>
    ///     The name of the topic to be created (required).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     The number of partitions for the new topic or -1 (the default) if a 
    ///     replica assignment is specified.
    /// </summary>
    public int NumPartitions { get; set; } = -1;

    /// <summary>
    ///     A map from partition id to replica ids (i.e., static broker ids) or null
    ///     if the number of partitions and replication factor are specified
    ///     instead.
    /// </summary>
    public Dictionary<int, List<int>> ReplicasAssignments { get; set; } = null;

    /// <summary>
    ///     The replication factor for the new topic or -1 (the default) if a 
    ///     replica assignment is specified instead.
    /// </summary>
    public short ReplicationFactor { get; set; } = -1;
}