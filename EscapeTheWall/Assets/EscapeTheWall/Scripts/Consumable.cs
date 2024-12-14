/// <summary>
/// Defines a contract for objects that can be consumed.
/// Classes implementing this interface should define specific consume behavior.
/// </summary>
public interface Consumable
{
    /// <summary>
    /// Performs the consume action on the object.
    /// Implementations should define what happens when the object is consumed.
    /// </summary>
    void Consume();
}

