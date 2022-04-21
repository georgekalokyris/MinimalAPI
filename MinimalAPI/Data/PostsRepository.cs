using Microsoft.EntityFrameworkCore;

namespace MinimalAPI.Data
{
    internal static class PostsRepository
    {
        internal async static Task<List<Post>> GetPostsAsync()
        {
            using (var db = new AppDbContext())
            {
                return await db.Posts.ToListAsync();
            }
        }

        internal async static Task<Post> GetPostByIdAsync(int postId)
        {
            using (var db = new AppDbContext())
            {
                return await db.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
            }
        }

        internal async static Task<bool> CreatePostAsync(Post post)
        {
            using(var db = new AppDbContext())
            {
                try
                {
                    await db.Posts.AddAsync(post);

                    return await db.SaveChangesAsync() >= 1;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        
        internal async static Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            using(var db = new AppDbContext())
            {
                try
                {
                    db.Posts.Update(postToUpdate);

                    return await db.SaveChangesAsync() >= 1;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        
        internal async static Task<bool> DeletePostAsync(int postId)
        {
            using(var db = new AppDbContext())
            {
                try
                {
                    Post postToDelete = await GetPostByIdAsync(postId);

                    db.Remove(postToDelete);

                    return await db.SaveChangesAsync() >= 1;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

    }
}
