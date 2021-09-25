using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;    //使用Session或Cookies功能都必需要加入參考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleLogin.Models;
using System.Threading.Tasks;

namespace SimpleLogin.Controllers
{
	public class LoginController : Controller
	{
		private readonly UserDbContext _db = new UserDbContext();

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult IndexOK()
		{
			if(HttpContext.Session.GetString("Login") != null)
			{
				TempData["SessionLogin"] = HttpContext.Session.GetString("Login");      //取得Session的值
			}

			if(HttpContext.Request.Cookies["cookieKey"] != null)
			{
				TempData["CookieLogin"] = HttpContext.Request.Cookies["cookieKey"];     //用於讀出Cookie的"值"
			}

			return View();
		}

		public IActionResult IndexFail()
		{
			return View();
		}

		public IActionResult SessionLogin()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SessionLogin(UserData _userData)
		{
			if ((_userData == null) && (!ModelState.IsValid)) return View();

			if ((_userData.UserName == "123") && (_userData.UserPassword == "123")){
				
				HttpContext.Session.SetString("Login", "OK! Welcome.");     //設定Session

				return RedirectToAction("IndexOK");
			}
			else
			{
				return RedirectToAction("IndexFail");
			}
			
		}

		public IActionResult CookieLogin()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CookieLogin(UserData _userData)
		{
			if ((_userData == null) && (!ModelState.IsValid)) return View();

			if ((_userData.UserName == "123") && (_userData.UserPassword == "123"))
			{

				HttpContext.Response.Cookies.Append("cookieKey", "cookieValue");    //用於新增Cookie

				return RedirectToAction("IndexOK");
			}
			else
			{
				return RedirectToAction("IndexFail");
			}
		}

		public IActionResult SqliteSessionLogin()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SqliteSessionLogin(UserData _userData)
		{
			if ((_userData == null) && (!ModelState.IsValid)) return View();

			var ListOne = from UserData in _db.UserData
						  where UserData.UserName == _userData.UserName && UserData.UserPassword == _userData.UserPassword
						  select UserData;

			UserData _result = ListOne.FirstOrDefault();

			if (_result == null)
			{
				return RedirectToAction("IndexFail");
			}

			HttpContext.Session.SetString("Login", "OK! Use Sqlite");

			return RedirectToAction("IndexOK");
		}

		public IActionResult SqliteCookieLogin()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SqliteCookieLogin(UserData _userData)
		{
			if ((_userData == null) && (!ModelState.IsValid)) return View();

			var ListOne = from UserData in _db.UserData
						  where UserData.UserName == _userData.UserName && UserData.UserPassword == _userData.UserPassword
						  select UserData;

			UserData _result = ListOne.FirstOrDefault();

			if (_result == null)
			{
				return RedirectToAction("IndexFail");
			}

			HttpContext.Response.Cookies.Append("cookieKey", "SqliteCookieValue");

			return RedirectToAction("IndexOK");
		}

		public IActionResult Logout()
		{
			HttpContext.Response.Cookies.Delete("cookieKey");   //用於刪除Cookie
			HttpContext.Session.Clear();                        //清除Session
			return RedirectToAction("Index");
		}
	}
}
