using AspNetCore_SPA_Tests.Builders;
using AspNetCore_SPA_Tests.Helpers;
using Domain.Model.Tasks;
using Interfaces.Tasks;
using NUnit.Framework;
using Repository;
using Repository.Base;
using Repository.Tasks;
using System;
using System.Linq;

namespace AspNetCore_SPA_Tests.Repository
{
    [TestFixture]
    [TestOf(typeof(TaskRepository))]
    public class TaskRepositoryTest
    {
        private readonly ITaskRepository _repo;
        private readonly SpaContext _context;

        public TaskRepositoryTest()
        {
            _context = InMemoryDb.GetContextWithData();
            _repo = new TaskRepository(_context);
        }

        [Test]
        [TestOf(typeof(BaseRepository<>))]
        public void Record_Is_Not_Persisted_Without_Calling_SaveAsync()
        {
            int numberOfItems = _context.Set<Task>().Count();
            Task newRecord = new TaskBuilder().WithName("NewTask").Build();

            DateTime insTsPreAdd = newRecord.InsTs;
            DateTime updTsPreAdd = newRecord.UpdTs;

            _repo.Add(newRecord);

            var records = _context.Set<Task>().ToList();

            Assert.AreEqual(numberOfItems, records.Count);
            Assert.That(!records.Contains(newRecord));
        }

        [Test]
        [TestOf(typeof(BaseRepository<>))]
        public void AddTest()
        {
            int numberOfItems = _context.Set<Task>().Count();
            Task newRecord = new TaskBuilder().WithName("NewTask").Build();

            DateTime insTsPreAdd = newRecord.InsTs;
            DateTime updTsPreAdd = newRecord.UpdTs;

            _repo.Add(newRecord);
            _repo.SaveAsync();

            var records = _context.Set<Task>().ToList();

            Assert.AreEqual(numberOfItems + 1, records.Count);
            Assert.Contains(newRecord, records);
            Assert.AreNotEqual(newRecord.InsTs, insTsPreAdd); // repo should change this date
            Assert.AreNotEqual(newRecord.UpdTs, updTsPreAdd); // repo should change this date
        }

        [Test]
        [TestOf(typeof(BaseRepository<>))]
        public void UpdateAsyncTest()
        {
            int numberOfItems = _context.Set<Task>().Count();
            Task record = _context.Set<Task>().First();
            record.UpdTs = DateTime.Today;

            Guid idPreAdd = record.Id;
            DateTime insTsPreAdd = record.InsTs;
            DateTime updTsPreAdd = record.UpdTs;

            _repo.UpdateAsync(record);

            var records = _context.Set<Task>().ToList();

            Assert.AreEqual(numberOfItems, records.Count);
            Assert.Contains(record, records);
            Assert.AreEqual(record.Id, idPreAdd);
            Assert.AreEqual(record.InsTs, insTsPreAdd); // repo should not change this date
            Assert.AreNotEqual(record.UpdTs, updTsPreAdd); // repo should change this date
        }

        [Test]
        [TestOf(typeof(BaseRepository<>))]
        public void DeleteTest()
        {
            int numberOfItems = _context.Set<Task>().Count();
            Task record = _context.Set<Task>().First();

            DateTime insTsPreAdd = record.InsTs;
            DateTime updTsPreAdd = record.UpdTs;
            bool isDeletedPreDel = record.IsDeleted;

            _repo.Delete(record);

            var records = _context.Set<Task>().ToList();

            Assert.AreEqual(numberOfItems, records.Count);
            Assert.Contains(record, records); // record exists in db after deletion
            Assert.AreNotEqual(isDeletedPreDel, record.IsDeleted);
            Assert.AreEqual(true, record.IsDeleted);
            Assert.AreEqual(record.InsTs, insTsPreAdd); // repo should change this date
            Assert.AreNotEqual(record.UpdTs, updTsPreAdd); // repo should change this date    
        }

        [Test]
        public void GetAll_Returns_Only_Not_Deleted_Objects()
        {
            var allRecordsInDb = _context.Set<Task>().ToList();
            int deletedRecords = allRecordsInDb.Count(x => x.IsDeleted);

            var getAllResult = _repo.GetAll().ToList();

            Assert.Greater(deletedRecords, 0);
            Assert.AreNotEqual(getAllResult.Count, allRecordsInDb);
            Assert.AreEqual(allRecordsInDb.Count - deletedRecords, getAllResult.Count);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context.Dispose();
        }
    }
}
