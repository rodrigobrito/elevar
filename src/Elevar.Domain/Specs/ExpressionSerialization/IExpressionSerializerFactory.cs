namespace Elevar.Domain.Specs.ExpressionSerialization
{
    /// <summary>Creates instances of expression serializers.</summary>
    public interface IExpressionSerializerFactory
    {
        /// <summary>Creates instances of expression serializers.</summary>
        IExpressionSerializer CreateSerializer();
    }
}
