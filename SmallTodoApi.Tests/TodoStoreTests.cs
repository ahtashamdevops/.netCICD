using Xunit;

public class TodoStoreTests
{
    [Fact]
    public void Create_AddsTodo()
    {
        var store = new TodoStore();

        var todo = store.Create("Learn CI/CD");

        Assert.Equal(3, todo.Id);
        Assert.Equal("Learn CI/CD", todo.Title);
        Assert.False(todo.IsCompleted);
    }

    [Fact]
    public void Update_ChangesTodo()
    {
        var store = new TodoStore();

        var todo = store.Update(1, "Updated", true);

        Assert.NotNull(todo);
        Assert.Equal("Updated", todo!.Title);
        Assert.True(todo.IsCompleted);
    }

    [Fact]
    public void Delete_RemovesTodo()
    {
        var store = new TodoStore();

        var deleted = store.Delete(1);

        Assert.True(deleted);
        Assert.Null(store.Get(1));
    }
}
