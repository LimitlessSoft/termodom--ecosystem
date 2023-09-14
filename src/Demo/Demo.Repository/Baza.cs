using Demo.Contracts.Entities;

namespace Demo.Repository
{
    public static class Baza
    {
        public static List<CarEntity> Cars { get; set; } = new List<CarEntity>()
        {
            new CarEntity()
            {
                Id = 1,
                Name = "Foo",
            },
            new CarEntity()
            {
                Id = 2,
                Name = "Foo2",
            },
            new CarEntity()
            {
                Id = 3,
                Name = "Foo3",
            },
        };
    }
}
