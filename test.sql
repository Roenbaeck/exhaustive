IF OBJECT_ID('ExhaustiveTest') IS NOT NULL
DROP TABLE ExhaustiveTest;

DECLARE @an_int int;
DECLARE @unknown_int ExhaustiveInt = ExhaustiveInt::Type('Unknown');

CREATE TABLE ExhaustiveTest (
	id int identity(1,1) not null,
	ex_int ExhaustiveInt not null
);
insert into ExhaustiveTest (ex_int)
values 
('100'), (case when @an_int is null then @unknown_int else ExhaustiveInt::Parse(@an_int) end); 

select 
	*, 
	ex_int.Value as Value,
	ex_int.ToString() as String, 
	ex_int.IsType('Known') as isKnownType, 
	ex_int.IsType('NULL') as isNULLType, 
	ex_int.IsType('Unknown') as isUnknownType, 
	case when ex_int = ExhaustiveInt::Type('NULL') then 'Y' else 'N' end as equalsNULL,
	case when ex_int = ExhaustiveInt::Type('Unknown') then 'Y' else 'N' end as equalsUnknown
from 
	ExhaustiveTest;

