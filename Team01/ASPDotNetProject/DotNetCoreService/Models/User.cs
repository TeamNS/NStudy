using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreService.Models
{
    public class User
    {
        /// <summary>
        /// 유저 UID PK
        /// </summary>
        [Key]
        public Int64 UniqueID { get; set; }

        [Required(ErrorMessage = "아이디를 입력하세요.")]
        public string LoginID { get; set; }

        [Required(ErrorMessage = "비밀번호를 입력하세요.")]
        public string LoginPassword { get; set; }

        [Required(ErrorMessage = "이름을 입력하세요.")]
        public string UserName { get; set; }
    }

    public class Friend
    {
        [Key]
        public Int64 UserUID { get; set; }

        public string UserName { get; set; }
    }
}
