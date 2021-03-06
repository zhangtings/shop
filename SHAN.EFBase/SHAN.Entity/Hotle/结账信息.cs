﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SHAN.Entity
{
    [Table("account")]
    public class 账目信息 : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 酒店编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 入住时间
        /// </summary>
        public int InTime { get; set; }
        /// <summary>
        /// 离开时间
        /// </summary>
        public int OutTime { get; set; }
        /// <summary>
        /// 客人序号
        /// </summary>
        public int GId { get; set; }
        /// <summary>
        /// 房间序号
        /// </summary>
        public int RId { get; set; }

    }
}
