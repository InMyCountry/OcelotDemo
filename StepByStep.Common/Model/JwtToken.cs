using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StepByStep.Common.Model
{
    /// <summary>
    /// Jwt认证信息
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ID不能为空")]
        public string ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// 
        [Display(Name = "用户名")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空")]
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "手机号")]
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "手机号")]
        public string Email { get; set; }
        /// <summary>
        /// 身份
        /// </summary>
        [Display(Name = "身份")]
        public string Sub { get; set; }
    }
}
