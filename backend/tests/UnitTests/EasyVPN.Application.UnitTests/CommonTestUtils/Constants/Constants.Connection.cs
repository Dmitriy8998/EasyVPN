namespace EasyVPN.Application.UnitTests.CommonTestUtils.Constants;

public static partial class Constants
{
    public static class Connection
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly int Days = 30;
        public static readonly string Config = "configuration";

        public static IEnumerable<Guid> GetMore(int count = 10)
        {
            for (int i = 0; i < count; i++)
                yield return GuidGenerator.GuidByIndex(i, Id);
        }
    }
}