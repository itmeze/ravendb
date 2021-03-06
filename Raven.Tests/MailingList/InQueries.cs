﻿using Xunit;
using System.Linq;
using Raven.Client.Linq;

namespace Raven.Tests.MailingList
{
	public class InQueries : RavenTest
	{
		public class User
		{
			public string Country { get; set; }
		}

		[Fact]
		public void WhenQueryContainsQuestionMark()
		{
			using (var store = NewRemoteDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					session.Store(new User
					{
						Country = "Asia?"
					});
					session.SaveChanges();
				}

				using (var session = store.OpenSession())
				{
					var collection = session.Query<User>().Where(x => x.Country.In("Asia?", "Israel*")).ToList();
					Assert.NotEmpty(collection);
				}
			}
		}

		[Fact]
		public void WhenQueryContainsOneElement()
		{
			using (var store = NewRemoteDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					session.Store(new User
					{
						Country = "Asia"
					});
					session.SaveChanges();
				}

				using (var session = store.OpenSession())
				{
					Assert.NotEmpty(session.Query<User>()
						.Where(x => x.Country.In("Asia"))
						.ToList());
				}
			}
		}

		[Fact]
		public void WhenElementcontainsCommas()
		{
			using (var store = NewRemoteDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					session.Store(new User
					{
						Country = "Asia,Japan"
					});
					session.SaveChanges();
				}

				using (var session = store.OpenSession())
				{
					var collection = session.Query<User>().Where(x => x.Country.In("Asia,Japan")).ToList();

					Assert.NotEmpty(collection);
				}
			}
		}
	}
}