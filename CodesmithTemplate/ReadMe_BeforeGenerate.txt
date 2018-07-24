MappingFile
----------------------------------------
1. After generating the MappingFile, do a replace for all for eLogixCore.Entity.Users to .User.
2. Do not remove Member and User mappings. They are custom mappings.

eLogixCore classes
----------------------------------------
1. Namespace should be eLogixCore.Entity
2. TransactionManager (generated in eLogixCore.Entity, should be placed under "eLogixCore" instead)
3. Do not delete/overwrite the classes "User" and "Member"!
4. Delete the class DataManager
5. No objects should have a relation to : ModeType, VehicleType and OperationType.