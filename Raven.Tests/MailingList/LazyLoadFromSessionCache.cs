﻿using Raven.Client.Embedded;
using Xunit;

namespace Raven.Tests.MailingList
{
	public class LazyLoadFromSessionCache
	{
		[Fact]
		public void Stored_doc_can_be_lazy_loaded()
		{
			using (var documentStore = new EmbeddableDocumentStore { RunInMemory = true })
			{
				documentStore.Initialize();
				string id = "Id";
				using (var session = documentStore.OpenSession())
				{
					session.Store(new Doc { Id = id });
					var doc = session.Load<Doc>(id);
					Assert.NotNull(doc); // Passes
				}

				using (var session = documentStore.OpenSession())
				{
					session.Store(new Doc { Id = id });
					var doc = session.Advanced.Lazily.Load<Doc>(id);
					Assert.NotNull(doc.Value); // Fails
				}
			}
		}

		public class Doc
		{
			public string Id { get; set; }
		} 
	}
}