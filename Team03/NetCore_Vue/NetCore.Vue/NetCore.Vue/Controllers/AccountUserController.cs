using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using NetCore.Vue.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace NetCore.Vue.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountUserController : ControllerBase
	{

		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _Env;

		public AccountUserController(IConfiguration configuration, IWebHostEnvironment web)
		{
			_configuration = configuration;
			_Env = web;
		}

		// 서버로 부터 데이터를 조회하기위한 방식(Get)[Select]
		[HttpGet]
		public JsonResult Get()
		{
			string mySelectQuery = @"
				select UserID, UserPW, UserName from dbo.AccountUser
			";

			DataTable dt = new DataTable();

			using (MySqlConnection myConnection = new MySqlConnection())
			{

				myConnection.Open();

				using (MySqlCommand myCommand = new MySqlCommand(mySelectQuery, myConnection))
				{
					MySqlDataReader myReader;

					
					myReader = myCommand.ExecuteReader();

					dt.Load(myReader);
				}
			}

			return new JsonResult("AddSuccess");
			// return new JsonResult(dt);
		}

		[HttpPost]
		public JsonResult Post(AccountUser user)
		{
			//string mySelectQuery = @"
			//	insert into dbo.AccountUser
			//	values(@ID, @PW)
			//";

			DataTable dt = new DataTable();

			//using (MySqlConnection myConnection = new MySqlConnection())
			//{

			//	myConnection.Open();

			//	using (MySqlCommand myCommand = new MySqlCommand(mySelectQuery, myConnection))
			//	{
			//		MySqlDataReader myReader;


			//		myReader = myCommand.ExecuteReader();

			//		dt.Load(myReader);
			//	}
			//}

			return new JsonResult(dt);
		}

		[HttpPut]
		public JsonResult Put(AccountUser user)
		{
			// 여기에는 값의 변경(Update)가 들어올 경우 처리가 필요

			DataTable dt = new DataTable();

			//using (MySqlConnection myConnection = new MySqlConnection())
			//{

			//	myConnection.Open();

			//	using (MySqlCommand myCommand = new MySqlCommand(mySelectQuery, myConnection))
			//	{
			//		MySqlDataReader myReader;


			//		myReader = myCommand.ExecuteReader();

			//		dt.Load(myReader);
			//	}
			//}

			return new JsonResult(dt);
		}

		[HttpDelete("{No}")]
		public JsonResult Delete(int No)
		{
			// 여기에는 값의 삭제(Delete)가 들어올 경우 처리가 필요

			DataTable dt = new DataTable();

			//using (MySqlConnection myConnection = new MySqlConnection())
			//{

			//	myConnection.Open();

			//	using (MySqlCommand myCommand = new MySqlCommand(mySelectQuery, myConnection))
			//	{
			//		MySqlDataReader myReader;


			//		myReader = myCommand.ExecuteReader();

			//		dt.Load(myReader);
			//	}
			//}

			return new JsonResult(dt);
		}
		[Route("SaveFile")]
		[HttpPost]
		public JsonResult SaveFile()
		{
			try
			{
				var httpRequest = Request.Form;
				var postedFile = httpRequest.Files[0];
				string fileName = postedFile.FileName;
				var physicalPath = _Env.ContentRootPath + "/Photos/" + fileName;

				using(var stream = new FileStream(physicalPath, FileMode.Create))
				{
					postedFile.CopyTo(stream);
				}

				return new JsonResult(fileName);
			}
			catch(Exception)
			{
				return new JsonResult("");
			}
		}

	}
}
