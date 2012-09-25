--if an identifier contains a space, enclose it in [] brackets
--Order Details ==> [Order Details]

--grade recorder thing
--http://10.128.161.122/grades


--subqueries
use northwind
select productname
from northwind.dbo.products
where productid in  
	(select productid
	from northwind.dbo.[order details]
	where discount = 0.1)

/* Example SELECT statement using a subquery. */

use Northwind

SELECT ProductName,UnitPrice
FROM Northwind.dbo.Products --database.owner.object format
WHERE UnitPrice =

    (SELECT UnitPrice
    FROM Northwind.dbo.Products
    WHERE ProductName = 'Sir Rodney''s Scones')

--Select4
use pubs
PRINT '' /* line break */
PRINT 'Records containing quantities between 40 and 75 INCLUSIVE:'
PRINT ''
SELECT *
FROM Sales
WHERE Qty BETWEEN 40 AND 75