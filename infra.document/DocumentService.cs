using model.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace infra.document
{
    public class DocumentService : IDocumentRepository<Document, ObjectId>
    {
        private readonly IMongoCollection<Document> _documents;

        public DocumentService(IMongoClient client)
        {
            var database = client.GetDatabase("DocumentDB");
            var collection = database.GetCollection<Document>(nameof(Document));

            _documents = collection;
        }

        public async Task<ObjectId> Add(Document document)
        {
            await _documents.InsertOneAsync(document);

            return document.ID;
        }

        public async Task<Document> Get(string id)
        {
            var filter = Builders<Document>
                .Filter.Eq(d => d.ID, ObjectId.Parse(id));

            return await _documents
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Document>> GetAll()
        {
            return await _documents
                .Find(d => d.IsValid)
                .ToListAsync();
        }
    }
}