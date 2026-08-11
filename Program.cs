var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TodoStore>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    name = "Small Todo API",
    status = "running"
}));

app.MapGet("/api/todos", (TodoStore store) =>
    Results.Ok(store.GetAll()));

app.MapGet("/api/todos/{id:int}", (int id, TodoStore store) =>
{
    var todo = store.Get(id);
    return todo is null ? Results.NotFound() : Results.Ok(todo);
});

app.MapPost("/api/todos", (CreateTodoRequest request, TodoStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest(new { message = "Title is required." });

    var todo = store.Create(request.Title);
    return Results.Created($"/api/todos/{todo.Id}", todo);
});

app.MapPut("/api/todos/{id:int}", (int id, UpdateTodoRequest request, TodoStore store) =>
{
    var todo = store.Update(id, request.Title, request.IsCompleted);
    return todo is null ? Results.NotFound() : Results.Ok(todo);
});

app.MapDelete("/api/todos/{id:int}", (int id, TodoStore store) =>
    store.Delete(id) ? Results.NoContent() : Results.NotFound());

app.Run();

public record Todo(int Id, string Title, bool IsCompleted);

public record CreateTodoRequest(string Title);

public record UpdateTodoRequest(string Title, bool IsCompleted);

public class TodoStore
{
    private readonly List<Todo> _todos =
    [
        new Todo(1, "Learn ASP.NET Core", true),
        new Todo(2, "Build a REST API", false)
    ];

    private int _nextId = 3;

    public IEnumerable<Todo> GetAll() => _todos;

    public Todo? Get(int id) =>
        _todos.FirstOrDefault(x => x.Id == id);

    public Todo Create(string title)
    {
        var todo = new Todo(_nextId++, title, false);
        _todos.Add(todo);
        return todo;
    }

    public Todo? Update(int id, string title, bool isCompleted)
    {
        var index = _todos.FindIndex(x => x.Id == id);

        if (index == -1)
            return null;

        var todo = new Todo(id, title, isCompleted);
        _todos[index] = todo;
        return todo;
    }

    public bool Delete(int id)
    {
        var todo = Get(id);

        if (todo is null)
            return false;

        _todos.Remove(todo);
        return true;
    }
}
