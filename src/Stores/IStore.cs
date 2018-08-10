using System;
using System.Collections.Generic;
using CallGate.Documents;

namespace CallGate.Stores
{
    public interface IStore<TDocument> where TDocument : IDocument
    {
        bool Any();
        
        TDocument Get(Guid id);
        
        IEnumerable<TDocument> GetAll();
        
        TDocument Add(TDocument document);

        TDocument AddToBus(TDocument document);
        
        void Update(TDocument document);

        void UpdateToBus(TDocument document);
    }
}