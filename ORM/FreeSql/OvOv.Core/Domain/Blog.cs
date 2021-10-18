﻿using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace OvOv.Core.Domain
{
    public class Blog : ISoftDelete
    {
        public Blog()
        {
        }

        public Blog(string title, string content, DateTime createTime, bool isDeleted)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            CreateTime = createTime;
            IsDeleted = isDeleted;
        }

        /// <summary>
        /// 文章所在分类专栏Id
        /// </summary>
        public int? ClassifyId { get; set; }
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        public virtual Classify Classify { get; set; }
        [Column(StringLength = 50)]
        public string Title { get; set; }
        [Column(StringLength = 500)]
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// 分类
    /// </summary>
    public class Classify
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 分类专栏名称
        /// </summary>
        [Column(DbType = "varchar(50)")]
        public string ClassifyName { get; set; }

        public virtual List<Blog> Articles { get; set; }

    }
}
