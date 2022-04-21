using Microsoft.OpenApi.Models;
using MinimalAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder =>
    {
        builder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000", "htpps://appname.azurestaticapps.net");
    });
}); 

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    swaggerGenOptions =>
    {
        swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo{Title = "ASP.NET 6 React App", Version = "v1"});
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions =>
{
    swaggerUIOptions.DocumentTitle = "ASP.NET React";
    swaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API serving a very simple Post model.");
    swaggerUIOptions.RoutePrefix = String.Empty;
});

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

app.MapGet("/get-all-posts", async () => await PostsRepository.GetPostsAsync())
    .WithTags("Posts Endpoints");

app.MapGet("/get-post-by-id/{postId}", async (int postId) =>
{
    Post postToReturn = await PostsRepository.GetPostByIdAsync(postId);

    if(postToReturn != null)
    {
        return Results.Ok(postToReturn);
    }
    else
    {
        return Results.BadRequest();
    }
})
    .WithTags("Posts Endpoints");

app.MapPost("/create-post", async (Post postToCreate) =>
{
    bool createSuccessful = await PostsRepository.CreatePostAsync(postToCreate);

    if(createSuccessful)
    {
        return Results.Ok("Post created successfully");
    }
    else
    {
        return Results.BadRequest();
    }
})
    .WithTags("Posts Endpoints");


app.MapPut("/update-post", async (Post postToUpdate) =>
{
    bool updateSuccessful = await PostsRepository.UpdatePostAsync(postToUpdate);
    if (updateSuccessful)
    {
        return Results.Ok("Post updated.");
    }
    else
    {
        return Results.BadRequest();
    }
})
    .WithTags("Posts Endpoints");



app.MapDelete("/delete-post-by-id/{postId}", async (int postId) =>
{
    bool deletedSuccessfully = await PostsRepository.DeletePostAsync(postId);

    if(deletedSuccessfully)
    {
        return Results.Ok("Post deleted successfully");
    }
    else
    {
        return Results.BadRequest();
    }
})
    .WithTags("Posts Endpoints");



app.Run();