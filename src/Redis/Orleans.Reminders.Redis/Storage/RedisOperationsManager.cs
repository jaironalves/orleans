using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Orleans.Reminders.Redis
{
    /// <summary>
    /// Centralized manager for common Redis operations including streams, hashes, and Lua scripts.
    /// </summary>
    public class RedisOperationsManager
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisOperationsManager"/> class.
        /// </summary>
        /// <param name="database">The Redis database instance.</param>
        public RedisOperationsManager(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        #region Hash Operations

        /// <summary>
        /// Gets all fields and values in a hash.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <returns>An array of hash entries.</returns>
        public Task<HashEntry[]> HashGetAllAsync(RedisKey key)
        {
            return _database.HashGetAllAsync(key);
        }

        /// <summary>
        /// Gets the value of a hash field.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="field">The field name.</param>
        /// <returns>The value of the field, or Nil when the field is not found.</returns>
        public Task<RedisValue> HashGetAsync(RedisKey key, RedisValue field)
        {
            return _database.HashGetAsync(key, field);
        }

        /// <summary>
        /// Sets the specified field to the specified value in a hash.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="hashFields">The fields to set.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task HashSetAsync(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return _database.HashSetAsync(key, hashFields, flags);
        }

        /// <summary>
        /// Sets the specified field to the specified value in a hash.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="field">The field name.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="when">The condition to apply.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>True if the field was set, false if the condition was not met.</returns>
        public Task<bool> HashSetAsync(RedisKey key, RedisValue field, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return _database.HashSetAsync(key, field, value, when, flags);
        }

        /// <summary>
        /// Deletes the specified fields from a hash.
        /// </summary>
        /// <param name="key">The key of the hash.</param>
        /// <param name="field">The field to delete.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>True if the field was removed.</returns>
        public Task<bool> HashDeleteAsync(RedisKey key, RedisValue field, CommandFlags flags = CommandFlags.None)
        {
            return _database.HashDeleteAsync(key, field, flags);
        }

        #endregion

        #region Lua Script Operations

        /// <summary>
        /// Executes a Lua script.
        /// </summary>
        /// <param name="script">The Lua script to execute.</param>
        /// <param name="keys">The keys to pass to the script.</param>
        /// <param name="values">The values to pass to the script.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>The result of the script execution.</returns>
        public Task<RedisResult> ScriptEvaluateAsync(string script, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
        {
            return _database.ScriptEvaluateAsync(script, keys, values, flags);
        }

        #endregion

        #region Stream Operations

        /// <summary>
        /// Adds an entry to a stream.
        /// </summary>
        /// <param name="key">The key of the stream.</param>
        /// <param name="streamField">The field name in the stream entry.</param>
        /// <param name="streamValue">The value of the stream entry.</param>
        /// <param name="messageId">The message ID (use "*" for auto-generation).</param>
        /// <param name="maxLength">The maximum length of the stream.</param>
        /// <param name="useApproximateMaxLength">Whether to use approximate trimming.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>The ID of the added entry.</returns>
        public Task<RedisValue> StreamAddAsync(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            return _database.StreamAddAsync(key, streamField, streamValue, messageId, maxLength, useApproximateMaxLength, flags);
        }

        /// <summary>
        /// Reads entries from a stream in a given range.
        /// </summary>
        /// <param name="key">The key of the stream.</param>
        /// <param name="minId">The minimum ID (inclusive).</param>
        /// <param name="maxId">The maximum ID (inclusive).</param>
        /// <param name="count">The maximum number of entries to return.</param>
        /// <param name="messageOrder">The order in which to return messages.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>An array of stream entries.</returns>
        public StreamEntry[] StreamRange(RedisKey key, RedisValue? minId = null, RedisValue? maxId = null, int? count = null, Order messageOrder = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return _database.StreamRange(key, minId, maxId, count, messageOrder, flags);
        }

        /// <summary>
        /// Removes the specified entries from a stream.
        /// </summary>
        /// <param name="key">The key of the stream.</param>
        /// <param name="messageIds">The IDs of the messages to remove.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>The number of messages removed.</returns>
        public Task<long> StreamDeleteAsync(RedisKey key, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return _database.StreamDeleteAsync(key, messageIds, flags);
        }

        #endregion

        #region String Operations

        /// <summary>
        /// Gets the value of a key.
        /// </summary>
        /// <param name="key">The key to get.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>The value of the key, or Nil when the key does not exist.</returns>
        public Task<RedisValue> StringGetAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return _database.StringGetAsync(key, flags);
        }

        /// <summary>
        /// Sets the value of a key.
        /// </summary>
        /// <param name="key">The key to set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="expiry">The expiry time.</param>
        /// <param name="when">The condition to apply.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>True if the operation was successful.</returns>
        public Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return _database.StringSetAsync(key, value, expiry, when, flags);
        }

        #endregion

        #region Key Operations

        /// <summary>
        /// Deletes the specified keys.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>True if the key was removed.</returns>
        public Task<bool> KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return _database.KeyDeleteAsync(key, flags);
        }

        /// <summary>
        /// Deletes the specified keys.
        /// </summary>
        /// <param name="keys">The keys to delete.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>The number of keys that were removed.</returns>
        public Task<long> KeyDeleteAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return _database.KeyDeleteAsync(keys, flags);
        }

        /// <summary>
        /// Sets a timeout on a key.
        /// </summary>
        /// <param name="key">The key to set the timeout on.</param>
        /// <param name="expiry">The timeout to set.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>True if the timeout was set, false if the key does not exist.</returns>
        public Task<bool> KeyExpireAsync(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            return _database.KeyExpireAsync(key, expiry, flags);
        }

        #endregion

        #region Set Operations

        /// <summary>
        /// Adds the specified member to the set stored at key.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>True if the value was added to the set.</returns>
        public Task<bool> SetAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return _database.SetAddAsync(key, value, flags);
        }

        /// <summary>
        /// Returns all the members of the set stored at key.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>An array of all members in the set.</returns>
        public Task<RedisValue[]> SetMembersAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return _database.SetMembersAsync(key, flags);
        }

        /// <summary>
        /// Removes the specified member from the set stored at key.
        /// </summary>
        /// <param name="key">The key of the set.</param>
        /// <param name="value">The value to remove.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>True if the member was removed from the set.</returns>
        public Task<bool> SetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return _database.SetRemoveAsync(key, value, flags);
        }

        #endregion

        #region Sorted Set Operations

        /// <summary>
        /// Returns all the elements in the sorted set at key with a value between min and max (lexicographically).
        /// </summary>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="exclude">The exclusion flags.</param>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="take">The number of elements to take.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>An array of elements in the specified range.</returns>
        public Task<RedisValue[]> SortedSetRangeByValueAsync(RedisKey key, RedisValue min = default, RedisValue max = default, Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return _database.SortedSetRangeByValueAsync(key, min, max, exclude, skip, take, flags);
        }

        /// <summary>
        /// Removes all elements in the sorted set stored at key between the lexicographical range min and max.
        /// </summary>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="exclude">The exclusion flags.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>The number of elements removed.</returns>
        public Task<long> SortedSetRemoveRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return _database.SortedSetRemoveRangeByValueAsync(key, min, max, exclude, flags);
        }

        /// <summary>
        /// Adds the specified member with the specified score to the sorted set stored at key.
        /// </summary>
        /// <param name="key">The key of the sorted set.</param>
        /// <param name="member">The member to add.</param>
        /// <param name="score">The score of the member.</param>
        /// <param name="when">The condition to apply.</param>
        /// <param name="flags">The command flags.</param>
        /// <returns>True if the value was added to the sorted set.</returns>
        public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return _database.SortedSetAddAsync(key, member, score, when, flags);
        }

        #endregion

        #region Transaction Operations

        /// <summary>
        /// Creates a transaction to execute multiple operations atomically.
        /// </summary>
        /// <param name="asyncState">The async state object.</param>
        /// <returns>A transaction instance.</returns>
        public ITransaction CreateTransaction(object? asyncState = null)
        {
            return _database.CreateTransaction(asyncState);
        }

        #endregion
    }
}
