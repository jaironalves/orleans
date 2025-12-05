# Redis Operations Centralization Refactoring

## Overview

This refactoring centralizes Redis operations across all Orleans Redis providers into a single `RedisOperationsManager` class. This improves code maintainability, reduces duplication, and provides a consistent API for Redis operations.

## What Changed

### New Component: RedisOperationsManager

A new class `RedisOperationsManager` has been created in each Redis project to centralize common Redis operations:

- **Hash Operations**: `HashGetAllAsync`, `HashGetAsync`, `HashSetAsync`, `HashDeleteAsync`
- **Lua Script Operations**: `ScriptEvaluateAsync`
- **Stream Operations**: `StreamAddAsync`, `StreamRange`, `StreamDeleteAsync`
- **String Operations**: `StringGetAsync`, `StringSetAsync`
- **Key Operations**: `KeyDeleteAsync`, `KeyExpireAsync`
- **Set Operations**: `SetAddAsync`, `SetMembersAsync`, `SetRemoveAsync`
- **Sorted Set Operations**: `SortedSetRangeByValueAsync`, `SortedSetRemoveRangeByValueAsync`, `SortedSetAddAsync`
- **Transaction Operations**: `CreateTransaction`

### Updated Projects

1. **Orleans.Persistence.Redis**
   - `RedisGrainStorage` now uses `RedisOperationsManager` for hash and script operations
   - Operations: `HashGetAllAsync`, `ScriptEvaluateAsync`

2. **Orleans.Clustering.Redis**
   - `RedisMembershipTable` now uses `RedisOperationsManager` for hash, key, and transaction operations
   - Operations: `HashSetAsync`, `KeyExpireAsync`, `KeyDeleteAsync`, `HashGetAllAsync`, `HashDeleteAsync`

3. **Orleans.Reminders.Redis**
   - `RedisReminderTable` now uses `RedisOperationsManager` for sorted set, script, and key operations
   - Operations: `KeyExpireAsync`, `SortedSetRangeByValueAsync`, `SortedSetRemoveRangeByValueAsync`, `KeyDeleteAsync`, `ScriptEvaluateAsync`

4. **Orleans.GrainDirectory.Redis**
   - `RedisGrainDirectory` now uses `RedisOperationsManager` for string and script operations
   - Operations: `StringGetAsync`, `ScriptEvaluateAsync`

5. **Orleans.DurableJobs.Redis**
   - `RedisJobShardManager` now uses `RedisOperationsManager` for set, hash, key, and script operations
   - `RedisJobShard` now uses `RedisOperationsManager` for stream and script operations
   - Operations: `SetMembersAsync`, `HashGetAllAsync`, `ScriptEvaluateAsync`, `KeyDeleteAsync`, `SetRemoveAsync`, `StreamRange`

## Benefits

1. **Centralized Operations**: All Redis operations are now in a single, well-defined class per project
2. **Improved Maintainability**: Changes to Redis operation logic only need to be made in one place
3. **Consistent API**: All Redis providers use the same interface for common operations
4. **Easier Testing**: The centralized manager can be mocked or stubbed for testing
5. **Better Code Organization**: Clear separation between business logic and Redis operations

## Migration Notes

- No public API changes were made
- All existing functionality is preserved
- The refactoring is transparent to users of the Orleans Redis providers
- Each provider maintains its own instance of `RedisOperationsManager` in its respective namespace

## Files Modified

### Created Files
- `src/Redis/Orleans.Persistence.Redis/Storage/RedisOperationsManager.cs`
- `src/Redis/Orleans.Clustering.Redis/Storage/RedisOperationsManager.cs`
- `src/Redis/Orleans.Reminders.Redis/Storage/RedisOperationsManager.cs`
- `src/Redis/Orleans.GrainDirectory.Redis/Storage/RedisOperationsManager.cs`
- `src/Redis/Orleans.DurableJobs.Redis/Storage/RedisOperationsManager.cs`

### Modified Files
- `src/Redis/Orleans.Persistence.Redis/Storage/RedisGrainStorage.cs`
- `src/Redis/Orleans.Clustering.Redis/Storage/RedisMembershipTable.cs`
- `src/Redis/Orleans.Reminders.Redis/Storage/RedisReminderTable.cs`
- `src/Redis/Orleans.GrainDirectory.Redis/RedisGrainDirectory.cs`
- `src/Redis/Orleans.DurableJobs.Redis/RedisJobShardManager.cs`
- `src/Redis/Orleans.DurableJobs.Redis/RedisJobShard.cs`

## Testing

All existing tests should continue to pass as the refactoring maintains the same behavior. The centralized operations manager uses the exact same StackExchange.Redis API calls as the original implementations.
