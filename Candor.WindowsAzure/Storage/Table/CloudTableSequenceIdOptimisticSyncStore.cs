﻿using System;
using System.Linq;
using System.Collections.Generic;
using Candor.Data;
using Microsoft.WindowsAzure.Storage.Table;

namespace Candor.WindowsAzure.Storage.Table
{
    /// <summary>
    /// An optimistic sync store for sequence Ids that persists to an Azure table.
    /// </summary>
    public class CloudTableSequenceIdOptimisticSyncStore : ISequenceIdOptimisticSyncStore
    {
        private const String PropertyFinalCachedId = "FinalCachedId";
        private string _connectionName;
        private CloudTableProxy<SequenceIdSchema> _schemaTableProxy;
        private CloudTableProxy<DynamicTableEntity> _sequenceTableProxy;

        /// <summary>
        /// Gets or sets the connection name to the Azure Table storage.
        /// </summary>
        public string ConnectionName
        {
            get { return _connectionName; }
            set
            {
                _connectionName = value;
                _schemaTableProxy = null;
            }
        }
        private CloudTableProxy<SequenceIdSchema> SchemaTableProxy
        {
            get
            {
                return _schemaTableProxy ?? (_schemaTableProxy = new CloudTableProxy<SequenceIdSchema>
                {
                    ConnectionName = ConnectionName,
                    PartitionKey = x => x.TableName.GetValidPartitionKey(),
                    RowKey = x => x.TableName.GetValidRowKey()
                });
            }
        }
        private CloudTableProxy<DynamicTableEntity> SequenceTableProxy
        {
            get
            {
                return _sequenceTableProxy ?? (_sequenceTableProxy = new CloudTableProxy<DynamicTableEntity>
                {
                    ConnectionName = ConnectionName,
                    PartitionKey = x => x.PartitionKey,
                    RowKey = x => x.RowKey,
                    TableName = "SequenceId"
                });
            }
        }
        /// <summary>
        /// Gets the sequence for a given table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public SequenceIdStore GetSequenceIdStore(string tableName)
        {
            var schema = SchemaTableProxy.Get(tableName.GetValidPartitionKey(), tableName.GetValidRowKey());
            if (schema == null || schema.Entity == null)
                return null;
            var sequence = SequenceTableProxy.Get(tableName.GetValidPartitionKey(), tableName.GetValidRowKey()) ??
                CreateSequenceFromSchema(tableName);
            return new SequenceIdStore
                {
                    FinalCachedId = sequence.Entity.Properties[PropertyFinalCachedId].StringValue,
                    Schema = schema.Entity
                };
        }
        /// <summary>
        /// Gets all sequence definitions available.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SequenceIdStore> GetSequenceIdStores()
        {
            var schemas = SchemaTableProxy.QueryPartitions("0", "zzzzz");
            var sequences = SequenceTableProxy.QueryPartitions("0", "zzzzz") ?? new List<TableEntityProxy<DynamicTableEntity>>();
            var stores = new List<SequenceIdStore>();
            foreach (var schema in schemas)
            {
                var sequence =
                    sequences.FirstOrDefault(x =>
                        String.Equals(schema.Entity.TableName, x.Entity[TableConstants.PartitionKey].StringValue,
                                      StringComparison.InvariantCultureIgnoreCase));
                if (sequence == null)
                {
                    sequence = new TableEntityProxy<DynamicTableEntity>(new DynamicTableEntity(schema.PartitionKey, schema.RowKey, "",
                        new Dictionary<string, EntityProperty>()));
                    sequence.Entity.Properties.Add(PropertyFinalCachedId, new EntityProperty(schema.Entity.SeedValue));
                    SequenceTableProxy.Insert(sequence.Entity); //this could throw if another thread created after SequenceTableProxy.QueryPartitions was called above.
                }
                stores.Add(new SequenceIdStore
                    {
                        FinalCachedId = sequence.Entity.Properties[PropertyFinalCachedId].StringValue,
                        Schema = schema.Entity
                    });
            }
            return stores;
        }
        /// <summary>
        /// Saves the specified sequence schema, overwriting the existing schema if applicable.
        /// The schema does not contain the current latest sequence value.
        /// </summary>
        /// <param name="sequence"></param>
        public void InsertOrUpdate(SequenceIdSchema sequence)
        {
            SchemaTableProxy.InsertOrUpdate(sequence);
        }
        /// <summary>
        /// Gets the current sequence value for a given table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public OptimisticSyncData GetData(string tableName)
        {
            var sequence = SequenceTableProxy.Get(tableName.GetValidPartitionKey(), tableName.GetValidRowKey()) ??
                           CreateSequenceFromSchema(tableName);
            return new OptimisticSyncData
                {
                    TableName = tableName,
                    ConcurrencyKey = sequence.ETag,
                    Data = sequence.Entity.Properties[PropertyFinalCachedId].StringValue
                };
        }

        private TableEntityProxy<DynamicTableEntity> CreateSequenceFromSchema(string tableName)
        {
            var schema = SchemaTableProxy.Get(tableName.GetValidPartitionKey(), tableName.GetValidRowKey());
            if (schema == null)
                throw new ArgumentException(String.Format("Sequence '{0}' is not defined.  Cannot get the latest reserved key value.", tableName));

            var sequence = new TableEntityProxy<DynamicTableEntity>(new DynamicTableEntity(schema.PartitionKey, schema.RowKey, "",
                new Dictionary<string, EntityProperty>()));
            sequence.Entity.Properties.Add(PropertyFinalCachedId, new EntityProperty(schema.Entity.SeedValue));
            SequenceTableProxy.Insert(sequence.Entity);
            //Insert would throw if another thread created after SequenceTableProxy.Get was called above.  Only one thread can create the sequence.
            //  - a lock would not help, since the other thread may be in another role (web or worker) instance
            return sequence;
        }
        /// <summary>
        /// Tries to write the current sequence value for a table.
        /// </summary>
        /// <param name="syncData">The table name and current value for the sequence.  Also an ETag representing
        /// the latest reserved value retrieved.  The ETag must match the curent stored value for the update/write
        /// to succeed.</param>
        /// <returns></returns>
        public bool TryWrite(OptimisticSyncData syncData)
        {
            var sequence = SequenceTableProxy.Get(syncData.TableName.GetValidPartitionKey(), syncData.TableName.GetValidRowKey()) ??
                           CreateSequenceFromSchema(syncData.TableName);

            if (sequence.ETag != syncData.ConcurrencyKey)
                return false; //caller can get data again and attempt a retry.

            sequence.Entity.Properties[PropertyFinalCachedId].StringValue = syncData.Data;
            SequenceTableProxy.Update(sequence);
            return true;
        }
    }
}
