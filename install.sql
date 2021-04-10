-- reset types
IF TYPE_ID('ExhaustiveBit') IS NOT NULL DROP TYPE ExhaustiveBit;
IF TYPE_ID('ExhaustiveTinyInt') IS NOT NULL DROP TYPE ExhaustiveTinyInt;
IF TYPE_ID('ExhaustiveSmallInt') IS NOT NULL DROP TYPE ExhaustiveSmallInt;
IF TYPE_ID('ExhaustiveInt') IS NOT NULL DROP TYPE ExhaustiveInt;
IF TYPE_ID('ExhaustiveBigInt') IS NOT NULL DROP TYPE ExhaustiveBigInt;
IF TYPE_ID('ExhaustiveSingle') IS NOT NULL DROP TYPE ExhaustiveSingle;
IF TYPE_ID('ExhaustiveDouble') IS NOT NULL DROP TYPE ExhaustiveDouble;
IF TYPE_ID('ExhaustiveDecimal') IS NOT NULL DROP TYPE ExhaustiveDecimal;
IF TYPE_ID('ExhaustiveMoney') IS NOT NULL DROP TYPE ExhaustiveMoney;
IF TYPE_ID('ExhaustiveDateTime') IS NOT NULL DROP TYPE ExhaustiveDateTime;
IF TYPE_ID('ExhaustiveDateTime2') IS NOT NULL DROP TYPE ExhaustiveDateTime2;

-- reset assembly
IF EXISTS(SELECT name FROM sys.assemblies WHERE name = 'ExhaustiveTypes')
DROP ASSEMBLY ExhaustiveTypes;

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

CREATE TYPE ExhaustiveBit EXTERNAL NAME ExhaustiveTypes.ExhaustiveBit;
CREATE TYPE ExhaustiveTinyInt EXTERNAL NAME ExhaustiveTypes.ExhaustiveTinyInt;
CREATE TYPE ExhaustiveSmallInt EXTERNAL NAME ExhaustiveTypes.ExhaustiveSmallInt;
CREATE TYPE ExhaustiveInt EXTERNAL NAME ExhaustiveTypes.ExhaustiveInt;
CREATE TYPE ExhaustiveBigInt EXTERNAL NAME ExhaustiveTypes.ExhaustiveBigInt;
CREATE TYPE ExhaustiveSingle EXTERNAL NAME ExhaustiveTypes.ExhaustiveSingle;
CREATE TYPE ExhaustiveDouble EXTERNAL NAME ExhaustiveTypes.ExhaustiveDouble;
CREATE TYPE ExhaustiveDecimal EXTERNAL NAME ExhaustiveTypes.ExhaustiveDecimal;
CREATE TYPE ExhaustiveMoney EXTERNAL NAME ExhaustiveTypes.ExhaustiveMoney;
CREATE TYPE ExhaustiveDateTime EXTERNAL NAME ExhaustiveTypes.ExhaustiveDateTime;
CREATE TYPE ExhaustiveDateTime2 EXTERNAL NAME ExhaustiveTypes.ExhaustiveDateTime2;


