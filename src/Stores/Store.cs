using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Documents;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;

namespace CallGate.Stores
{
    public class Store<TDocument> : IStore<TDocument> where TDocument : class, IDocument
    {
        protected readonly IRethinkDbDelegateBus _rethinkDbDelegateBus;
        protected static readonly RethinkDB R = RethinkDB.R;
        protected readonly Connection Connection;
        protected readonly string DbName;
        protected readonly string TableName;
        
        protected Store(
            IRethinkDbConnectionFactory connectionFactory,
            IRethinkDbDelegateBus rethinkDbDelegateBus
        ){
            Connection = connectionFactory.CreateConnection();
            Connection.CheckOpen();
            
            DbName = connectionFactory.GetOptions().Database;
            TableName = typeof(TDocument).Name;

            _rethinkDbDelegateBus = rethinkDbDelegateBus;
        }
        
        public bool Any()
        {
            var empty = R.Db(DbName)
                .Table(TableName)
                .IsEmpty()
                .Run(Connection);
            
            return !empty;
        }
        
        public TDocument Get(Guid id)
        {
            Cursor<TDocument> result = R.Db(DbName)
                .Table(TableName)
                .Filter(id.ToString())[new {index = "Id"}]
                .Run<TDocument>(Connection);

            return result.SingleOrDefault();
        }

        public IEnumerable<TDocument> GetAll()
        {
            Cursor<TDocument> all = R.Db(DbName)
                .Table(TableName)
                .Run<TDocument>(Connection);

            return all.ToList();
        }
        
        public TDocument Add(TDocument document)
        {
            R.Db(DbName)
                .Table(TableName)
                .Insert(document)
                .RunResult(Connection);
            
            return document;
        }
        
        public TDocument AddToBus(TDocument document)
        {
            ReqlExpr CommandDelegate() => R.Db(DbName)
                .Table(TableName)
                .Insert(document);

            _rethinkDbDelegateBus.AddDelegateToRunResult(CommandDelegate);
            
            return document;
        }

        public void Update(TDocument document)
        {
            var documents = GetAllDocumentsById(document.GetId());

            if (documents.Count > 0)
            {
                R.Db(DbName)
                    .Table(TableName)
                    .Get(documents.First().GetId().ToString())
                    .Update(document)
                    .RunResult(Connection);
            }
        }
        
        public void UpdateToBus(TDocument document)
        {
            var documents = GetAllDocumentsById(document.GetId());

            if (documents.Count > 0)
            {
                ReqlExpr CommandDelegate() => R.Db(DbName)
                    .Table(TableName)
                    .Get(documents.First().GetId().ToString())
                    .Update(document);

                _rethinkDbDelegateBus.AddDelegateToRunResult(CommandDelegate);
            }
        }

        private IList<TDocument> GetAllDocumentsById(Guid id)
        {
            return R.Db(DbName)
                .Table(TableName)
                .GetAll(id.ToString())[new { index = "Id" }]
                .Run<TDocument>(Connection)
                .ToList();
        }
    }
}