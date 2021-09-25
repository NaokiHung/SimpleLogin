using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleLogin.Models
{
	public class UserData
	{
		public int id { get; set; }
		public string UserName { get; set; }
		public string UserNickName { get; set; }
		public string UserPassword { get; set; }
		public int UserRank { get; set; }
		public bool UserApproved { get; set; }

	}
}
