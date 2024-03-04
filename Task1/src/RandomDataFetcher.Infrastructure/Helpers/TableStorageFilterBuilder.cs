using System.Text;
namespace RandomDataFetcher.Infrastructure.Helpers;

internal class TableStorageFilterBuilder
{
    private const string AndExpression = " and ";
    private const string OrExpression = " or ";
    private const string EqualsExpression = " eq ";
    private const string PartitionKey = "PartitionKey";
    private const string RowKey = "RowKey";
    private readonly StringBuilder _filter = new();

    public TableStorageFilterBuilder And()
    {
        return Append(AndExpression);
    }

    public string Build()
    {
        return _filter.ToString();
    }

    public TableStorageFilterBuilder Equals()
    {
        return Append(EqualsExpression);
    }

    public TableStorageFilterBuilder Or()
    {
        return Append(OrExpression);
    }

    public TableStorageFilterBuilder PartitionKeyEquals(string value)
    {
        return EqualsExpressionFor(PartitionKey, value);
    }

    public TableStorageFilterBuilder PropertyIsNull(string property)
    {
        return Append($"not({property} ne '')");
    }

    public TableStorageFilterBuilder PropertyKeyEquals(string property, string value)
    {
        return EqualsExpressionFor(property, value);
    }

    public TableStorageFilterBuilder RowKeyEquals(string value)
    {
        return EqualsExpressionFor(RowKey, value);
    }

    private TableStorageFilterBuilder EqualsExpressionFor(string property, string value)
    {
        return Append($"{property}{EqualsExpression}'{value}'");
    }

    private TableStorageFilterBuilder Append(string value)
    {
        _filter.Append(value);
        return this;
    }
}
