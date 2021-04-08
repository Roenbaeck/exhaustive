-- Set this to the path containing the ExhaustiveTypesXXXX.DLL files
DECLARE @path varchar(max) = 'S:\Sisula\exhaustive\';

EXEC sys.sp_configure 'clr enabled', 1;
reconfigure with override;

declare @version char(4) =
	case 
		when patindex('% 2[0-2][0-9][0-9] %', @@VERSION) > 0
		then substring(@@VERSION, patindex('% 2[0-2][0-9][0-9] %', @@VERSION) + 1, 4)
		else '????'
	end

-- since some version of 2017 assemblies must be explicitly whitelisted
IF(@version >= 2017 AND OBJECT_ID('sys.sp_add_trusted_assembly') IS NOT NULL) 
BEGIN
	CREATE TABLE #hash([hash] varbinary(64));
	EXEC('INSERT INTO #hash SELECT CONVERT(varbinary(64), ''0x'' + H, 1) FROM OPENROWSET(BULK ''' + @path + 'ExhaustiveTypes' + @version + '.SHA512'', SINGLE_CLOB) T(H);');
	DECLARE @hash varbinary(64);
	SELECT @hash = [hash] FROM #hash;
    IF NOT EXISTS(SELECT [hash] FROM sys.trusted_assemblies WHERE [hash] = @hash)
        EXEC sys.sp_add_trusted_assembly @hash, N'ExhaustiveTypes';
END
CREATE ASSEMBLY ExhaustiveTypes
AUTHORIZATION dbo
FROM @path + 'ExhaustiveTypes' + @version + '.dll'
WITH PERMISSION_SET = SAFE;

CREATE TYPE ExhaustiveInt EXTERNAL NAME ExhaustiveTypes.ExhaustiveInt;
CREATE TYPE ExhaustiveBigInt EXTERNAL NAME ExhaustiveTypes.ExhaustiveBigInt;
----------------------------------------------------
-- reset
DROP TABLE ExhaustiveTest;
DROP TYPE ExhaustiveInt;
DROP TYPE ExhaustiveBigInt;
DROP ASSEMBLY ExhaustiveTypes;
----------------------------------------------------
IF OBJECT_ID('ExhaustiveTest') IS NOT NULL
DROP TABLE ExhaustiveTest;

CREATE TABLE ExhaustiveTest (
	id int identity(1,1) not null,
	ex_int ExhaustiveInt not null, 
	ex_bigint ExhaustiveBigInt not null
);
insert into ExhaustiveTest (ex_int, ex_bigint)
values 
('100', '12345678901234567'), 
(null, null);

select 
	id, 
	ex_int, ex_int.ToString(), ex_int.IsUnknown(), 
	ex_bigint, ex_bigint.ToString(), ex_bigint.IsUnknown() 
from 
	ExhaustiveTest;


