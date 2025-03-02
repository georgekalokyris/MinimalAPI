﻿using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Data
{
    internal sealed class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(10000)]
        public string Content { get; set; } = string.Empty;
    }
}
