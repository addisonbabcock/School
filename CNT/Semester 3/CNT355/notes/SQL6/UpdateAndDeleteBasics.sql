use test

--assumption is that a reviews table exists already!

DELETE Reviews --remove all rows
select * from Reviews --prove it!

--now insert some test data into Reviews to play with:
INSERT INTO Reviews
	(ReviewId,EmployeeId,ProductId,Date,ReviewText)
VALUES
	(1,1,1,GetDate(),'Great Product!  Tastes like chicken but much cheaper.')
GO

INSERT INTO Reviews
	(ReviewId,EmployeeId,ProductId,Date,ReviewText)
VALUES
	(2,1,2,GetDate(),'Tastes like dogfood.')
GO

INSERT INTO Reviews
	(ReviewId,EmployeeId,ProductId,Date,ReviewText)
VALUES
	(3,2,3,GetDate(),'Good value for the money.')
GO

INSERT INTO Reviews
	(EmployeeId,ReviewId,Date,ReviewText,ProductId) --note order of cols does not
	--have to match order in table; what is important is that the values occur in
	--the correct positions 
VALUES
	(5,4,GetDate(),'Good Product',6)
GO

select * from Reviews --check to see inserts worked

--------------------------------------------------------
--UPDATE theory:

UPDATE Reviews
SET ReviewText='Horrible taste' --change all rows

select * from Reviews --have a look

--to update selected rows, simply include a WHERE
UPDATE Reviews
SET ReviewText='Magnificent taste' --change all rows
WHERE EmployeeId=1 AND ProductId=2

select * from Reviews --have a look

--to UPDATE multiple fields:
UPDATE Reviews
SET 
	ReviewText='Great taste', --change all rows
	Date='Dec 25, 2003'
WHERE EmployeeId=1 AND ProductId=2

select * from Reviews --have a look
-------------------------------------

DELETE Reviews
WHERE EmployeeId=1 AND ProductId=2

select * from Reviews



